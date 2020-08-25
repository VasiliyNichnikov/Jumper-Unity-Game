using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class JumperAutoSimulation : MonoBehaviour
{
    // Скрипт симуляции джампера
    [HideInInspector] public SimulationJumperPhysics SimulationJumperPhysics;

    [HideInInspector]
    public bool FoundPointMax;


    private void OnTriggerEnter(Collider other)
    {
        StopJumper(other);
    }

    private void OnTriggerStay(Collider other)
    {
        StopJumper(other);
    }

    private void StopJumper(Collider other)
    {
        if (other.CompareTag("Object") && FoundPointMax)
        {
            //print("Other Object Name - " + other.name);
            SimulationJumperPhysics.TransformEmptyObject = other.GetComponent<CheckCollider>().TransformEnemyObject;
        }
            
    }
}
