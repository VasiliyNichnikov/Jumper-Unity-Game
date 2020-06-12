using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данный скрипт придает скорость и управляет джампером во время полета
/// </summary>
public class FlightJumper : MonoBehaviour
{
    ///<summary>
    /// Переменные, которые отображаются в инспекторе 
    ///</summary>
    [Header("Максимальная скорость полета джампера")] [SerializeField] [Range(0, 100)]
    private float _maximumSpeedJump = .0f;
    
    [Header("Минимальная скорость полета джампера")] [SerializeField] [Range(0, 100)]
    private float _minimumSpeedJump = .0f;

    [Header("Скорость анимации приземления джампера")] [SerializeField] [Range(0, 100)]
    private float _speedLandingAnimationJumper = .0f;

    [Header("Вектор, который отслеживает угол и высоту джампера")] [SerializeField]
    private Transform _vector3HeightAngle = null;
    
    //[Header("Вектор, относительно которого происходит расчет скорости")] [SerializeField]
    //private Transform _vector3StartAngle = null;

    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>
    
    // rigidbody джампера
    private Rigidbody _rigidbodyJumper = null;

    private Transform _thisTransform = null;
    
    // Куротина, которая запущена (Все эти куротины связаны с анимациями) 
    private IEnumerator _animationJumper = null;

    private void Start()
    {
        _rigidbodyJumper = GetComponent<Rigidbody>();
        _thisTransform = transform;
        _rigidbodyJumper.isKinematic = true;
    }

    // Для теста
    private void FixedUpdate()
    {
        //print(_rigidbodyJumper.velocity);
        if (_rigidbodyJumper.velocity.y < 0 && _animationJumper == null)
        {
            _animationJumper = AnimationJumperLanding();
            StartCoroutine(_animationJumper);
        }
    }
    

    public void AddSpeedJumper()
    {
        _rigidbodyJumper.isKinematic = false;
        _rigidbodyJumper.AddForce(_vector3HeightAngle.position * 0.5f, ForceMode.Impulse);
    }

    // Меняет угол джампера, когда он начинает падать
    private IEnumerator AnimationJumperLanding()
    {
        Quaternion rotationEnd = Quaternion.Euler(0, 0, 5);
        while (_thisTransform.rotation.eulerAngles != rotationEnd.eulerAngles)
        {
            _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, rotationEnd,
                _speedLandingAnimationJumper * Time.deltaTime);
            
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 100, Color.black);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 3f))
            {
                if (hit.distance < 0.1f)
                {
                    _rigidbodyJumper.isKinematic = true;
                    StopCoroutine(_animationJumper);
                    _animationJumper = null;
                }
                _rigidbodyJumper.constraints = RigidbodyConstraints.FreezeRotationZ;
                //print(hit.distance);
                //_rigidbodyJumper.isKinematic = true;
                rotationEnd = Quaternion.Euler(0, 0, 0);
            }
            yield return null;
        }
    }
}
