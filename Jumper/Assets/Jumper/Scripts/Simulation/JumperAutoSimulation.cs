using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperAutoSimulation : MonoBehaviour
{
    // Скрипт симуляции джампера
    [HideInInspector] public SimulationJumperPhysics SimulationJumperPhysics = null;

    [HideInInspector]
    public bool FoundPointMax = false;


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
        if (other.tag == "Block" && FoundPointMax)
        {
            SimulationJumperPhysics.TransformEnemyObject = other.GetComponent<CheckCollider>().TransformEnemyObject;
            //SimulationJumperPhysics.CheckJumperStop = true;
        }
    }
    
}
