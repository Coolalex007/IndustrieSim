using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineDetailsPanel : MonoBehaviour
{
    private static MachineDetailsPanel instance;
    private readonly Dictionary<string, Text> values = new Dictionary<string, Text>();
    private Text title;
    private Text subtitle;
    private Text status;
    private Font font;

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

        IndustrialMachineData data = selectedObject.GetComponent<IndustrialMachineData>();
        if (data == null)
        {
            data = selectedObject.AddComponent<IndustrialMachineData>();
        }

        data.InitializeDefaults(selectedObject.name);
        title.text = data.machineName;
        subtitle.text = data.machineId + "  |  " + data.manufacturer + " " + data.model;
        status.text = data.status;
        status.color = data.status == "In Betrieb" ? new Color(0.31f, 0.86f, 0.49f) : new Color(1f, 0.71f, 0.24f);
        SetValue("Auslastung", data.utilizationPercent.ToString("0") + " %");
        SetValue("Temperatur", data.temperatureCelsius.ToString("0.0") + " C");
        SetValue("Leistungsaufnahme", data.powerConsumptionKw.ToString("0.0") + " kW");
        SetValue("Betriebsstunden", data.operatingHours.ToString("N0") + " h");
        SetValue("Letzte Wartung", data.lastMaintenance);
        SetValue("Naechste Wartung", data.nextMaintenance);
        SetValue("Produkt", data.product);
        SetValue("Durchsatz", data.unitsPerHour + " Stk./h");
        SetValue("Effizienz", data.efficiencyPercent.ToString("0") + " %");
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void BuildPanel()
    {
        font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
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

        GameObject content = new GameObject("MachineDetailsContent", typeof(RectTransform), typeof(VerticalLayoutGroup));
        content.transform.SetParent(transform, false);
        RectTransform contentRect = content.GetComponent<RectTransform>();
        contentRect.anchorMin = Vector2.zero;
        contentRect.anchorMax = Vector2.one;
        contentRect.offsetMin = new Vector2(24f, 22f);
        contentRect.offsetMax = new Vector2(-24f, -22f);
        VerticalLayoutGroup layout = content.GetComponent<VerticalLayoutGroup>();
        layout.spacing = 7f;
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        CreateText(content.transform, "MASCHINENDATEN", 15, FontStyle.Bold, new Color(0.34f, 0.74f, 1f), 26f);
        title = CreateText(content.transform, "Maschine", 25, FontStyle.Bold, Color.white, 38f);
        subtitle = CreateText(content.transform, "", 12, FontStyle.Normal, new Color(0.67f, 0.74f, 0.82f), 24f);
        CreateText(content.transform, "STATUS", 11, FontStyle.Bold, new Color(0.52f, 0.62f, 0.72f), 20f);
        status = CreateText(content.transform, "", 18, FontStyle.Bold, Color.white, 30f);
        CreateSpacer(content.transform, 8f);
        CreateSection(content.transform, "BETRIEBSDATEN");
        CreateRow(content.transform, "Auslastung");
        CreateRow(content.transform, "Temperatur");
        CreateRow(content.transform, "Leistungsaufnahme");
        CreateRow(content.transform, "Betriebsstunden");
        CreateSpacer(content.transform, 8f);
        CreateSection(content.transform, "WARTUNG");
        CreateRow(content.transform, "Letzte Wartung");
        CreateRow(content.transform, "Naechste Wartung");
        CreateSpacer(content.transform, 8f);
        CreateSection(content.transform, "PRODUKTION");
        CreateRow(content.transform, "Produkt");
        CreateRow(content.transform, "Durchsatz");
        CreateRow(content.transform, "Effizienz");
    }

    private void CreateSection(Transform parent, string label)
    {
        CreateText(parent, label, 11, FontStyle.Bold, new Color(0.34f, 0.74f, 1f), 22f);
    }

    private void CreateRow(Transform parent, string label)
    {
        GameObject row = new GameObject(label, typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(LayoutElement));
        row.transform.SetParent(parent, false);
        row.GetComponent<LayoutElement>().preferredHeight = 24f;
        HorizontalLayoutGroup layout = row.GetComponent<HorizontalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        Text labelText = CreateText(row.transform, label, 13, FontStyle.Normal, new Color(0.68f, 0.74f, 0.82f), 24f);
        Text valueText = CreateText(row.transform, "-", 13, FontStyle.Bold, Color.white, 24f);
        valueText.alignment = TextAnchor.MiddleRight;
        values[label] = valueText;
    }

    private Text CreateText(Transform parent, string text, int size, FontStyle style, Color color, float height)
    {
        GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(Text), typeof(LayoutElement));
        textObject.transform.SetParent(parent, false);
        Text uiText = textObject.GetComponent<Text>();
        uiText.font = font;
        uiText.text = text;
        uiText.fontSize = size;
        uiText.fontStyle = style;
        uiText.color = color;
        uiText.alignment = TextAnchor.MiddleLeft;
        uiText.raycastTarget = false;
        textObject.GetComponent<LayoutElement>().preferredHeight = height;
        return uiText;
    }

    private static void CreateSpacer(Transform parent, float height)
    {
        GameObject spacer = new GameObject("Spacer", typeof(RectTransform), typeof(LayoutElement));
        spacer.transform.SetParent(parent, false);
        spacer.GetComponent<LayoutElement>().preferredHeight = height;
    }

    private void SetValue(string key, string value)
    {
        values[key].text = value;
    }
}
