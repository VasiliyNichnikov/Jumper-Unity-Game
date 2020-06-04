using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Данный класс проверяет место на какой поверхности находится джампер
public class InspectionGround : MonoBehaviour
{
    // Rigidbody игрока
    private Rigidbody _rigidbodyJumper = null;
    
    

    private void Start()
    {
        _rigidbodyJumper = GetComponent<Rigidbody>();
    }

    public IEnumerator CheckGround()
    {
        while (true)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10, Color.black); 
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.09f)) 
            {
                
                if (hit.collider.tag == "ground" || hit.collider.tag == "Block")
                {
                    _rigidbodyJumper.isKinematic = true;
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
