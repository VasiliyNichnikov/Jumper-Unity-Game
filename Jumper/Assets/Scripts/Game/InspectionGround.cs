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
    private Transform _thisTransform = null;

    private void Start()
    {
        _thisTransform = transform;
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

    // private void OnCollisionExit(Collision other)
    // {
    //     print(other.gameObject.name + " Exit");
    //     IsGround = false;
    // }

    private void Update()
    {
        // RaycastHit hit;
        // float distanceHit = 10.0f;
        // Debug.DrawRay(_thisTransform.position, transform.TransformDirection(Vector3.down), Color.yellow, distanceHit);
        // Ray ray = new Ray(_thisTransform.position, transform.TransformDirection(Vector3.down * 0.01f));
        // Physics.Raycast(ray, out hit, distanceHit);
        //
        // //print(hit.collider.gameObject.name);
        // if (hit.collider != null && hit.collider.tag == "ground")
        // {
        //     Vector3 objectPosition = hit.collider.transform.position;
        //     print(Vector3.Distance(_thisTransform.position, objectPosition) + " " + hit.collider.gameObject.name);
        //     float distance = Vector3.Distance(_thisTransform.position, objectPosition);
        //     if (distance <= 1)
        //     {
        //         //print("Ground");
        //         _managingJumper.ChangeRigidbodyKinematic(true);
        //         IsGround = true;
        //     }
        // }
    }
}
