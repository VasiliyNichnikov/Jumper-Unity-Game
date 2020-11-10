using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAngle : MonoBehaviour
{
    [SerializeField] [Header("Угол")] [Range(-180, 180)]
    private float _angle;

    [SerializeField] private bool _checkAngle;
    
    [SerializeField] [Header("Расстояние")] private float _distance;

    [SerializeField] [Header("Пивот")] private Transform _pivotTransform;
    
    private Transform _thisTransform;

    private void Start()
    {
        _thisTransform = transform;
    }

    private Vector3 _startVector3;
    private Vector3 _endVector3;
    private Vector3 _startPos;

    private void FixedUpdate()
    {
        if (_checkAngle)
            CheckAngle();
    }
    
    private void CheckAngle()
    { 
        float height = 1.75f;

        for (float i = height; i >= -height; i -= 0.01f)
        {
            RaycastHit hit;
            _startPos = new Vector3(_thisTransform.position.x, _thisTransform.position.y + i, _thisTransform.position.z +  0.7f / 2);
            if (Physics.Raycast(_startPos, _thisTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                _startVector3 = new Vector3(_pivotTransform.position.x, _pivotTransform.position.y, _pivotTransform.position.z + 0.7f / 2);
                _endVector3 = hit.point;
                _angle = Vector3.Angle(Vector3.up, hit.point - _startVector3);
                _distance = hit.distance;
                break;
            }
        }
        _checkAngle = !_checkAngle;
    }

    private void OnDrawGizmos()
    {
        if (_endVector3 != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_startPos, _endVector3);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_startVector3, _endVector3);
        }
    }
}
