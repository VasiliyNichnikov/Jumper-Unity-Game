using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SpawnZoneInfo))]
public class SpawnZoneInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("AutoSize"))
        {
            var SZI = (SpawnZoneInfo)target;
            SZI.AutoSize();
        }
        if (GUILayout.Button("TestSpawn"))
        {
            var SZI = (SpawnZoneInfo)target;
            SZI.SpawnOnZone();
        }
    }
}
