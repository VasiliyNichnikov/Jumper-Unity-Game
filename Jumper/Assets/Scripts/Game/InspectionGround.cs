using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Данный класс проверяет место на какой поверхности находится джампер
public class InspectionGround : MonoBehaviour
{
    // Rigidbody игрока
    private Rigidbody _rigidbodyJumper = null;

    private Transform _thisTransform = null;
    

    private void Start()
    {
        _thisTransform = transform;
        _rigidbodyJumper = GetComponent<Rigidbody>();
    }

<<<<<<< HEAD
    public IEnumerator CheckGround()
=======
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
>>>>>>> master
    {
        while (true)
        {
<<<<<<< HEAD
            RaycastHit hit;
            Debug.DrawRay(_thisTransform.position, _thisTransform.TransformDirection(Vector3.down) * 10, Color.black); 
            if (Physics.Raycast(_thisTransform.position, _thisTransform.TransformDirection(Vector3.down), out hit, 0.09f)) 
            {
                
                if (hit.collider.tag == "ground" || hit.collider.tag == "Block")
                {
                    //var timeCount = 0.0f;
                    _rigidbodyJumper.isKinematic = true;
                    // while (_thisTransform.rotation.eulerAngles != new Vector3(0, 0, 0))
                    // {
                    //     print(_thisTransform.rotation.eulerAngles + ":" + new Vector3(0, 0, 0));
                    //     //print(_jumper.transform.rotation);
                    //     _thisTransform.rotation = Quaternion.Slerp(_thisTransform.rotation, 
                    //         Quaternion.Euler(0, 0, 0), timeCount);
                    //     // Должно умножаться в зависимости от скорости (Доработать)
                    //     timeCount += Time.deltaTime * 2f;
                    //     yield return null;
                    // }
                    // print("End Animation");
                    yield break;
                }
            }
            yield return null;
        }
    }
=======
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
        //}
    }
>>>>>>> master

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + transform.TransformDirection(Vector3.down) * 0.05f, 0.06242963f);
    }
}
