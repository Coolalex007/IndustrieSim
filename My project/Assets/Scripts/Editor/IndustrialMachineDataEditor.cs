using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IndustrialMachineData))]
public class IndustrialMachineDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        IndustrialMachineData machineData = (IndustrialMachineData)target;

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "Der JSON-Schluessel bestimmt, welcher gespeicherte Datensatz geladen wird. Fuer neue Cubes kann hier der Schluessel eines vorhandenen Datensatzes eingetragen werden.",
            MessageType.Info);

        if (GUILayout.Button("Gespeicherte JSON-Werte laden"))
        {
            Undo.RecordObject(machineData, "Maschinendaten aus JSON laden");
            bool loaded = machineData.LoadSavedValues();
            EditorUtility.SetDirty(machineData);
            EditorUtility.DisplayDialog(
                "IndustrieSim",
                loaded ? "Die gespeicherten Werte wurden geladen." : "Fuer diesen JSON-Schluessel wurde kein gespeicherter Datensatz gefunden.",
                "OK");
        }

        if (GUILayout.Button("Aktuelle Werte als JSON speichern"))
        {
            machineData.Save();
            EditorUtility.DisplayDialog("IndustrieSim", "Die aktuellen Werte wurden gespeichert.", "OK");
        }
    }

    [MenuItem("IndustrieSim/Maschinendaten zu Auswahl hinzufuegen oder laden")]
    private static void AddOrLoadForSelection()
    {
        foreach (GameObject selectedObject in Selection.gameObjects)
        {
            IndustrialMachineData machineData = selectedObject.GetComponent<IndustrialMachineData>();
            if (machineData == null)
            {
                machineData = Undo.AddComponent<IndustrialMachineData>(selectedObject);
            }

            if (string.IsNullOrWhiteSpace(machineData.StorageKey))
            {
                machineData.StorageKey = selectedObject.name;
            }

            machineData.LoadSavedValues();
            EditorUtility.SetDirty(machineData);
        }
    }

    [MenuItem("IndustrieSim/Maschinendaten zu Auswahl hinzufuegen oder laden", true)]
    private static bool ValidateAddOrLoadForSelection()
    {
        return Selection.gameObjects.Length > 0;
    }
}
