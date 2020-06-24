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
        //_rigidbodyJumper.isKinematic = false; было изначально
        _rigidbodyJumper.isKinematic = false;
        _panelGameOver.SetActive(false);
        crutch_flag = true;
    }
    
    public void AddSpeedJumper()
    {
        _rigidbodyJumper.isKinematic = false;
        _rigidbodyJumper.AddForce((_vector3TransformHeightAngle.position - _thisTransform.position) * GetSpeedJumper(GetSpeedJumper(_calculatingAngleHeightJumper.GerPercentHeightJumper), GetSpeedJumper(_calculatingAngleHeightJumper.GetPercentAngleJumper), 10), ForceMode.Impulse);
    }

    // Меняет угол джампера, когда он начинает падать
    public IEnumerator AnimationJumperLanding()
    {
        Vector3 startRaycast = new Vector3(_thisTransform.position.x, _thisTransform.position.y + 0.2f, _thisTransform.position.z);
        Quaternion rotationEnd = Quaternion.Euler(0, 0, 0);
        bool selectSide = false;
        bool jumperStop = false;
        float speedAnimation = _speedFlightAnimationJumper;

        while (_thisTransform.rotation.eulerAngles != rotationEnd.eulerAngles || !_rigidbodyJumper.isKinematic || !jumperStop) //_thisTransform.rotation.eulerAngles != rotationEnd.eulerAngles
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
                    isFlyJumper = true;
                    selectSide = true;
                }
                startRaycast = new Vector3(_thisTransform.position.x, _thisTransform.position.y + 0.2f,
                    _thisTransform.position.z);
                RaycastHit hit;
                Debug.DrawRay(startRaycast, _thisTransform.TransformDirection(Vector3.down) * 100, Color.black);
                if (Physics.Raycast(startRaycast, _thisTransform.TransformDirection(Vector3.down), out hit, 1f) && !jumperStop)
                {
                    if (hit.distance < 0.3f && hit.collider.tag != "Player")
                    {
                        jumperStop = true;
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
    private bool isFlyJumper = false;

    ///костыль
    bool crutch_flag = false;
    void Crutch()
    {
        if (crutch_flag)
        {
            crutch_flag = false;
            _rigidbodyJumper.isKinematic = true;
        }
    }
    //костыль закончился
    private void OnCollisionEnter(Collision other)
    {
        Crutch();
        _normalVector = other.GetContact(0).normal.normalized;
        _pointVector = other.GetContact(0).point;

        if (isFlyJumper)
        {
            float angle = Vector3.Angle(_pointVector - (_pointVector + _normalVector * 5), Vector3.right);
            
            print($"Угол джампера {Vector3.Angle(_pointVector - (_pointVector + _normalVector * 5), Vector3.right)}");
            
            //if (angle <= 60)
            //{
            //    print("GameOver");
            //    ClickTracking.GameOverPlayer = true;
            //    _panelGameOver.SetActive(true);
            //}
            //else
            //{
                _rigidbodyJumper.isKinematic = true;
                isFlyJumper = false;
            //}
        }
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

    private void OnDrawGizmosSelected()
    {
        // Gizmos.color = Color.blue;
        // if(_normalVector == Vector3.zero)
        //     return;
        // Gizmos.DrawLine(_pointVector + _normalVector * 25, _pointVector);
        //
        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(transform.position, _vector3TransformHeightAngle.position);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right);
        
    }

    private float GetSpeedJumper(float percent, float maximumSpeed=1)
    {
        return maximumSpeed * percent / 100;
    }

    private float GetSpeedJumper(float percentHeight, float percentAngle, float maximumSpeed)
    {
        return maximumSpeed * ((percentHeight + percentAngle) * 2) / 100;
    }
    
}
