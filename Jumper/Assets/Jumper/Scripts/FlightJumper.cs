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
    private float _maximumSpeedFlightJumper;
    
    [Header("Скорость анимации полета джампера (После прыжка)")] [SerializeField] [Range(0, 100)]
    private float _speedFlightAnimationJumper;

    [Header("Скорость анимации после приземления и при возвращение джампера перпендикулярно земли")]
    [SerializeField]
    [Range(0, 100)]
    private float _speedAfterLandingAnimationJumper;

    [Header("Угол наклона джампера при падении")] [SerializeField] [Range(0, 45)]
    private float _angleIncidence;

    [Header("Скрипт, который настраивает отвечает за движение камеры")] [SerializeField]
    private CameraTracking _cameraTracking;

    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>

    // rigidbody джампера
    private Rigidbody _rigidbodyJumper;
    
    // очистка кэша
    private Transform _thisTransform;

    // Скрипт, который управляет углом и высотой джампера
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper;
    
    // Скрипт, который управляет симуляцией джампера
    private SimulationJumperPhysics _simulationJumperPhysics;

    // Переменная, которая хранит скорость поворота во время полета по оси Z
    private float _speedRotationJumperFlight;
    
    // Вектор для нахождения толчка
    private Transform _vector3TransformHeightAngle;
    
    // Радиус Capsule Collider
    private float _radiusCollider = .0f;
    
    // Переменная хранит конечный поворот
    private Quaternion _rotationEnd = Quaternion.identity;
    private bool _animationStartJumperEnd;
    private bool _landingCollider;
    
    public event Action<Collision> OnJumperStop;
    
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

    // Возвращает максимальную скорость джампера
    public float GetMaximumSpeedJumper
    {
        get
        {
            return _maximumSpeedFlightJumper;
        }
    }

    private void Start()
    {
        _rigidbodyJumper = GetComponent<Rigidbody>();
        _simulationJumperPhysics = GetComponent<SimulationJumperPhysics>();
        _calculatingAngleHeightJumper = GetComponent<CalculatingAngleHeightJumper>();
        _radiusCollider = GetComponent<CapsuleCollider>().radius;
        _thisTransform = transform;
        // FreezePositionAndRotation();
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
            Vector3 vectorForce =
                new Vector3(-vectorDifference.x * speedX, Mathf.Abs(vectorDifference.y * speedY), vectorDifference.z);
             Dictionary<string, float> dictionaryDistance =
                 _simulationJumperPhysics.SimulationJumper(transform.position, vectorForce);
            float positionJumperEndAxesY = dictionaryDistance["finite_distance_axes_y"];
            _speedRotationJumperFlight = dictionaryDistance["speed_rotation_jumper_flight"];
            _rigidbodyJumper.isKinematic = false;
            if (positionJumperEndAxesY != .0f)
                _cameraTracking.PositionY = positionJumperEndAxesY;
            //FreezePositionAndRotation();
            _rigidbodyJumper.AddForce(vectorForce, ForceMode.Impulse);
        }    
    }

    
    
    // Анимация, которая выравнивает джампер перпендикулярно земли
    public IEnumerator AnimationStartJumper()
    {
        _rotationEnd = Quaternion.Euler(0, 0, 0);
        _animationStartJumperEnd = false;
        _landingCollider = false;
        
        float positionJumperNowY = .0f;
        bool maxPointYFound = false;
        while (!maxPointYFound)
        {    
            _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _rotationEnd, _speedFlightAnimationJumper * Time.deltaTime);
            if (positionJumperNowY > _thisTransform.transform.position.y && ClickTracking.JumpPlayer)
            {
                _animationStartJumperEnd = true;
                maxPointYFound = true;
                print("End AnimationJump");
                yield return AnimationSeveralDegreesRelationGround();
            }
            else
            {
                positionJumperNowY = _thisTransform.transform.position.y;
            }

            yield return null;
        }
    }
    
    // Анимация во время полета. Начинает выстраивать джампер на несколько градусов относительно земли.
    private IEnumerator AnimationSeveralDegreesRelationGround()
    {
        if (_thisTransform.rotation.z < 0)
            _rotationEnd = Quaternion.Euler(0, 0, _angleIncidence);
        else
            _rotationEnd = Quaternion.Euler(0, 0, -_angleIncidence);
        while (!_landingCollider && ClickTracking.JumpPlayer)
        {
            print("AnimationSeveralDegreesRelationGround");
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
        while (_thisTransform.rotation != _rotationEnd && !ClickTracking.FingerInputScreen)
        {
            _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _rotationEnd, _speedAfterLandingAnimationJumper * Time.deltaTime);
            yield return null;
        }
        print("End ReturnRotationJumper");
    }
    
    private float _savePositionY;
    
    // Проверяем есть ли под джампером земля (Доделать)
    private bool CheckGroundDown()
    {
        bool checkRaycast = false;
        bool checkVelocity = false;
        
        RaycastHit hit;
        int layerMask = 1 << 9;
        
        Vector3 startPosition = new Vector3(_thisTransform.position.x, _thisTransform.position.y + 1, _thisTransform.position.z);
        
        if(Physics.SphereCast(startPosition, _radiusCollider, Vector3.down,  out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("Ground") || (hit.collider.CompareTag("Object") && hit.distance > 1)) // || (hit.collider.CompareTag("Object") && hit.distance > 1)
            {
                checkRaycast = true;
            }
        }
        if (_rigidbodyJumper.velocity.y < -0.1f || _rigidbodyJumper.velocity.y > 0.1f)
            checkVelocity = true;
    
        
        //print($"Check raycast - {checkRaycast}");
        //print($"Check velocity - {checkVelocity}");
        if (checkRaycast || checkVelocity)
        {
            //print("While");
            return true;
        }

        return false;
    }


    // Прикосновение к объекту
    private void OnCollisionEnter(Collision other){
        if (_animationStartJumperEnd)
            EndAnimationJumper(other);
    }
    
    // Нахождение на объекте
    private void OnCollisionStay(Collision other)
    {
        if (_animationStartJumperEnd)
            EndAnimationJumper(other);
    }

    // Данная куротина проверяет нужно или нет продолжать игру 
    private IEnumerator ContinuationGame()
    {
        _rigidbodyJumper.isKinematic = false;
        while (CheckGroundDown())
        {
            yield return null;
        }

        if (!ClickTracking.GameOverPlayer)
        {
            ClickTracking.JumpPlayer = false;
            _rigidbodyJumper.isKinematic = true;
            //FreezePositionAndRotation(true);
        }
    }
    


    // Анимация заканчивается
    private void EndAnimationJumper(Collision collision)
    {
        _landingCollider = true;
        _animationStartJumperEnd = false;
        _rigidbodyJumper.isKinematic = true;
        StartCoroutine(_calculatingAngleHeightJumper.ReturnUpperPartJumper(true));
        StartCoroutine(ContinuationGame());

        // if (collision.collider.CompareTag("Object"))
        // {
        //     CheckCollider colliderObject =  collision.collider.GetComponent<CheckCollider>();
        //     if (colliderObject.OnOffClickTrackingJumpPlayer(collision))
        //     {
        //         StartCoroutine(ContinuationGame());
        //     }
        // }
        
        //ClickTracking.JumpPlayer = false;
        //_rigidbodyJumper.isKinematic = true;
        // if (CheckGroundDown())
        //     _rigidbodyJumper.isKinematic = false;
        // else 
        //     _rigidbodyJumper.isKinematic = true;
  
            
        print("Animation End, start now");
    }

    // Заморозка позиции и поворота
    private void FreezePositionAndRotation(bool freeze=false)
    {
        if (freeze)
        {
            //print("Frezee true");
            _rigidbodyJumper.constraints |= RigidbodyConstraints.FreezePositionX;
            //_rigidbodyJumper.constraints |= RigidbodyConstraints.FreezePositionY;
            //_rigidbodyJumper.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            _rigidbodyJumper.constraints &= ~RigidbodyConstraints.FreezePositionX;
            //_rigidbodyJumper.constraints |= RigidbodyConstraints.FreezePositionX;
            //_rigidbodyJumper.constraints |= RigidbodyConstraints.FreezePositionY;
            //print("Frezee false");
            //_rigidbodyJumper.constraints &= ~RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        }

        _rigidbodyJumper.freezeRotation = true;
    }
    
    
    // Получение скорости джампера
    private float GetSpeedJumper(float percent, float maximumSpeed=1f)
    {
        return maximumSpeed * percent / 100;
    }
}
