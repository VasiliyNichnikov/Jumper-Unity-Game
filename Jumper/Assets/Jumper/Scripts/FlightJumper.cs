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
    [Header("Максимальная скорость полета джампера")] [SerializeField] [Range(0, 50)]
    private float _maximumSpeedFlightJumper = .0f;
    
    [Header("Скорость анимации полета джампера (После прыжка)")] [SerializeField] [Range(0, 100)]
    private float _speedFlightAnimationJumper = .0f;
    
    [Header("Скорость анимации приземления джампера")] [SerializeField] [Range(0, 100)]
    private float _speedLandingAnimationJumper = .0f;

    [Header("Скорость анимации после приземления и при возвращение джампера перпендикулярно земли")]
    [SerializeField]
    [Range(0, 100)]
    private float _speedAfterLandingAnimationJumper = .0f;

    [Header("Угол наклона джампера при падении")] [SerializeField] [Range(0, 45)]
    private float _angleIncidence = .0f;
    
    [Header("Вес джампера при старте")] [SerializeField] [Range(0, 5)]
    private float _massJumper = .0f;
    
    [Header("Вектор для для нахождения вектора толчка")] [SerializeField]
    private Transform _vector3TransformHeightAngle = null;

    [Header("Скрипт, который отображает траекторию джамперв")] [SerializeField]
    private Trajectory _trajectory = null;
    
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
    
    // Для настроек джампера(Конец)
    
    // Возвращает среднюю скорость джампера
    public float GetAverageSpeedJumper
    {
        get
        {
            return GetSpeedJumper(_calculatingAngleHeightJumper.GerPercentHeightJumper, _maximumSpeedFlightJumper);
            //(GetSpeedJumper(_calculatingAngleHeightJumper.GetPercentAngleJumper, _maximumSpeedFlightJumper) +
            //GetSpeedJumper(_calculatingAngleHeightJumper.GerPercentHeightJumper, _maximumSpeedFlightJumper)) / 2;
        }
    }

    private void Awake()
    {
        _rigidbodyJumper = GetComponent<Rigidbody>();
        _calculatingAngleHeightJumper = GetComponent<CalculatingAngleHeightJumper>();
        _thisTransform = transform;
        _rigidbodyJumper.isKinematic = true;
    }

    // Добавление скорости джамперу
    public void AddSpeedJumper()
    {
        var speedX = GetSpeedJumper(_calculatingAngleHeightJumper.GetPercentAngleJumper, _maximumSpeedFlightJumper);
        var speedY = GetSpeedJumper(_calculatingAngleHeightJumper.GerPercentHeightJumper, _maximumSpeedFlightJumper);
        _rigidbodyJumper.isKinematic = false;
        if (speedY > 0)
        {
            Vector3 vectorDifference = _vector3TransformHeightAngle.position - _thisTransform.position;
            Vector3 vectorForce = new Vector3(vectorDifference.x * speedX, vectorDifference.y * speedY, vectorDifference.z);
            //_addMassJumper = Vector3.Project(vectorForce, Vector3.up).y / 2; // Добавляем массу джамперу при падениии
            Vector3 positionJumperEnd = _trajectory.ShowTrajectory(transform.position, vectorForce);
            if(positionJumperEnd != Vector3.zero)
                _cameraTracking.PositionY = positionJumperEnd.y;
            _rigidbodyJumper.AddForce(vectorForce, ForceMode.Impulse);
        }
    }

    private Quaternion _rotationEnd = Quaternion.identity;
    private bool _animationStartJumperEnd = false;
    private bool _landingCollider = false;
    
    // Хранит начальную позиция джампера при прыжке по оси X
    private float _startPositionJump = .0f;
    
    // Анимация начала анимации (Во время полета становится перпендикулярно земле)
    public IEnumerator AnimationStartJumper()
    {
        _rotationEnd = Quaternion.Euler(0, 0, 0);
        _animationStartJumperEnd = false;
        _landingCollider = false;
        _startPositionJump = Mathf.Abs(_thisTransform.position.x);
        while (_rigidbodyJumper.velocity.y >= 0)
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
        var endPositionJump = Math.Abs(_thisTransform.position.x);
        var differenceDistance = Mathf.Abs(endPositionJump - _startPositionJump);
        //_rigidbodyJumper.mass += _addMassJumper;
        if (differenceDistance > .5f)
        {
            if (_thisTransform.rotation.z < 0)
                _rotationEnd = Quaternion.Euler(0, 0, _angleIncidence);
            else
                _rotationEnd = Quaternion.Euler(0, 0, -_angleIncidence);

            while (!_landingCollider)
            {
                _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _rotationEnd,
                    _speedLandingAnimationJumper * Time.deltaTime);
                yield return null;
            }
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

    private void OnCollisionStay(Collision other)
    {
        if (_animationStartJumperEnd)
            EndAnimationJumper();
    }

    
    // Анимация заканчивается
    private void EndAnimationJumper()
    {
        _landingCollider = true;
        ClickTracking.JumpPlayer = false; 
        _rigidbodyJumper.isKinematic = true;
        _animationStartJumperEnd = false; // Test
        print("Animation End, start now");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 newVector = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
        Gizmos.DrawSphere(newVector, 0.05f);
    }

    private float GetSpeedJumper(float percent, float maximumSpeed=1f)
    {
        return maximumSpeed * percent / _calculatingAngleHeightJumper.MaximumPercentScreenForMaximumSpeedJumper;
    }
}
