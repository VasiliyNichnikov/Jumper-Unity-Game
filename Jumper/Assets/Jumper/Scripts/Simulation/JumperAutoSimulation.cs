using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class JumperAutoSimulation : MonoBehaviour
{
    // Скрипт симуляции джампера
    [HideInInspector] public SimulationJumperPhysics SimulationJumperPhysics = null;

    [HideInInspector]
    public bool FoundPointMax = false;


    private void OnTriggerEnter(Collider other)
    {
       // Thread thread = new Thread(Test);
       // thread.Start();
                    
        StopJumper(other);
    }

    private void OnTriggerStay(Collider other)
    {
        StopJumper(other);
    }

    private void StopJumper(Collider other)
    {
        // var angleStart = -30;
        // for (int i = 0; i < 60; i++)
        // {
        //     transform.localRotation = Quaternion.Euler(0, 0, angleStart);
        //     print($"Collider other - {other.gameObject.name}");
        //     angleStart++;
        // }
        
        if (other.tag == "Object" && FoundPointMax)
        {
            SimulationJumperPhysics.TransformEmptyObject = other.GetComponent<CheckCollider>().TransformEnemyObject;
            //SimulationJumperPhysics.CheckJumperStop = true;
        }
    }

    private void Test()
    {
        
    }
    
    
}
