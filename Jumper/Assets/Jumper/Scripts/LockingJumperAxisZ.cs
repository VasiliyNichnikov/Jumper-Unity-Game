using System;
using UnityEngine;

/// <summary>
/// Данный скрипт блокирует вращение джампера по оси Z
/// </summary>
public class LockingJumperAxisZ : MonoBehaviour
{
    ///<summary>
    /// Переменные, которые отображаются в инспекторе 
    ///</summary>
    // Скрипт, который задает высоту и угол джампера, в зависимости от пальца
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper = null;

    private void Start()
    {
        _calculatingAngleHeightJumper = GetComponentInParent<CalculatingAngleHeightJumper>();
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     // Блокировка по оси Z
    //     if (other.CompareTag("Object") && !ClickTracking.JumpPlayer)
    //     {
    //         _calculatingAngleHeightJumper.LockingUnlockJumperAngle();
    //     }
    // }
    
}
