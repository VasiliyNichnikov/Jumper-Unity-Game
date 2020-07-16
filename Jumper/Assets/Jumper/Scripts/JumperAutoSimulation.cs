using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperAutoSimulation : MonoBehaviour
{
    // Скрипт траектории
    [HideInInspector] public Trajectory Trajectory = null;
    private ListModelsTest _listModelsTest = null;
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
            Trajectory.ObjectLandingJumper = other.gameObject;
            Trajectory.CheckJumperStop = true;
        }
        else if(other.tag == "Ground")
        {
            //print($"Ground");
        }
    }
    
}
