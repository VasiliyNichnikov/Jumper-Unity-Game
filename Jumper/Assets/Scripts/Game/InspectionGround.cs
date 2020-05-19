using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Данный класс проверяет место на какой поверхности находится джампер
public class InspectionGround : MonoBehaviour
{
    [Header("Скрипт, управляет джампером")] [SerializeField]
    private ManagingJumper _managingJumper = null;
    
    // Проверяет, находится ли джампер на земле или нет
    public static bool IsGround = false;
    
    // Очистка кэша
    //private Transform _thisTransform = null;

    private void Start()
    {
        //_thisTransform = transform;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "ground")
        {
            print("Ground:" + other.gameObject.name);
        }
    }
    
    private void OnCollisionStay(Collision other)
    {
        //if (other.collider.tag == "ground")
        //{
        print(other.gameObject.name + " Enter"); 
        _managingJumper.StartAnimationTopJumper();
        _managingJumper.ChangeRigidbodyKinematic(true);
        IsGround = true;
        //}
    }

    public float GetHeight()
    {
        return transform.position.y;
    }
    
    
}
