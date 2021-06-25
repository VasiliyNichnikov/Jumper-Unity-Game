using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFan : MonoBehaviour
{
    [Header("Максимальная скорость")] [Range(100, 1000)]
    public float MaxSpeedRotation;

    [Header("Скорость разгона")] [Range(100, 1000)]
    public float SpeedRotation;
    
    [HideInInspector] // Включение и выключение вентилятора
    public bool OnAndOffFan = false;
    // Сохранение скорость 
    private float _saveSpeedRotation;
    private Transform _thisTransform;
    
    private void Start()
    {
        _saveSpeedRotation = 0;
        _thisTransform = transform;
    }
    
    private void Update()
    {
        if (OnAndOffFan && _saveSpeedRotation < MaxSpeedRotation)
        {
            _saveSpeedRotation += SpeedRotation * Time.deltaTime;
        }
        else if(!OnAndOffFan && _saveSpeedRotation > 0)
        {
            _saveSpeedRotation -= SpeedRotation * Time.deltaTime;
        }
        
        _thisTransform.Rotate(0, _saveSpeedRotation * Time.deltaTime, 0);
    }
}
