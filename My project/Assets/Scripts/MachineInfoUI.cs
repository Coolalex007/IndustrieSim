using TMPro;
using UnityEngine;

public class MachineInfoUI : MonoBehaviour
{
    public GameObject panel;

    public TMP_Text titleText;
    public TMP_Text typeText;
    public TMP_Text statusText;
    public TMP_Text productionText;
    public TMP_Text powerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HidePanel();
    }

    public void ShowMachineInfo(MachineData data)
    {
        Debug.Log("ShowMachineInfo wurde aufgerufen für: " + data.machineName);

        panel.SetActive(true);

        titleText.text = "Name: " + data.machineName; 
        typeText.text = "Art: " + data.machineType;
        statusText.text = "Status: " + data.workStatus;
        productionText.text = "Produktion: " + data.productionPerMinute + " / min";
        powerText.text = "Stromverbrauch: " + data.powerUsage + " kW";
    }
    
    public void HidePanel()
    {
        panel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
