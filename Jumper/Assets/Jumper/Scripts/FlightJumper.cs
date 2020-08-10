﻿using System;
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
    [Header("Максимальная скорость полета джампера")] [SerializeField] [Range(0, 50)]
    private float _maximumSpeedFlightJumper = .0f;
    
    [Header("Скорость анимации полета джампера (После прыжка)")] [SerializeField] [Range(0, 100)]
    private float _speedFlightAnimationJumper = .0f;
    
    //[Header("Скорость анимации приземления джампера")] [SerializeField] [Range(0, 100)]
    //private float _speedLandingAnimationJumper = .0f;

    [Header("Скорость анимации после приземления и при возвращение джампера перпендикулярно земли")]
    [SerializeField]
    [Range(0, 100)]
    private float _speedAfterLandingAnimationJumper = .0f;

    [Header("Угол наклона джампера при падении")] [SerializeField] [Range(0, 45)]
    private float _angleIncidence = .0f;

    //[Header("Вектор для нахождения толчка")] [SerializeField]
    

    //[Header("Скрипт, который включает поражение игрока")] [SerializeField]
    //private GameOverPlayer _gameOverPlayer = null;

    [Header("Скрипт, который отслеживает нажатие")] [SerializeField]
    private ClickTracking _clickTracking = null;

    [Header("Скрипт, который настраивает отвечает за движение камеры")] [SerializeField]
    private CameraTracking _cameraTracking = null;

    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>

    // rigidbody джампера
    private Rigidbody _rigidbodyJumper = null;
    
    // очистка кэша
    private Transform _thisTransform = null;

    // Скрипт, который управляет углом и высотой джампера
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper = null;
    
    // Скрипт, который управляет симуляцией джампера
    private SimulationJumperPhysics _simulationJumperPhysics = null;

    // Переменная, которая хранит скорость поворота во время полета по оси Z
    private float _speedRotationJumperFlight = .0f;
    
    // Вектор для нахождения толчка
    private Transform _vector3TransformHeightAngle = null;
    
    // Переменная хранит конечный поворот
    private Quaternion _rotationEnd = Quaternion.identity;
    private bool _animationStartJumperEnd = false;
    private bool _landingCollider = false;
    
    // Для настроек джампера(Начало)
    public float ChangeMassJumper
    {
        set
        {
            if (value <= 1) _rigidbodyJumper.mass = value;
        }

        get
        {
            return _rigidbodyJumper.mass;
        }
    }

    public float ChangeMaximumSpeedJumper
    {
        set
        {
            if (value <= 100) _maximumSpeedFlightJumper = value;
        }
        get
        {
            return _maximumSpeedFlightJumper;
        }
    }

    public Transform ChoiceVector3TransformHeightAngle
    {
        set { _vector3TransformHeightAngle = value; }
    }
    
    // Для настроек джампера(Конец)

    // Возвращает скорость джампера по оси Y
    // public float GetVelocityAxesYJumper
    // {
    //     get { return _rigidbodyJumper.velocity.y; }
    // }
    
    // Возвращает максимальную скорость джампера
    public float GetMaximumSpeedJumper
    {
        get
        {
            return _maximumSpeedFlightJumper;
        }
    }
    
    // Возвращает среднюю скорость джампера
    // public float GetAverageSpeedJumper
    // {
    //     get
    //     {
    //         return GetSpeedJumper(_calculatingAngleHeightJumper.GerPercentHeightJumper, _maximumSpeedFlightJumper);
    //     }
    // }

    private void Start()
    {
        _rigidbodyJumper = GetComponent<Rigidbody>();
        _simulationJumperPhysics = GetComponent<SimulationJumperPhysics>();
        _calculatingAngleHeightJumper = GetComponent<CalculatingAngleHeightJumper>();
        _thisTransform = transform;
        //FreezePositionAndRotationJumper();
    }
    

    // Добавление скорости джамперу
    public void AddSpeedJumper()
    {
        var speedX = GetSpeedJumper(_calculatingAngleHeightJumper.GetPercentAngleJumper, _maximumSpeedFlightJumper);
        var speedY = GetSpeedJumper(_calculatingAngleHeightJumper.GerPercentHeightJumper, _maximumSpeedFlightJumper);
        _calculatingAngleHeightJumper.LockingUnlockJumperAngle(true);
        if (speedY > 0)
        {
            ClickTracking.JumpPlayer = true;
            Vector3 vectorDifference = _vector3TransformHeightAngle.position - _thisTransform.position;
            //print($"Угол наклона - {Vector3.Angle( vectorDifference, Vector3.left)}");
            Vector3 vectorForce =
                new Vector3(vectorDifference.x * speedX, vectorDifference.y * speedY, vectorDifference.z);
            Dictionary<string, float> dictionaryDistance =
                _simulationJumperPhysics.SimulationJumper(transform.position, vectorForce);
            float positionJumperEndAxesY = dictionaryDistance["finite_distance_axes_y"];
            _speedRotationJumperFlight = dictionaryDistance["speed_rotation_jumper_flight"];
            if (positionJumperEndAxesY != .0f)
                _cameraTracking.PositionY = positionJumperEndAxesY;
            _rigidbodyJumper.isKinematic = false;
            //FreezePositionAndRotationJumper(true);
            _rigidbodyJumper.AddForce(vectorForce, ForceMode.Impulse);
        }    
    }

    
    // Анимация начала анимации (Во время полета становится перпендикулярно земле)
    public IEnumerator AnimationStartJumper()
    {
        _rotationEnd = Quaternion.Euler(0, 0, 0);
        _animationStartJumperEnd = false;
        _landingCollider = false;
        while (_rigidbodyJumper.velocity.y >= 0 && ClickTracking.JumpPlayer)
        {
            _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _rotationEnd, _speedFlightAnimationJumper * Time.deltaTime);
            yield return null;
        }
        _animationStartJumperEnd = true;
        print("End AnimationJump");
        yield return AnimationSeveralDegreesRelationGround();
        
    }
    
    // Анимация во время полета. Начинает выстраивать джампер на несколько градусов относительно земли.
    private IEnumerator AnimationSeveralDegreesRelationGround()
    {
        // Нужно чтобы джампер не всегда крутился во время полета
        // var endPositionJump = Math.Abs(_thisTransform.position.x);
        // var differenceDistance = Mathf.Abs(endPositionJump - _startPositionJump);
        // if (differenceDistance > .5f)
        // {
        //     if (_thisTransform.rotation.z < 0)
        //         _rotationEnd = Quaternion.Euler(0, 0, _angleIncidence);
        //     else
        //         _rotationEnd = Quaternion.Euler(0, 0, -_angleIncidence);
        //     print($"Landing Collider - {_landingCollider}");
        //     while (!_landingCollider)
        //     {
        //         _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _rotationEnd,
        //             _speedLandingAnimationJumper * Time.deltaTime);
        //         yield return null;
        //     }
        // }
        //print($"Расстояние полета джампера - {_flightDistanceJumperAxesX}");
        if (_thisTransform.rotation.z < 0)
            _rotationEnd = Quaternion.Euler(0, 0, _angleIncidence);
        else
            _rotationEnd = Quaternion.Euler(0, 0, -_angleIncidence);
        //print($"Landing Collider - {_landingCollider}");
        while (!_landingCollider && ClickTracking.JumpPlayer)
        {
            _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _rotationEnd,
                _speedRotationJumperFlight / 1.35f * Time.deltaTime);
            yield return null;
        }
        
        print("End AnimationMiddleFlight");
        yield return AnimationReturnRotationJumper();
        
    }
    
    // Возвращает джампер перпендикулярно земле при посадке.
    private IEnumerator AnimationReturnRotationJumper()
    {
        _rotationEnd = Quaternion.Euler(0, 0, 0);
        while (_thisTransform.rotation != _rotationEnd && !_clickTracking.FingerInputScreen)
        {
            _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _rotationEnd, _speedAfterLandingAnimationJumper * Time.deltaTime);
            yield return null;
        }
        print("End ReturnRotationJumper");
    }

    //private bool _exitCollision = false;
    //private GameObject _otherGameobject = null;
    // Прикосновение к объекту
    private void OnCollisionEnter(Collision other){
        //ContactPoint contactPoint = other.contacts[0];
        //print($"contactPoint - {contactPoint}");
        //print(other.collider);
        if (_animationStartJumperEnd)
            EndAnimationJumper(other);
    }
    
    // Нахождение в объекте
    private void OnCollisionStay(Collision other)
    {
        //ContactPoint contactPoint = other.contacts[0];
        
        if (_animationStartJumperEnd)
            EndAnimationJumper(other);
    }
    


    // Анимация заканчивается
    private void EndAnimationJumper(Collision other)
    {
        _landingCollider = true;
        ClickTracking.JumpPlayer = false;
        _rigidbodyJumper.isKinematic = true;
        _animationStartJumperEnd = false;
        _calculatingAngleHeightJumper.StartCoroutine(_calculatingAngleHeightJumper.ReturnUpperPartJumper(true));
        // Проверка проиграл игрок или нет
        if (other.collider.tag != "Ground")
        {
            CheckCollider _checkCollider = other.gameObject.GetComponent<CheckCollider>();
            _checkCollider.CheckGameOver(other);
        }
        
        
        print("Animation End, start now");
    }
    
    // Запуск луча и определение расстояния до объекта
    // private void RayCastCheckSurface()
    // {
    //     RaycastHit hit;
    //
    //     if (Physics.Raycast(_thisTransform.position, _thisTransform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
    //     {
    //         if (hit.collider.tag == "Ground")
    //         {
    //             _gameOverPlayer.GameOverPlayerMethod();
    //         }
    //     }
    // }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 newVector = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
        Gizmos.DrawSphere(newVector, 0.05f);
    }
    
    // Заморозка позиции и поворота джампера
    private void FreezePositionAndRotationJumper(bool unfreezePosition = false)
    {
        if (unfreezePosition)
        {
            _rigidbodyJumper.constraints = RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            _rigidbodyJumper.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        }
        _rigidbodyJumper.freezeRotation = true;
    }
    
    // Получение скорости джампера
    private float GetSpeedJumper(float percent, float maximumSpeed=1f)
    {
        return maximumSpeed * percent / 100;
    }
}