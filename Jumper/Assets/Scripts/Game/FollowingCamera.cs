using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Данный скрипт следует за игроком
public class FollowingCamera : MonoBehaviour
{
    [Header("Объект за которым следит камера")] [SerializeField]
    private Transform _transformTarget = null;
    
    [Header("Скрипт, который управлляет джампером")] [SerializeField]
    private ManagingJumper _managingJumper = null;
    
    [Header("Скорость движения камеры")] [SerializeField]
    private float _speedCamera = .0f;

    [Header("Разница расстояния от камеры до объекта")] [SerializeField]
    private float DifferenceDistances = .0f;
    
    
    // Позиция камеры
    private Transform _thisTransform = null;

    private void Start()
    {
        _thisTransform = transform;
    }

    private void Update()
    {
        //float distance = Vector3.Distance(new Vector3(_thisTransform.position.x, 0, _thisTransform.position.z), 
        //    new Vector3(_transformTarget.position.x, 0, _transformTarget.position.z));
        //print(distance);
        
        Vector3 targetDir = _transformTarget.position - transform.position;
        targetDir = new Vector3(targetDir.x, 0, targetDir.z);
        float angle = Vector3.Angle(targetDir, transform.forward);
        // Получаем угол от середины камеры до объекта
        //print(angle);
        //Quaternion.Angle(Quaternion.Euler(targetDir), Quaternion.Euler(transform.forward))
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.black);
        Debug.DrawRay(transform.position, targetDir * 1000, Color.red);
    }

    // (Доделать поворот угла)
    private void LateUpdate()
    {
        // Позиция, до которой нужно двигаться 
        float positionEndX = _transformTarget.transform.position.x - DifferenceDistances;
        _thisTransform.localPosition = new Vector3(Mathf.MoveTowards(_thisTransform.localPosition.x, positionEndX, 
            _managingJumper.GetSpeedUpJumper * Time.deltaTime), _thisTransform.localPosition.y, _thisTransform.localPosition.z); 
        
        Vector3  differenceVector = _transformTarget.position - transform.position;
        differenceVector = new Vector3(differenceVector.x, 0, differenceVector.z);
        // Вычисляем разницу между камерой и джампером
        float angle = Vector3.Angle(differenceVector, transform.forward);
        //print(angleCameraY - angle);
        //print(_transformTarget.rotation.y + ":" + Quaternion.);
        //_thisTransform.localRotation = Quaternion.Slerp(_thisTransform.rotation, 
        //    Quaternion.Euler(0,  GetLessAngle() - angle, 0), 2 * Time.deltaTime);
        //_thisTransform.LookAt(_transformTarget);
    }

    // Возвращает актуальный угол, от 0 до 180 (Доделать)
    private float GetLessAngle()
    {
        float angleCameraY = _thisTransform.transform.eulerAngles.y;
        // if (angleCameraY > 180)
        // {
        //     //print(angleCameraY - 360);
        //     return angleCameraY - 360;
        //}

        return angleCameraY;
    }
    
}
