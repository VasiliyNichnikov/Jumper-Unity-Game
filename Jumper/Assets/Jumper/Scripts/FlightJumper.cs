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
    
    // Маска земли на которую можно приземляться джамперу
    // private int _layerMaskJumper = 8;

    private void Start()
    {
        _rigidbodyJumper = GetComponent<Rigidbody>();
        _calculatingAngleHeightJumper = GetComponent<CalculatingAngleHeightJumper>();
        //_radiusCollider = GetComponent<CapsuleCollider>().radius;
        //_layerMaskJumper = 1 << 8;
        _thisTransform = transform;
        _rigidbodyJumper.isKinematic = true;
        _panelGameOver.SetActive(false);
    }
    
    public void AddSpeedJumper()
    {
        _rigidbodyJumper.isKinematic = false;
        float speed = GetSpeedJumper(GetSpeedJumper(_calculatingAngleHeightJumper.GerPercentHeightJumper),
            GetSpeedJumper(_calculatingAngleHeightJumper.GetPercentAngleJumper), _maximumSpeedFlightJumper);
        Vector3 vectorDifference = _vector3TransformHeightAngle.position - _thisTransform.position;
        Vector3 vectorForce = new Vector3(vectorDifference.x * GetSpeedJumper(_calculatingAngleHeightJumper.GetPercentAngleJumper,  _maximumSpeedFlightJumper), 
            vectorDifference.y * GetSpeedJumper(_calculatingAngleHeightJumper.GerPercentHeightJumper, _maximumSpeedFlightJumper), vectorDifference.z);
        //print($"Speed: {speed}");
        //print(speed);
        _rigidbodyJumper.AddForce(vectorForce,ForceMode.Impulse);
        //_rigidbodyJumper.AddForce((_vector3TransformHeightAngle.position - _thisTransform.position) * speed, ForceMode.Impulse);
    }

    //private bool _endAnimation = false;
    
    // Меняет угол джампера, когда он начинает падать
    // public IEnumerator AnimationJumperLanding()
    // {
        //Vector3 startRaycast = new Vector3(_thisTransform.position.x, _thisTransform.position.y + 0.2f, _thisTransform.position.z);
        // Quaternion rotationEnd = Quaternion.Euler(0, 0, 0);
        // bool selectSide = false;
        // bool jumperStop = false;
        // _endAnimation = false;
        // bool velocityLowerZero = false;
        // float speedAnimation = _speedFlightAnimationJumper;

        // while ((_thisTransform.rotation.eulerAngles != rotationEnd.eulerAngles || !jumperStop) && !_clickTracking.FingerInputScreen) //_thisTransform.rotation.eulerAngles != rotationEnd.eulerAngles
        // {
        //     print("Work");
        //     // print(_clickTracking.FingerInputScreen);
        //     // if (_clickTracking.FingerInputScreen)
        //     // {
        //     //     print("Break Animation");
        //     //     yield break;
        //     // }
        //
        //     //print("Work!");
        //     _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, rotationEnd, speedAnimation * Time.deltaTime);
        //     
        //     //print($"Rigidbody velocity - {_rigidbodyJumper.velocity.y}");
        //     if (_rigidbodyJumper.velocity.y < 0 || velocityLowerZero) // || _endAnimation
        //     {
        //         velocityLowerZero = true;
        //         print("Velocity");
        //         if (!selectSide)
        //         {
        //             if (_thisTransform.rotation.z < 0)
        //                 rotationEnd = Quaternion.Euler(0, 0, _angleIncidence);
        //             else
        //                 rotationEnd = Quaternion.Euler(0, 0, -_angleIncidence);
        //             
        //             speedAnimation = _speedLandingAnimationJumper;
        //             isFlyJumper = true;
        //             selectSide = true;
        //             //velocityLowerZero = true;
        //             print(rotationEnd);    
        //             print("Jumper down");
        //         }
        //         // startRaycast = new Vector3(_thisTransform.position.x, _thisTransform.position.y + 0.05f,
        //         //     _thisTransform.position.z);
        //         RaycastHit hit;
        //         Debug.DrawRay(_thisTransform.position, _thisTransform.TransformDirection(Vector3.down) * 100, Color.black);
        //
        //         if (_endAnimation)
        //         {
        //             jumperStop = true;
        //             rotationEnd = Quaternion.Euler(0, 0, 0);
        //             speedAnimation = _speedAfterLandingAnimationJumper;
        //             ClickTracking.JumpPlayer = false;
        //         }
                
                // if (Physics.SphereCast(_thisTransform.position, _radiusCollider,
                //     _thisTransform.TransformDirection(Vector3.down), out hit, 1f, _layerMaskJumper))
                // {
                //     //print("Raycast");
                //     print($"Distance - {hit.distance}; Object - {hit.collider.name}");
                //     if (hit.distance < 0.3f)//&& hit.collider.tag != "Player" && !jumperStop)
                //     {
                //         print("Raycast dis < 0.3f");
                //         jumperStop = true;
                //         rotationEnd = Quaternion.Euler(0, 0, 0);
                //         speedAnimation = _speedAfterLandingAnimationJumper;
                //         //yield return _calculatingAngleHeightJumper.ReturnUpperPartJumper();
                //         print("Stop Animation");
                //         ClickTracking.JumpPlayer = false;
                //     }
                // }
                
                // if (Physics.Raycast(startRaycast, _thisTransform.TransformDirection(Vector3.down), out hit, 1f) && !jumperStop)
                // {
                //     
                // }
    //         }
    //
    //         yield return null;
    //     }
    //     print("End Jumper");
    // }
    
    private Quaternion _rotationEnd = Quaternion.identity;
    private bool _animationStartJumperEnd = false;
    private bool _landingCollider = false;
    
    // Анимация начала анимации (Во время полета становится перпендикулярно земле)
    public IEnumerator AnimationStartJumper()
    {
        _rotationEnd = Quaternion.Euler(0, 0, 0);
        _animationStartJumperEnd = false;
        _landingCollider = false;
        
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
        if (_thisTransform.rotation.z < 0)
            _rotationEnd = Quaternion.Euler(0, 0, _angleIncidence);
        else
            _rotationEnd = Quaternion.Euler(0, 0, -_angleIncidence);

        while (!_landingCollider)
        {
            _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _rotationEnd, _speedLandingAnimationJumper * Time.deltaTime);
            yield return null;
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
    // private Vector3 _normalVector = Vector3.zero;
    // private Vector3 _pointVector = Vector3.zero;
    // private bool isFlyJumper = false;
    
    private void OnCollisionEnter(Collision other)
    {
        // _normalVector = other.GetContact(0).normal.normalized;
        // _pointVector = other.GetContact(0).point;
        if (_animationStartJumperEnd)
        {
            _landingCollider = true;
            _rigidbodyJumper.isKinematic = true;
        }
            
        // if (isFlyJumper)
        // {
        //     _endAnimation = true;
        //     _rigidbodyJumper.isKinematic = true;
        //     isFlyJumper = false;
            //float angle = Vector3.Angle(_pointVector - (_pointVector + _normalVector * 5), Vector3.right);
            //float angle = 90;
            //print($"Угол джампера {Vector3.Angle(_pointVector - (_pointVector + _normalVector * 25), Vector3.right)}");
            
            // if (angle <= 60 || angle >= 150)
            // {
            //     print("GameOver");
            //     ClickTracking.GameOverPlayer = true;
            //     _panelGameOver.SetActive(true);
            // }
            // else
            // {
            //     _rigidbodyJumper.isKinematic = true;
            //     isFlyJumper = false;
            // }
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Object")
        {
            print("GameOver");
            ClickTracking.GameOverPlayer = true;
            _panelGameOver.SetActive(true);
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
        // if(_normalVector == Vector3.zero)
        //     return;
        // Gizmos.DrawLine(_pointVector + _normalVector * 25, _pointVector);
        //
        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(transform.position, _vector3TransformHeightAngle.position);
        // Angle
        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(transform.position, transform.position + Vector3.right);
        // Gizmos.color = Color.blue;
        // Gizmos.DrawLine(transform.position, _pointVector - (_pointVector + _normalVector * 25));
    }

    private float GetSpeedJumper(float percent, float maximumSpeed=1f)
    {
        //print($"Speed One - {maximumSpeed * percent / 100}");
        return maximumSpeed * percent / 100;
    }

    private float GetSpeedJumper(float percentHeight, float percentAngle, float maximumSpeed)
    {
        //print(percentHeight);
        //print(percentAngle);
        //print(percentHeight + percentAngle);
        return maximumSpeed * ((percentHeight + percentAngle) * 2) / 100;
    }
    
}
