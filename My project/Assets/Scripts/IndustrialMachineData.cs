using UnityEngine;

public class IndustrialMachineData : MonoBehaviour
{
    [Header("JSON-Speicherung")]
    [SerializeField] private string storageKey;

    [Header("Identifikation")]
    public string machineName;
    public string machineId;
    public string manufacturer;
    public string model;

    [Header("Betrieb")]
    public string status;
    public float utilizationPercent;
    public float temperatureCelsius;
    public float powerConsumptionKw;
    public int operatingHours;
    public string lastMaintenance;
    public string nextMaintenance;

    [Header("Produktion")]
    public string product;
    public int unitsPerHour;
    public float efficiencyPercent;

    public string StorageKey
    {
        get { return string.IsNullOrWhiteSpace(storageKey) ? gameObject.name : storageKey; }
        set { storageKey = value; }
    }

    public void InitializeDefaults(string objectName)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
        {
            storageKey = objectName;
        }

        if (string.IsNullOrWhiteSpace(machineName))
        {
            bool isSecondMachine = objectName.Contains("(1)");
            machineName = isSecondMachine ? "CNC-Fraese 02" : "CNC-Fraese 01";
            machineId = isSecondMachine ? "CNC-002" : "CNC-001";
            manufacturer = "IndustrieSim Systems";
            model = "MX-500";
            status = isSecondMachine ? "Wartung faellig" : "In Betrieb";
            utilizationPercent = isSecondMachine ? 68f : 84f;
            temperatureCelsius = isSecondMachine ? 72.6f : 64.8f;
            powerConsumptionKw = isSecondMachine ? 11.7f : 13.4f;
            operatingHours = isSecondMachine ? 4290 : 3842;
            lastMaintenance = isSecondMachine ? "12.04.2026" : "18.05.2026";
            nextMaintenance = isSecondMachine ? "02.06.2026" : "18.07.2026";
            product = isSecondMachine ? "Getriebegehaeuse" : "Pumpenflansch";
            unitsPerHour = isSecondMachine ? 31 : 42;
            efficiencyPercent = isSecondMachine ? 73f : 91f;
        }

        LoadSavedValues();
    }

    public bool LoadSavedValues()
    {
        return IndustrialMachineStorage.Load(StorageKey, this);
    }

    public void Save()
    {
        IndustrialMachineStorage.Save(StorageKey, this);
    }
}
