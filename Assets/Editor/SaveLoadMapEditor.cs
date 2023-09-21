using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveLoadMap))]

public class SaveLoadMapEditor : Editor
{
    
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveLoadMap saveLoadMap = (SaveLoadMap)target;
        saveLoadMap.Save();
        if (GUILayout.Button("Load"))
        {
            Debug.Log("Loading tile map");
            saveLoadMap.Load();
        }
        if (GUILayout.Button("Save"))
        {
            Debug.Log("Saving map");
            saveLoadMap.Save();
        }
    }
}
