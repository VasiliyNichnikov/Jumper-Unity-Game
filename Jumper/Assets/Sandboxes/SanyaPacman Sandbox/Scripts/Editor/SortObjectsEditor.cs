using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SortObjects))]
public class SortObjectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //кнопка AutoSize
        if (GUILayout.Button("Sort by width"))
        {
            var OI = (SortObjects)target;
            OI.SortByWidth();
        }
        base.OnInspectorGUI();
        //кнопка AutoSize
        if (GUILayout.Button("Sort by height"))
        {
            var OI = (SortObjects)target;
            OI.SortObjectByHeight();
        }
        //кнопка Recalculate Sizes     
        //GUILayout.Label("После ручного изменения параметров обязательно нажать кнопку");
        //if (GUILayout.Button("Recalculate Sizes"))
        //{
        //    var OI = (ObjectInfo)target;
        //    OI.CalculateSizes();
        //    Debug.Log("размеры пересчитаны");
        //}
    }
}
