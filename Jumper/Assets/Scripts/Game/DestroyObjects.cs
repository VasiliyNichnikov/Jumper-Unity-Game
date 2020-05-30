using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    [Header("Скрипт, который создает карту")] [SerializeField]
    private GenerationMap _generationMap = null;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Block")
        {
            print("Destroy");
            Destroy(other.gameObject);
            _generationMap.CreateObject();
        }
    }
}
