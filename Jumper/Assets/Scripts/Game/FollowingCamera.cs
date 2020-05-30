using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Данный скрипт следует за игроком
public class FollowingCamera : MonoBehaviour
{
    [Header("Объект за которым следит камера")] [SerializeField]
    private Transform _transformTarget = null;

    //[Header("Анимация закончилась")] public bool EndAnimtion = true;
    
    //[Header("Скрипт, который управлляет джампером")] [SerializeField]
    //private ManagingJumper _managingJumper = null;
    
    [Header("Скорость движения камеры")] [SerializeField]
    private float _speedCamera = .0f;

    [Header("Разница расстояния от камеры до объекта")] [SerializeField]
    private float DifferenceDistances = .0f;
    
    // Позиция камеры
    private Transform _thisTransform = null;

    private void Start()
    {
        _thisTransform = transform;
        StartCoroutine(AnimationStartCamera(-1));
    }

    // private void Update()
    // {
    //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.black);
    //     Debug.DrawRay(transform.position, targetDir * 1000, Color.red);
    // }

    // (Доделать поворот угла)
    private void LateUpdate()
    {
        // Позиция, до которой нужно двигаться 
        float positionEndX = _transformTarget.transform.position.x - DifferenceDistances;
        float positionEndY = _transformTarget.transform.position.y + 3;
        _thisTransform.localPosition = Vector3.MoveTowards(_thisTransform.localPosition,
            new Vector3(positionEndX, positionEndY, _thisTransform.localPosition.z), _speedCamera * Time.deltaTime);
        // _thisTransform.localPosition = new Vector3(Mathf.MoveTowards(_thisTransform.localPosition.x, positionEndX, 
        //     _speedCamera * Time.deltaTime), _thisTransform.localPosition.y, _thisTransform.localPosition.z); 
        
    }

    public IEnumerator AnimationStartCamera(float differenceX)
    {
        Vector3  differenceVector = _transformTarget.position - transform.position;
        differenceVector = new Vector3(differenceVector.x, 0, differenceVector.z);
        float angle = Vector3.Angle(differenceVector, transform.forward);
        float resultAngleY = _thisTransform.transform.eulerAngles.y + angle;
        if(differenceX > 0)
            resultAngleY = _thisTransform.transform.eulerAngles.y - angle;
        while (_thisTransform.transform.eulerAngles.y != resultAngleY)
        {
            differenceVector = _transformTarget.position - transform.position;
            differenceVector = new Vector3(differenceVector.x, 0, differenceVector.z);
            // Вычисляем разницу между камерой и джампером
            angle = Vector3.Angle(differenceVector, transform.forward);
            resultAngleY = _thisTransform.transform.eulerAngles.y + angle;
            if(differenceX > 0)
                resultAngleY = _thisTransform.transform.eulerAngles.y - angle;
            _thisTransform.rotation = Quaternion.Slerp(Quaternion.Euler(0, _thisTransform.transform.eulerAngles.y, 0), 
                Quaternion.Euler(0,resultAngleY, 0), 20 * Time.deltaTime);
            yield return null;
        }
    }
    
    // Возвращает актуальный угол, от 0 до 180 (Доделать)
    private Quaternion GetLessAngle(float angle)
    {
        float angleCameraY = _thisTransform.transform.eulerAngles.y;
        //print(angleCameraY + ": " + (angleCameraY - angle));
        //print(_thisTransform.transform.rotation);
        //print(Quaternion.Euler(0, angleCameraY - angle, 0));
        // if (angleCameraY > 180)
        // {
        //     //print(angleCameraY - 360);
        //     return angleCameraY - 360;
        //}

        return Quaternion.Euler(0, angleCameraY - angle, 0);
    }
    
}
