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
    [Header("Максимальная скорость полета джампера")] [SerializeField] [Range(0, 10)]
    private float _maximumSpeedJump = .0f;

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

    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>
    
    // Вектор в которую полетит джампер
    private Vector3 _vectorSpeedJumper = Vector3.zero;
    
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
        if(_thisTransform.rotation.z < 0)
            _vectorSpeedJumper = new Vector3(0.3f, 1, 0);
        else 
            _vectorSpeedJumper = new Vector3(-0.3f, 1, 0);
        _rigidbodyJumper.isKinematic = false;
        print(GetSpeedJumper(GetSpeedJumper(_calculatingAngleHeightJumper.GerPercentHeightJumper), GetSpeedJumper(_calculatingAngleHeightJumper.GetPercentAngleJumper), _maximumSpeedJump));
        _rigidbodyJumper.AddForce(_vectorSpeedJumper * GetSpeedJumper(GetSpeedJumper(_calculatingAngleHeightJumper.GerPercentHeightJumper), GetSpeedJumper(_calculatingAngleHeightJumper.GetPercentAngleJumper), 10), ForceMode.Impulse);
    }


    // Меняет угол джампера, когда он начинает падать
    public IEnumerator AnimationJumperLanding()
    {
        Vector3 startRaycast = new Vector3(_thisTransform.position.x, _thisTransform.position.y + 0.2f, _thisTransform.position.z);
        Quaternion rotationEnd = Quaternion.Euler(0, 0, 0);
        bool selectSide = false;
        bool jumperStop = false;
        float speedAnimation = _speedFlightAnimationJumper;

        while (_thisTransform.rotation.eulerAngles != rotationEnd.eulerAngles || !_rigidbodyJumper.isKinematic) //_thisTransform.rotation.eulerAngles != rotationEnd.eulerAngles
        {
            _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, rotationEnd, speedAnimation * Time.deltaTime);
            
            if (_rigidbodyJumper.velocity.y < 0)
            {
                if (!selectSide)
                {
                    if (_thisTransform.rotation.z < 0)
                        rotationEnd = Quaternion.Euler(0, 0, _angleIncidence);
                    else
                        rotationEnd = Quaternion.Euler(0, 0, -_angleIncidence);
                    speedAnimation = _speedLandingAnimationJumper;
                    //isFlyJumper = true;
                    selectSide = true;
                }
                startRaycast = new Vector3(_thisTransform.position.x, _thisTransform.position.y + 0.2f,
                    _thisTransform.position.z);
                RaycastHit hit;
                Debug.DrawRay(startRaycast, _thisTransform.TransformDirection(Vector3.down) * 100, Color.black);
                if (Physics.Raycast(startRaycast, _thisTransform.TransformDirection(Vector3.down), out hit, 1f) && !jumperStop)
                {
                    if (hit.distance < 0.2f && hit.collider.tag != "Player")
                    {
                        jumperStop = true;
                        _rigidbodyJumper.isKinematic = true;
                        rotationEnd = Quaternion.Euler(0, 0, 0);
                        speedAnimation = _speedAfterLandingAnimationJumper;
                        yield return _calculatingAngleHeightJumper.ReturnUpperPartJumper();
                        print("Stop Animation");
                        ClickTracking.JumpPlayer = false;
                    }
                }
            }

            yield return null;
        }
    }

    private Vector3 _normalVector = Vector3.zero;
    private Vector3 _pointVector = Vector3.zero;
    //private bool isFlyJumper = false;
    
    private void OnCollisionEnter(Collision other)
    {
        print("Enter");
        _normalVector = other.GetContact(0).normal.normalized;
        _pointVector = other.GetContact(0).point;

        //_rigidbodyJumper.isKinematic = isFlyJumper ? true : false;
        
        // if (isFlyJumper)
        // {
        //     _rigidbodyJumper.isKinematic = true;
        //     isFlyJumper = false;
        // }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if(_normalVector == Vector3.zero)
            return;
        Gizmos.DrawLine(_pointVector + _normalVector * 25, _pointVector);
    }

    private float GetSpeedJumper(float percent, float maximumSpeed=1)
    {
        return maximumSpeed * percent / 100;
    }

    private float GetSpeedJumper(float percentHeight, float percentAngle, float maximumSpeed)
    {
        print(Mathf.Abs(maximumSpeed * ((percentHeight + percentAngle) / 2) / 10));
        return maximumSpeed * ((percentHeight + percentAngle) * 6f) / 100;
    }
    
}
