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
    
    [Header("Вектор для для нахождения вектора толчка")] [SerializeField]
    private Transform _vector3TransformHeightAngle = null;

    [Header("Панель, которая появляется во время проигрыша")] [SerializeField]
    private GameObject _panelGameOver = null;

    [Header("Скрипт, который отслеживает нажатие")] [SerializeField]
    private ClickTracking _clickTracking = null;

    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>

    // rigidbody джампера
    private Rigidbody _rigidbodyJumper = null;
    
    // очистка кэша
    private Transform _thisTransform = null;

    // Скрипт, который управляет углом и высотой джампера
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper = null;

    private void Start()
    {
        _rigidbodyJumper = GetComponent<Rigidbody>();
        _calculatingAngleHeightJumper = GetComponent<CalculatingAngleHeightJumper>();
        _thisTransform = transform;
        _rigidbodyJumper.isKinematic = true;
    }
    
    public void AddSpeedJumper()
    {
        _rigidbodyJumper.isKinematic = false;
        Vector3 vectorDifference = _vector3TransformHeightAngle.position - _thisTransform.position;
        Vector3 vectorForce = new Vector3(vectorDifference.x * GetSpeedJumper(_calculatingAngleHeightJumper.GetPercentAngleJumper,  _maximumSpeedFlightJumper), 
            vectorDifference.y * GetSpeedJumper(_calculatingAngleHeightJumper.GerPercentHeightJumper, _maximumSpeedFlightJumper), vectorDifference.z);
        _rigidbodyJumper.AddForce(vectorForce,ForceMode.Impulse);
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
        ClickTracking.JumpPlayer = false;
        _rotationEnd = Quaternion.Euler(0, 0, 0);
        while (_thisTransform.rotation != _rotationEnd && !_clickTracking.FingerInputScreen)
        {
            _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _rotationEnd, _speedAfterLandingAnimationJumper * Time.deltaTime);
            yield return null;
        }
        print("End ReturnRotationJumper");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_animationStartJumperEnd)
        {
            _landingCollider = true;
            _rigidbodyJumper.isKinematic = true;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (_animationStartJumperEnd)
        {
            _landingCollider = true;
            _rigidbodyJumper.isKinematic = true;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 newVector = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
        Gizmos.DrawSphere(newVector, 0.05f);
    }

    private float GetSpeedJumper(float percent, float maximumSpeed=1f)
    {
        return maximumSpeed * percent / 100;
    }

    // private float GetSpeedJumper(float percentHeight, float percentAngle, float maximumSpeed)
    // {
    //     return maximumSpeed * ((percentHeight + percentAngle) * 2) / 100;
    // }
    
}
