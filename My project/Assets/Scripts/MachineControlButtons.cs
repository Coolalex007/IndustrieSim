using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineControlButtons : MonoBehaviour
{
    private PlayerInteract player;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        MachineDetailsPanel panel = MachineDetailsPanel.GetOrCreate();
        if (panel == null || panel.transform.Find("MachineDetailsContent/MachineControlButtons") != null)
        {
            return;
        }

        Transform detailsContent = panel.transform.Find("MachineDetailsContent");
        if (detailsContent == null)
        {
            return;
        }

        GameObject controls = new GameObject("MachineControlButtons", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(LayoutElement), typeof(MachineControlButtons));
        controls.transform.SetParent(detailsContent, false);
        controls.GetComponent<LayoutElement>().preferredHeight = 67f;
    }

    private void Awake()
    {
        player = Object.FindFirstObjectByType<PlayerInteract>();
        BuildControls();
    }

    private void BuildControls()
    {
        VerticalLayoutGroup column = GetComponent<VerticalLayoutGroup>();
        column.spacing = 5f;
        column.childControlHeight = true;
        column.childControlWidth = true;
        column.childForceExpandHeight = false;
        column.childForceExpandWidth = true;

        CreateText(transform, "STEUERUNG", 11, FontStyles.Bold, new Color(0.34f, 0.74f, 1f), 20f);
        GameObject row = new GameObject("ControlRow", typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(LayoutElement));
        row.transform.SetParent(transform, false);
        row.GetComponent<LayoutElement>().preferredHeight = 34f;
        HorizontalLayoutGroup layout = row.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = 5f;
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        CreateButton(row.transform, "START", StartMachine, new Color(0.12f, 0.55f, 0.31f));
        CreateButton(row.transform, "STOP", StopMachine, new Color(0.58f, 0.18f, 0.18f));
        CreateButton(row.transform, "WARTEN", CompleteMaintenance, new Color(0.73f, 0.48f, 0.12f));
    }

    private void StartMachine()
    {
        IndustrialMachineData machine = GetSelectedMachine();
        if (machine == null)
        {
            return;
        }

        if (!machine.TryStart())
        {
            Debug.LogWarning("Die Maschine muss zuerst gewartet werden.");
        }

        RefreshUi();
    }

    private void StopMachine()
    {
        IndustrialMachineData machine = GetSelectedMachine();
        if (machine == null)
        {
            return;
        }

        machine.Stop();
        RefreshUi();
    }

    private void CompleteMaintenance()
    {
        IndustrialMachineData machine = GetSelectedMachine();
        if (machine == null)
        {
            return;
        }

        machine.CompleteMaintenance();
        RefreshUi();
    }

    private IndustrialMachineData GetSelectedMachine()
    {
        if (player == null || player.clickedObject == null)
        {
            return null;
        }

        return player.clickedObject.GetComponent<IndustrialMachineData>();
    }

    private void RefreshUi()
    {
        if (player == null || player.clickedObject == null)
        {
            return;
        }

        MachineDetailsPanel.GetOrCreate().Show(player.clickedObject);
        IndustrialMachineDashboard.RefreshNow();
    }

    private void CreateButton(Transform parent, string label, UnityEngine.Events.UnityAction action, Color color)
    {
        GameObject buttonObject = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
        buttonObject.transform.SetParent(parent, false);
        buttonObject.GetComponent<Image>().color = color;
        buttonObject.GetComponent<LayoutElement>().preferredHeight = 32f;
        Button button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(action);
        TMP_Text text = CreateText(buttonObject.transform, label, 11, FontStyles.Bold, Color.white, 32f);
        RectTransform rect = text.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        text.alignment = TextAlignmentOptions.Center;
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
}
