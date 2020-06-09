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

    public IEnumerator CheckGround()
    {
        while (true)
        {
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + transform.TransformDirection(Vector3.down) * 0.05f, 0.06242963f);
    }
}
