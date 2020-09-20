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
    
    // Скрипт отвечает за поражение игрока
    private AnimationGameOverJumper _animationGameOverJumper;
    
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
        _animationGameOverJumper = GetComponent<AnimationGameOverJumper>();
        _simulationJumperPhysics = GetComponent<SimulationJumperPhysics>();
        _calculatingAngleHeightJumper = GetComponent<CalculatingAngleHeightJumper>();
        _radiusCollider = GetComponent<CapsuleCollider>().radius;
        _thisTransform = transform;
        FreezePositionAndRotation();
    }

    // private void Update()
    // {
    //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10, Color.yellow);
    // }
    
    // Скорость джампера
    //private Vector3 _vectorForceJumper = Vector3.zero;
    
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
            if (positionJumperEndAxesY != .0f)
                _cameraTracking.PositionY = positionJumperEndAxesY;
            //_rigidbodyJumper.isKinematic = false;
            FreezePositionAndRotation();
            _rigidbodyJumper.AddForce(vectorForce, ForceMode.Impulse);
        }    
    }

    
    
    // Анимация, которая выравнивает джампер перпендикулярно земли
    public IEnumerator AnimationStartJumper()
    {
        _rotationEnd = Quaternion.Euler(0, 0, 0);
        _animationStartJumperEnd = false;
        _landingCollider = false;
        // while (_rigidbodyJumper.velocity.y >= 0 && ClickTracking.JumpPlayer)
        // {
        //     _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, _rotationEnd, _speedFlightAnimationJumper * Time.deltaTime);
        //     yield return null;
        // }
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

    //private delegate bool _delegateCheckCollider(Collision collision);
    
    // Проверка угла джампера при приземление
    private IEnumerator CheckAngleJumper(Collision collision)
    {
        CheckCollider checkCollider = collision.gameObject.GetComponent<CheckCollider>();
        while (!checkCollider.CheckJumpPlayer(collision))
        {
            yield return null;
        }
        //ClickTracking.JumpPlayer = false;
        
    }
    
    // Проверка расстояния от джампера до ближайшего объекта
    //private IEnumerator CheckHeightFromJumperToObject(Collision collision)
    //{
        //
        // RaycastHit hit;
        // Vector3 startPosition = Vector3.zero;;
        // while (ClickTracking.JumpPlayer)
        // {
        //     print("Work");
        //     startPosition = new Vector3(_thisTransform.position.x, _thisTransform.position.y + 1, _thisTransform.position.z);
        //     if (Physics.SphereCast(startPosition, 0.07427804f, _thisTransform.TransformDirection(Vector3.down), out hit,
        //         10))
        //     {
        //         Debug.DrawRay(startPosition, _thisTransform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
        //         print($"Object - {hit.collider.name}; Distance - {hit.distance}");
        //         if (hit.collider.CompareTag("Object") && hit.distance <= 2f)
        //         {
        //             //ClickTracking.JumpPlayer = false;
        //             
        //         }
        //     }
        //     yield return null;
        // }
        //
    //}

    // Проверяем есть ли под джампером земля
    private bool CheckGroundDown()
    {
        RaycastHit hit;
        int layerMask = 1 << 9;
        Vector3 startPosition = new Vector3(_thisTransform.position.x, _thisTransform.position.y + 1, _thisTransform.position.z);
        // if (Physics.Raycast(startPosition, Vector3.down, out hit, Mathf.Infinity, layerMask))
        // {
        //     
        //     if (hit.collider.CompareTag("Ground"))
        //     {
        //         print($"Object Name - {hit.collider.name}");
        //         return true;
        //     }
        //     // if (hit.collider.CompareTag("Ground"))
        //     // {
        //     //     print("Ground");
        //     //     return true;
        //     // }
        // }
        
        if(Physics.SphereCast(startPosition, _radiusCollider, Vector3.down,  out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("Ground") || (hit.collider.CompareTag("Object") && hit.distance > 1))
            {
                print($"Object Name - {hit.collider.name}");
                print($"Distance to object - {hit.distance}");
                return true;
            }
        }
        return false;
    }

    // private void Update()
    // {
    //     Debug.DrawRay(new Vector3(_thisTransform.position.x, _thisTransform.position.y + 1, _thisTransform.position.z), Vector3.down * 100, Color.yellow);
    // }


    // Прикосновение к объекту
    private void OnCollisionEnter(Collision other){
        if (_animationStartJumperEnd)
            EndAnimationJumper(other);
    }
    
    // Прикосновение к объекту
    private void OnCollisionStay(Collision other)
    {
        if (_animationStartJumperEnd)
            EndAnimationJumper(other);
    }

    // Данная куротина проверяет нужно или нет продолжать игру
    private IEnumerator ContinuationGame()
    {
        while (CheckGroundDown())
        {
            yield return null;
        }
        
        if(!ClickTracking.GameOverPlayer)
            ClickTracking.JumpPlayer = false;
    }
    
    // Анимация заканчивается
    private void EndAnimationJumper(Collision other)
    {
        _landingCollider = true;
        _animationStartJumperEnd = false;
        
        FreezePositionAndRotation(true);
        StartCoroutine(_calculatingAngleHeightJumper.ReturnUpperPartJumper(true));
        StartCoroutine(ContinuationGame());
        
        if (OnJumperStop != null)
        {
            OnJumperStop(other);
        }
        
        //else
        //     _animationGameOverJumper.StartAnimationGameOver();
        // Нужно переделать
        // else if (other.collider.CompareTag("Object"))
        // {
        //     CheckCollider checkCollider = other.collider.GetComponent<CheckCollider>();
        //     checkCollider.CheckGameOver(other);
        //
        //     //StartCoroutine(CheckHeightFromJumperToObject(other));
        //     //StartCoroutine(CheckAngleJumper(other));
        //     
        //     //_delegateCheckCollider = checkCollider.CheckJumpPlayer(other);
        //     //print("Other - " + other.gameObject.name);
        //     
        //     // if (_checkCollider.CheckJumpPlayer(other))
        //     // {
        //     //     ClickTracking.JumpPlayer = false;
        //     //     //_rigidbodyJumper.isKinematic = true;
        //     //     
        //     // }
        //
        //     if (OnJumperStop != null)
        //     {
        //         OnJumperStop(other);
        //     }
        // }
        
        //print($"Animation Start Jumper End - {_animationStartJumperEnd};");
        
        // Проверка проиграл игрок или нет
        
        
        print("Animation End, start now");
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.blue;
    //     Vector3 newVector = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
    //     Gizmos.DrawSphere(newVector, 0.05f);
    // }

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
