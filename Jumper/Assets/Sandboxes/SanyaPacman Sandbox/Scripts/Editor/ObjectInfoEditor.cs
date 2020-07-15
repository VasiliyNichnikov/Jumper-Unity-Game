using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ObjectInfo))]
public class ObjectInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //кнопка AutoSize
        if (GUILayout.Button("AutoSize"))
        {
            var OI = (ObjectInfo)target;
            OI.AutoSize();
        }
        //кнопка Recalculate Sizes     
        GUILayout.Label("После ручного изменения параметров обязательно нажать кнопку");
        if (GUILayout.Button("Recalculate Sizes"))
        {
            var OI = (ObjectInfo)target;
            OI.CalculateSizes();
            Debug.Log("размеры пересчитаны");
        }
    }
}
