using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockingJumperAxisZ : MonoBehaviour
{
    [Header("Скрипт, который задает высоту и угол джампера, в зависимости от пальца")] [SerializeField]
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper = null;
    
    private void OnTriggerEnter(Collider other)
    {
        // Блокировка по оси Z
        if (other.tag == "Block" && !ClickTracking.JumpPlayer)
        {
            print(other.name);
            _calculatingAngleHeightJumper.LockingUnlockJumperAngle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Block" && !ClickTracking.JumpPlayer)
        {
            print(other.name);
            _calculatingAngleHeightJumper.LockingUnlockJumperAngle(true);
        }
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     // Блокировка по оси Z 
    //     if (other.tag == "Block" && !ClickTracking.JumpPlayer)
    //     {
    //         print(other.name);
    //         _calculatingAngleHeightJumper.LockingUnlockJumperAngle();
    //     }
    // }
}
