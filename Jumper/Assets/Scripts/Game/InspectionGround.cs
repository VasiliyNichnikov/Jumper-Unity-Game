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

    private void Update()
    {
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10, Color.black);
        RaycastHit hit;
        
        if (Physics.SphereCast(transform.position, 0.18f / 2, transform.TransformDirection(Vector3.down), out hit, 0.5f))
        {
            print(hit.collider.name);
        }
        
        // Debug.DrawRay(Physics.SphereCast(transform.position, 2,  transform.TransformDirection(Vector3.forward) * 10, out hit));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "ground")
        {
            //print("Ground:" + other.gameObject.name);
        }
    }
    
    private void OnCollisionStay(Collision other)
    {
        //if (other.collider.tag == "ground")
        //{
        // Vector3 targetDir = transform.position - other.transform.position;
        // float angle = Vector3.Angle(targetDir, Vector3.right);
        // print(angle);
        
        //rint(other.gameObject.name + " Enter"); 
        _managingJumper.StartAnimationTopJumper();
        _managingJumper.ChangeRigidbodyKinematic(true);
        IsGround = true;
        print(IsGround);
        //}
    }

    public float GetHeight()
    {
        return transform.position.y;
    }
}
