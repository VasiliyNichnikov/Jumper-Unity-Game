using System;
using UnityEngine;

/// <summary>
/// Данный скрипт блокирует вращение джампера по оси Z
/// </summary>
public class LockingJumperAxisZ : MonoBehaviour
{
    [SerializeField] [Header("Максимальный угол")] private float _angle;

    [SerializeField] [Header("Максимальное расстояние до объекта")] [Range(0, 100)]
    private float _maximuDistance;
    
    private Transform _thisTransform;
    private Vector3 _startPositionRayCast, _endPointHit;
    private float _height;
    private float _radius;
    
    private void Start()
    {
        _thisTransform = transform;
        _height = GetComponent<CapsuleCollider>().height;
    }

    // private void FixedUpdate()
    // {
    //     CheckAngle();
    // }

    public void CheckAngle()
    {
        int layerMask = 1 << 9;
        for (float i = _height; i >= 0; i -= 0.01f)
        {
            RaycastHit hit;
            _startPositionRayCast = new Vector3(_thisTransform.position.x, _thisTransform.position.y + i, _thisTransform.position.z);
            if (Physics.Raycast(_startPositionRayCast, _thisTransform.TransformDirection(Vector3.left), out hit, _maximuDistance, layerMask))
            {
                // _startVector3 = new Vector3(_pivotTransform.position.x, _pivotTransform.position.y, _pivotTransform.position.z + 0.7f / 2);
                _endPointHit = hit.point;
                _angle = Vector3.Angle(Vector3.up, hit.point - _thisTransform.position);
                // _distance = hit.distance;
                if (_angle < 30)
                    break;
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        if (_endPointHit != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_startPositionRayCast, _endPointHit);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_thisTransform.position, _endPointHit);
        }
    }
    
}
