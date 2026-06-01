using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class IndustrialMachineRecord
{
    public string objectName;
    public string machineName;
    public string machineId;
    public string manufacturer;
    public string model;
    public string status;
    public float utilizationPercent;
    public float temperatureCelsius;
    public float powerConsumptionKw;
    public int operatingHours;
    public string lastMaintenance;
    public string nextMaintenance;
    public string product;
    public int unitsPerHour;
    public float efficiencyPercent;
}

[Serializable]
public class IndustrialMachineStore
{
    public List<IndustrialMachineRecord> machines = new List<IndustrialMachineRecord>();
}

public static class IndustrialMachineStorage
{
    public static string FilePath
    {
        get { return Path.Combine(Application.persistentDataPath, "industrial-machines.json"); }
    }

    public static void Load(string objectName, IndustrialMachineData target)
    {
        IndustrialMachineRecord record = ReadStore().machines.Find(machine => machine.objectName == objectName);
        if (record == null)
        {
            return;
        }

        target.machineName = record.machineName;
        target.machineId = record.machineId;
        target.manufacturer = record.manufacturer;
        target.model = record.model;
        target.status = record.status;
        target.utilizationPercent = record.utilizationPercent;
        target.temperatureCelsius = record.temperatureCelsius;
        target.powerConsumptionKw = record.powerConsumptionKw;
        target.operatingHours = record.operatingHours;
        target.lastMaintenance = record.lastMaintenance;
        target.nextMaintenance = record.nextMaintenance;
        target.product = record.product;
        target.unitsPerHour = record.unitsPerHour;
        target.efficiencyPercent = record.efficiencyPercent;
    }

    public static void Save(string objectName, IndustrialMachineData source)
    {
        IndustrialMachineStore store = ReadStore();
        IndustrialMachineRecord record = store.machines.Find(machine => machine.objectName == objectName);
        if (record == null)
        {
            record = new IndustrialMachineRecord();
            record.objectName = objectName;
            store.machines.Add(record);
        }

        record.machineName = source.machineName;
        record.machineId = source.machineId;
        record.manufacturer = source.manufacturer;
        record.model = source.model;
        record.status = source.status;
        record.utilizationPercent = source.utilizationPercent;
        record.temperatureCelsius = source.temperatureCelsius;
        record.powerConsumptionKw = source.powerConsumptionKw;
        record.operatingHours = source.operatingHours;
        record.lastMaintenance = source.lastMaintenance;
        record.nextMaintenance = source.nextMaintenance;
        record.product = source.product;
        record.unitsPerHour = source.unitsPerHour;
        record.efficiencyPercent = source.efficiencyPercent;

        try
        {
            File.WriteAllText(FilePath, JsonUtility.ToJson(store, true));
        }
        catch (Exception exception)
        {
            Debug.LogError("Maschinendaten konnten nicht gespeichert werden: " + exception.Message);
        }
    }

    private static IndustrialMachineStore ReadStore()
    {
        if (!File.Exists(FilePath))
        {
            return new IndustrialMachineStore();
        }

        try
        {
            IndustrialMachineStore store = JsonUtility.FromJson<IndustrialMachineStore>(File.ReadAllText(FilePath));
            if (store == null)
            {
                return new IndustrialMachineStore();
            }

            if (store.machines == null)
            {
                store.machines = new List<IndustrialMachineRecord>();
            }

            return store;
        }
        catch (Exception exception)
        {
            Debug.LogWarning("Maschinendaten konnten nicht gelesen werden: " + exception.Message);
            return new IndustrialMachineStore();
        }
    }
}
