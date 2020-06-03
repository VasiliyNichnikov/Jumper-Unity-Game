using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OpenFileButtonScript))]
public class Info : Editor
{
    public Vector3 Point1;
    public Vector3 Point2;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        OpenFileButtonScript myScript = (OpenFileButtonScript)target;
        if (GUILayout.Button("Open File"))
        {
            myScript.OpenDialog();
        }
    }

}
