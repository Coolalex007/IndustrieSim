using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndustrialMachineDashboard : MonoBehaviour
{
    private static IndustrialMachineDashboard instance;
    private readonly Dictionary<string, TMP_Text> values = new Dictionary<string, TMP_Text>();
    private float nextRefreshTime;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null || GameObject.Find("IndustrialMachineDashboard") != null)
        {
            return;
        }

        GameObject dashboard = new GameObject("IndustrialMachineDashboard", typeof(RectTransform), typeof(Image), typeof(IndustrialMachineDashboard));
        dashboard.transform.SetParent(canvas.transform, false);
    }

    public static void RefreshNow()
    {
        if (instance != null)
        {
            instance.DiscoverMachines();
            instance.RefreshValues();
        }
    }

    private void Awake()
    {
        instance = this;
        BuildDashboard();
    }

    private void Start()
    {
        DiscoverMachines();
        RefreshValues();
    }

    private void Update()
    {
        if (Time.unscaledTime < nextRefreshTime)
        {
            return;
        }

        nextRefreshTime = Time.unscaledTime + 0.5f;
        DiscoverMachines();
        RefreshValues();
    }

    private void DiscoverMachines()
    {
        foreach (Renderer renderer in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
        {
            GameObject candidate = renderer.gameObject;
            if (candidate.GetComponent<Collider>() == null || !candidate.name.StartsWith("Cube"))
            {
                continue;
            }

            IndustrialMachineData machine = candidate.GetComponent<IndustrialMachineData>();
            if (machine == null)
            {
                machine = candidate.AddComponent<IndustrialMachineData>();
            }

            machine.InitializeDefaults(candidate.name);
        }
    }

    private void RefreshValues()
    {
        IndustrialMachineData[] machines = Object.FindObjectsByType<IndustrialMachineData>(FindObjectsSortMode.None);
        int active = 0;
        int maintenance = 0;
        int stopped = 0;
        float totalPower = 0f;
        float totalEfficiency = 0f;

        foreach (IndustrialMachineData machine in machines)
        {
            if (machine.status == "In Betrieb")
            {
                active++;
                totalPower += machine.powerConsumptionKw;
            }
            else if (!string.IsNullOrEmpty(machine.status) && machine.status.Contains("Wartung"))
            {
                maintenance++;
            }
            else
            {
                stopped++;
            }

            totalEfficiency += machine.efficiencyPercent;
        }

        SetValue("Maschinen gesamt", machines.Length.ToString());
        SetValue("Aktiv", active.ToString());
        SetValue("Wartung", maintenance.ToString());
        SetValue("Ausgeschaltet", stopped.ToString());
        SetValue("Verbrauch aktiv", totalPower.ToString("0.0") + " kW");
        SetValue("Effizienz Durchschnitt", machines.Length == 0 ? "-" : (totalEfficiency / machines.Length).ToString("0") + " %");
    }

    private void BuildDashboard()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = new Vector2(18f, -18f);
        rect.sizeDelta = new Vector2(270f, 210f);

        Image image = GetComponent<Image>();
        image.color = new Color(0.035f, 0.055f, 0.09f, 0.92f);
        image.raycastTarget = false;

        VerticalLayoutGroup layout = gameObject.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(16, 16, 12, 12);
        layout.spacing = 5f;
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        CreateText(transform, "FABRIK-DASHBOARD", 15, FontStyles.Bold, new Color(0.34f, 0.74f, 1f), 27f);
        CreateRow("Maschinen gesamt");
        CreateRow("Aktiv");
        CreateRow("Wartung");
        CreateRow("Ausgeschaltet");
        CreateRow("Verbrauch aktiv");
        CreateRow("Effizienz Durchschnitt");
    }

    private void CreateRow(string label)
    {
        GameObject row = new GameObject(label, typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(LayoutElement));
        row.transform.SetParent(transform, false);
        row.GetComponent<LayoutElement>().preferredHeight = 22f;
        HorizontalLayoutGroup layout = row.GetComponent<HorizontalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        CreateText(row.transform, label, 13, FontStyles.Normal, new Color(0.68f, 0.74f, 0.82f), 22f);
        TMP_Text value = CreateText(row.transform, "-", 13, FontStyles.Bold, Color.white, 22f);
        value.alignment = TextAlignmentOptions.MidlineRight;
        values[label] = value;
    }

    private TMP_Text CreateText(Transform parent, string content, int size, FontStyles style, Color color, float height)
    {
        GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        textObject.transform.SetParent(parent, false);
        TMP_Text text = textObject.GetComponent<TMP_Text>();
        text.text = content;
        text.fontSize = size;
        text.fontStyle = style;
        text.color = color;
        text.alignment = TextAlignmentOptions.MidlineLeft;
        text.raycastTarget = false;
        textObject.GetComponent<LayoutElement>().preferredHeight = height;
        return text;
    }

    private void SetValue(string key, string value)
    {
        values[key].text = value;
    }
}
