using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineDetailsPanel : MonoBehaviour
{
    private static MachineDetailsPanel instance;
    private readonly Dictionary<string, TMP_Text> values = new Dictionary<string, TMP_Text>();
    private readonly Dictionary<string, TMP_InputField> inputs = new Dictionary<string, TMP_InputField>();
    private IndustrialMachineData selectedData;
    private GameObject detailsContent;
    private GameObject editContent;
    private TMP_Text title;
    private TMP_Text subtitle;
    private TMP_Text status;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        GetOrCreate();
    }

    public static MachineDetailsPanel GetOrCreate()
    {
        if (instance != null)
        {
            return instance;
        }

        GameObject panelObject = GameObject.Find("Panel");
        if (panelObject == null)
        {
            return null;
        }

        instance = panelObject.GetComponent<MachineDetailsPanel>();
        if (instance == null)
        {
            instance = panelObject.AddComponent<MachineDetailsPanel>();
        }

        return instance;
    }

    private void Awake()
    {
        instance = this;
        BuildPanel();
        gameObject.SetActive(false);
    }

    public void Show(GameObject selectedObject)
    {
        if (selectedObject == null)
        {
            Hide();
            return;
        }

        selectedData = selectedObject.GetComponent<IndustrialMachineData>();
        if (selectedData == null)
        {
            selectedData = selectedObject.AddComponent<IndustrialMachineData>();
        }

        selectedData.InitializeDefaults(selectedObject.name);
        ShowDetails();
        gameObject.SetActive(true);
    }

    public void RefreshIfSelected(IndustrialMachineData machine)
    {
        if (machine == null || selectedData != machine || !gameObject.activeSelf || editContent.activeSelf)
        {
            return;
        }

        ShowDetails();
    }

    public void Hide()
    {
        selectedData = null;
        gameObject.SetActive(false);
    }

    private void BuildPanel()
    {
        RectTransform panelRect = GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(1f, 0f);
        panelRect.anchorMax = new Vector2(1f, 1f);
        panelRect.pivot = new Vector2(1f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(390f, 0f);

        Image panelImage = GetComponent<Image>();
        if (panelImage != null)
        {
            panelImage.color = new Color(0.035f, 0.055f, 0.09f, 0.96f);
        }

        detailsContent = CreateVerticalContent("MachineDetailsContent");
        CreateText(detailsContent.transform, "MASCHINENDATEN", 15, FontStyles.Bold, new Color(0.34f, 0.74f, 1f), 26f);
        title = CreateText(detailsContent.transform, "Maschine", 25, FontStyles.Bold, Color.white, 38f);
        subtitle = CreateText(detailsContent.transform, "", 12, FontStyles.Normal, new Color(0.67f, 0.74f, 0.82f), 24f);
        CreateText(detailsContent.transform, "STATUS", 11, FontStyles.Bold, new Color(0.52f, 0.62f, 0.72f), 20f);
        status = CreateText(detailsContent.transform, "", 18, FontStyles.Bold, Color.white, 30f);
        CreateSection(detailsContent.transform, "BETRIEBSDATEN");
        CreateValueRow(detailsContent.transform, "Auslastung");
        CreateValueRow(detailsContent.transform, "Temperatur");
        CreateValueRow(detailsContent.transform, "Leistungsaufnahme");
        CreateValueRow(detailsContent.transform, "Betriebsstunden");
        CreateSection(detailsContent.transform, "WARTUNG");
        CreateValueRow(detailsContent.transform, "Letzte Wartung");
        CreateValueRow(detailsContent.transform, "Naechste Wartung");
        CreateSection(detailsContent.transform, "PRODUKTION");
        CreateValueRow(detailsContent.transform, "Produkt");
        CreateValueRow(detailsContent.transform, "Durchsatz");
        CreateValueRow(detailsContent.transform, "Effizienz");
        CreateButton(detailsContent.transform, "BEARBEITEN", BeginEdit, new Color(0.12f, 0.42f, 0.68f));

        editContent = CreateVerticalContent("MachineEditContent");
        CreateText(editContent.transform, "MASCHINENDATEN BEARBEITEN", 15, FontStyles.Bold, new Color(0.34f, 0.74f, 1f), 28f);
        CreateEditRow(editContent.transform, "Maschine");
        CreateEditRow(editContent.transform, "Maschinen-ID");
        CreateEditRow(editContent.transform, "Hersteller");
        CreateEditRow(editContent.transform, "Modell");
        CreateEditRow(editContent.transform, "Status");
        CreateEditRow(editContent.transform, "Auslastung");
        CreateEditRow(editContent.transform, "Temperatur");
        CreateEditRow(editContent.transform, "Leistungsaufnahme");
        CreateEditRow(editContent.transform, "Betriebsstunden");
        CreateEditRow(editContent.transform, "Letzte Wartung");
        CreateEditRow(editContent.transform, "Naechste Wartung");
        CreateEditRow(editContent.transform, "Produkt");
        CreateEditRow(editContent.transform, "Durchsatz");
        CreateEditRow(editContent.transform, "Effizienz");

        GameObject buttonRow = CreateRowObject(editContent.transform, "EditButtons", 34f);
        CreateButton(buttonRow.transform, "SPEICHERN", SaveChanges, new Color(0.12f, 0.55f, 0.31f));
        CreateButton(buttonRow.transform, "ABBRECHEN", ShowDetails, new Color(0.48f, 0.18f, 0.18f));
        editContent.SetActive(false);
    }

    private void ShowDetails()
    {
        if (selectedData == null)
        {
            return;
        }

        title.text = selectedData.machineName;
        subtitle.text = selectedData.machineId + "  |  " + selectedData.manufacturer + " " + selectedData.model;
        status.text = selectedData.status;
        status.color = selectedData.status == IndustrialMachineData.RunningStatus ? new Color(0.31f, 0.86f, 0.49f) : new Color(1f, 0.71f, 0.24f);
        SetValue("Auslastung", selectedData.utilizationPercent.ToString("0") + " %");
        SetValue("Temperatur", selectedData.temperatureCelsius.ToString("0.0") + " C");
        SetValue("Leistungsaufnahme", selectedData.powerConsumptionKw.ToString("0.0") + " kW");
        SetValue("Betriebsstunden", selectedData.operatingHours.ToString("N0") + " h");
        SetValue("Letzte Wartung", selectedData.lastMaintenance);
        SetValue("Naechste Wartung", selectedData.nextMaintenance);
        SetValue("Produkt", selectedData.product);
        SetValue("Durchsatz", selectedData.unitsPerHour + " Stk./h");
        SetValue("Effizienz", selectedData.efficiencyPercent.ToString("0") + " %");
        editContent.SetActive(false);
        detailsContent.SetActive(true);
    }

    private void BeginEdit()
    {
        if (selectedData == null)
        {
            return;
        }

        SetInput("Maschine", selectedData.machineName);
        SetInput("Maschinen-ID", selectedData.machineId);
        SetInput("Hersteller", selectedData.manufacturer);
        SetInput("Modell", selectedData.model);
        SetInput("Status", selectedData.status);
        SetInput("Auslastung", FormatNumber(selectedData.utilizationPercent));
        SetInput("Temperatur", FormatNumber(selectedData.temperatureCelsius));
        SetInput("Leistungsaufnahme", FormatNumber(selectedData.powerConsumptionKw));
        SetInput("Betriebsstunden", selectedData.operatingHours.ToString());
        SetInput("Letzte Wartung", selectedData.lastMaintenance);
        SetInput("Naechste Wartung", selectedData.nextMaintenance);
        SetInput("Produkt", selectedData.product);
        SetInput("Durchsatz", selectedData.unitsPerHour.ToString());
        SetInput("Effizienz", FormatNumber(selectedData.efficiencyPercent));
        detailsContent.SetActive(false);
        editContent.SetActive(true);
    }

    private void SaveChanges()
    {
        if (selectedData == null)
        {
            return;
        }

        selectedData.machineName = GetInput("Maschine");
        selectedData.machineId = GetInput("Maschinen-ID");
        selectedData.manufacturer = GetInput("Hersteller");
        selectedData.model = GetInput("Modell");
        selectedData.status = GetInput("Status");
        selectedData.utilizationPercent = ReadFloat("Auslastung", selectedData.utilizationPercent);
        selectedData.temperatureCelsius = ReadFloat("Temperatur", selectedData.temperatureCelsius);
        selectedData.powerConsumptionKw = ReadFloat("Leistungsaufnahme", selectedData.powerConsumptionKw);
        selectedData.operatingHours = ReadInt("Betriebsstunden", selectedData.operatingHours);
        selectedData.lastMaintenance = GetInput("Letzte Wartung");
        selectedData.nextMaintenance = GetInput("Naechste Wartung");
        selectedData.product = GetInput("Produkt");
        selectedData.unitsPerHour = ReadInt("Durchsatz", selectedData.unitsPerHour);
        selectedData.efficiencyPercent = ReadFloat("Effizienz", selectedData.efficiencyPercent);
        selectedData.Save();
        IndustrialMachineDashboard.RefreshNow();
        ShowDetails();
    }

    private GameObject CreateVerticalContent(string name)
    {
        GameObject content = new GameObject(name, typeof(RectTransform), typeof(VerticalLayoutGroup));
        content.transform.SetParent(transform, false);
        RectTransform contentRect = content.GetComponent<RectTransform>();
        contentRect.anchorMin = Vector2.zero;
        contentRect.anchorMax = Vector2.one;
        contentRect.offsetMin = new Vector2(24f, 22f);
        contentRect.offsetMax = new Vector2(-24f, -22f);
        VerticalLayoutGroup layout = content.GetComponent<VerticalLayoutGroup>();
        layout.spacing = 5f;
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;
        return content;
    }

    private void CreateSection(Transform parent, string label)
    {
        CreateText(parent, label, 11, FontStyles.Bold, new Color(0.34f, 0.74f, 1f), 20f);
    }

    private void CreateValueRow(Transform parent, string label)
    {
        GameObject row = CreateRowObject(parent, label, 22f);
        CreateText(row.transform, label, 13, FontStyles.Normal, new Color(0.68f, 0.74f, 0.82f), 22f);
        TMP_Text valueText = CreateText(row.transform, "-", 13, FontStyles.Bold, Color.white, 22f);
        valueText.alignment = TextAlignmentOptions.MidlineRight;
        values[label] = valueText;
    }

    private void CreateEditRow(Transform parent, string label)
    {
        GameObject row = CreateRowObject(parent, label, 28f);
        CreateText(row.transform, label, 12, FontStyles.Normal, new Color(0.68f, 0.74f, 0.82f), 28f);
        inputs[label] = CreateInputField(row.transform);
    }

    private GameObject CreateRowObject(Transform parent, string name, float height)
    {
        GameObject row = new GameObject(name, typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(LayoutElement));
        row.transform.SetParent(parent, false);
        row.GetComponent<LayoutElement>().preferredHeight = height;
        HorizontalLayoutGroup layout = row.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = 6f;
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        return row;
    }

    private TMP_InputField CreateInputField(Transform parent)
    {
        GameObject inputObject = new GameObject("Input", typeof(RectTransform), typeof(Image), typeof(TMP_InputField), typeof(LayoutElement));
        inputObject.transform.SetParent(parent, false);
        Image image = inputObject.GetComponent<Image>();
        image.color = new Color(0.1f, 0.14f, 0.2f, 1f);
        TMP_Text inputText = CreateText(inputObject.transform, "", 12, FontStyles.Normal, Color.white, 26f);
        RectTransform inputTextRect = inputText.GetComponent<RectTransform>();
        inputTextRect.anchorMin = Vector2.zero;
        inputTextRect.anchorMax = Vector2.one;
        inputTextRect.offsetMin = new Vector2(6f, 0f);
        inputTextRect.offsetMax = new Vector2(-6f, 0f);
        inputText.alignment = TextAlignmentOptions.MidlineRight;
        TMP_InputField input = inputObject.GetComponent<TMP_InputField>();
        input.textViewport = inputTextRect;
        input.textComponent = inputText;
        input.targetGraphic = image;
        inputObject.GetComponent<LayoutElement>().preferredHeight = 26f;
        return input;
    }

    private void CreateButton(Transform parent, string label, UnityEngine.Events.UnityAction action, Color color)
    {
        GameObject buttonObject = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
        buttonObject.transform.SetParent(parent, false);
        buttonObject.GetComponent<Image>().color = color;
        buttonObject.GetComponent<LayoutElement>().preferredHeight = 32f;
        Button button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(action);
        TMP_Text buttonText = CreateText(buttonObject.transform, label, 12, FontStyles.Bold, Color.white, 32f);
        RectTransform textRect = buttonText.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        buttonText.alignment = TextAlignmentOptions.Center;
    }

    private TMP_Text CreateText(Transform parent, string text, int size, FontStyles style, Color color, float height)
    {
        GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        textObject.transform.SetParent(parent, false);
        TMP_Text uiText = textObject.GetComponent<TMP_Text>();
        uiText.text = text;
        uiText.fontSize = size;
        uiText.fontStyle = style;
        uiText.color = color;
        uiText.alignment = TextAlignmentOptions.MidlineLeft;
        uiText.raycastTarget = false;
        textObject.GetComponent<LayoutElement>().preferredHeight = height;
        return uiText;
    }

    private void SetValue(string key, string value)
    {
        values[key].text = value;
    }

    private void SetInput(string key, string value)
    {
        inputs[key].text = value;
    }

    private string GetInput(string key)
    {
        return inputs[key].text.Trim();
    }

    private float ReadFloat(string key, float fallback)
    {
        float result;
        string value = GetInput(key).Replace(',', '.');
        return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result) ? result : fallback;
    }

    private int ReadInt(string key, int fallback)
    {
        int result;
        return int.TryParse(GetInput(key), out result) ? result : fallback;
    }

    private static string FormatNumber(float value)
    {
        return value.ToString("0.##", CultureInfo.InvariantCulture);
    }
}
