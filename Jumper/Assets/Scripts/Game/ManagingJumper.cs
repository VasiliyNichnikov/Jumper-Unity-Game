using System;
using System.Collections;
using UnityEngine;

// данный класс управляет джампером 
public class ManagingJumper : MonoBehaviour
{
    [Header("Главная камера")] [SerializeField]
    private Camera _mainCamera = null;

    [Header("Джампер (Игрок)")] [SerializeField]
    private GameObject _jumper = null;

    [Header("Скрипт, который проверяет, находится ли игрок на земле или нет")] [SerializeField]
    private InspectionGround _inspectionGround = null;
    
    
    //
    // [Header("Основная часть джампера")] [SerializeField]
    // private GameObject _mainJumper = null;
    //
    // [Header("Верхняя часть джемпера")] [SerializeField]
    // private Transform _upperPartJumper = null;
    //
    // [Header("RigidBody джампера")] [SerializeField]
    // private Rigidbody _rigidbodyJumper = null;
    //
    // [Header("Максимальный угол наклона по модулю")] [SerializeField]
    // private float _maximumAngleInclination = .0f;
    //
    // [Header("Максимальная скорость прыжка")] [SerializeField] [Range(0, 100)]
    // private float _maximumJumpSpeed = .0f;
    //
    // [Header("Минимальное сжатие джампа")] [SerializeField] [Range(0f, 1f)]
    // private float _minimumJumpCompression = .0f;
    //     
    // [Header("Максимальное сжатие джампа")] [SerializeField] [Range(0f, 1f)]
    // private float _maximumJumpCompression = .0f;
    //
    // // Разница координат по X (Нужно для понимания, куда идет палец (Право/Влево))
    // private float _differenceX = .0f;
    //
    // //"Скорость поднятия на высоту джампера"
    // private float _speedUpJumper = .0f;
    //
    //
    // // Данная переменная, проверяет анимируется ли верхняя часть джампера или нет
    // private bool _animationTopPartJumper = false;
    
    // Переменные, которые отвечают за угол игрока
    [Header("Максимальный угол, на который может повернуться джампер по оси Z")] [SerializeField] [Range(0, 90)]
    private float _maximumJumperAngleZ = 0.0f;

    // Переменный, которые отвечают за высоту второй части джампера (Верхней части)
    [Header("Верхняя часть джампера")] [SerializeField]
    private Transform _jumperPartUpper = null;
    
    [Header("Минимальное сжатие джампера")] [SerializeField] [Range(0f, 1f)]
    private float _minimumJumpCompression = .0f;
    
    [Header("Максимальное сжатие джампера")] [SerializeField] [Range(0f, 1f)] 
    private float _maximumJumpCompression = .0f;
    
    [Header("Максимальная скорость прыжка")] [SerializeField] [Range(0, 100)]
    private float _maximumJumpSpeed = .0f;
    
    //"Скорость поднятия на высоту джампера"
    private float _speedUpJumper = .0f;
    
    //[Header("Высота, которая должна быть в данный момент у верхней части")] [SerializeField] [Range(0, 10)] // Test
    //private float _jumperHeightY = 0.0f;

    [Header("Вектор начала")] [SerializeField]
    private Transform _vectorStart = null;
    
    [Header("Вектор кинца")] [SerializeField]
    private Transform _vectorEnd = null;
    
    // Дополнительные переменные
    // Стартовая позиция мыши (пальца)
     private Vector2 _startFingerPosition = Vector2.zero;

     private Rigidbody _rigidbodyJumper = null;
     
    private void Start()
    {
        //_jumperHeightY = _jumperPartUpper.localPosition.y;
        _rigidbodyJumper = _jumper.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //_jumperPartUpper.localPosition = new Vector3(_jumperPartUpper.localPosition.x, _jumperHeightY, _jumperPartUpper.localPosition.z);
        //_jumper.transform.localRotation = Quaternion.Euler(_jumper.transform.localRotation.x, _jumper.transform.localRotation.y, -_jumperAngleZ);
    }
    
    // Рисование кнопки для запуска джампера (Создан для тестирования)
    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 150, 100), "Start"))
        {
            Rigidbody _rigidbodyJumper = _jumper.GetComponent<Rigidbody>();
            _rigidbodyJumper.isKinematic = false;
            _rigidbodyJumper.AddForce((_vectorEnd.position - _vectorStart.position) * 12);
            StartCoroutine(ReturnStartingTopJumperPosition(2));
            StartCoroutine(ReturnStartingTopJumperRotation());
            StartCoroutine(StartCheckGround());
        }
        
    }

    private IEnumerator StartCheckGround()
    {
        Vector3 startPositionJumper = _jumper.transform.position;
        while (true)
        {
            if (_jumper.transform.position.y > startPositionJumper.y + 0.1f)
            {
                yield return _inspectionGround.CheckGround();
                yield break;
            }
            yield return null;
        }
    }
    
    
    // public void ChangeRigidbodyKinematic(bool condition)
    // { 
    //     _jumper.GetComponent<Rigidbody>().isKinematic = condition;
    // }

    public void StartAnimationTopJumper()
    {
        //if(_upperPartJumper.localPosition.y != 1 && !_animationTopPartJumper)
        //    StartCoroutine(ReturnStartingTopJumperPosition(1, 1));
    }
    

    private void OnMouseDown()
    {
        //if (InspectionGround.IsGround)
        //{
            _startFingerPosition = new Vector2(_mainCamera.ScreenToViewportPoint(Input.mousePosition).x, 
            _mainCamera.ScreenToViewportPoint(Input.mousePosition).y);
        //}
    }

    private void OnMouseDrag()
    {
        // if (InspectionGround.IsGround)
        // {
        _jumperPartUpper.localPosition = new Vector3(_jumperPartUpper.localPosition.x, 
            GetPositionJumper(), _jumperPartUpper.localPosition.z);
        
        _jumper.transform.localRotation = Quaternion.Euler(_jumper.transform.localRotation.x, _jumper.transform.localRotation.y, 
            GetAngleJumper());
        // }
    }
    
    private void OnMouseUp()
    {
        Rigidbody _rigidbodyJumper = _jumper.GetComponent<Rigidbody>();
        // Выключаем кинематику
        _rigidbodyJumper.isKinematic = false;
        // Вычисляем скорость
        var percentagePositionY = 100 - GetPositionJumper() * 100;
        //print(percentagePositionY);
        _speedUpJumper = percentagePositionY * _maximumJumpSpeed / 100;
        _speedUpJumper = Mathf.Clamp(_speedUpJumper, 0, _maximumJumpSpeed);
        // Добавляем скорость
        //print(_speedUpJumper);
        _rigidbodyJumper.AddForce((_vectorEnd.position - _vectorStart.position) * _speedUpJumper);
        StartCoroutine(ReturnStartingTopJumperPosition(2));
       // StartCoroutine(ReturnStartingTopJumperRotation());
        StartCoroutine(StartCheckGround());
        
        // _rigidbodyJumper.AddForce(new Vector3(positionX, 1, 0) * _speedUpJumper);
        
        // if (InspectionGround.IsGround)
        // {
        //     AddSpeedJumperY(GetPositionX(), GetPositionJumper());
        //     if(!_animationTopPartJumper)
        //         StartCoroutine(ReturnStartingTopJumperPosition(GetPositionTopPartJumper(), 1.5f));
        //     //StartCoroutine(ReturnStartingTopJumperRotation());
        //     //StartCoroutine(_followingCamera.AnimationCamera(_differenceX));
        //     InspectionGround.IsGround = false;
        // }
    }
    
    // Возвращает до какой позиции надо сдвигать высшую ножку джампера
    //private float GetPositionTopPartJumper()
    //{
        // var percent = _speedUpJumper * 100 / _maximumJumpSpeed;
        // //print(percent);
        // if (percent > 30)
        //     return 0.8f;
        // return 1f;
        //return 1f;// test
    //}
    
    // Сделать изменение в зависимости от скорости
    //private float GetPositionX()
    //{
    //    // float posX = 1f;
        // if (GetAngleRotationJumper() > 3f)
        //     posX = -1f;
        // else if (GetAngleRotationJumper() <= 3f && GetAngleRotationJumper() >= 0)
        //     posX = 0.0f;
        // return posX;
    //    return 1f;// test
    //}
    
    // Возвращает сжатие прыжка
    private float GetPositionJumper()
    {
        float positionNowY = _mainCamera.ScreenToViewportPoint(Input.mousePosition).y;
        float differenceY = _startFingerPosition.y - positionNowY;
        float percentageScreenY = 100 - differenceY * 100;
        float jumpCompressionY = percentageScreenY * _maximumJumpCompression / 100;
        jumpCompressionY = Mathf.Clamp(jumpCompressionY, _minimumJumpCompression, _maximumJumpCompression);
        return jumpCompressionY;
    }

    // Возвращает нужный поворот
    private float GetAngleJumper()
    {
        float positionNowX = _mainCamera.ScreenToViewportPoint(Input.mousePosition).x;
        float differenceX = _startFingerPosition.x - positionNowX;

        float percentageScreenX = differenceX * 100;
        
        var angleInclination = percentageScreenX * _maximumJumperAngleZ / 100;
        angleInclination = Mathf.Clamp(angleInclination, -_maximumJumperAngleZ, _maximumJumperAngleZ);
        return angleInclination;
    }
    
    // Возвращает верхнюю часть джампера в первоначальное положение
    private IEnumerator ReturnStartingTopJumperPosition(float speed, float posY = 1)
    {
        while (_jumperPartUpper.localPosition.y != posY)
        {
            _jumperPartUpper.localPosition = Vector3.MoveTowards(_jumperPartUpper.localPosition,
                new Vector3(_jumperPartUpper.localPosition.x, posY, _jumperPartUpper.localPosition.z),
                speed * Time.deltaTime);
            yield return null;
        }
    }
    
    // Возвращает угол джампера в первоначальное положение
    private IEnumerator ReturnStartingTopJumperRotation()
    {
        var timeCount = .0f;
        while (_jumper.transform.rotation != Quaternion.Euler(0, 0, 0))
        {
            //print(_jumper.transform.rotation);
            _jumper.transform.rotation = Quaternion.Slerp(_jumper.transform.rotation, 
                 Quaternion.Euler(0, 0, 0), timeCount);
            // Должно умножаться в зависимости от скорости (Доработать)
            timeCount += Time.deltaTime * 3;
            yield return null;
        }
    }

    private void AddSpeedJumperY(float positionX, float positionY)
    {
        // Выключаем кинематику
        //_rigidbodyJumper.isKinematic = false;
        // Вычисляем скорость
        // var percentagePositionY = 100 - positionY * 100;
        // _speedUpJumper = percentagePositionY * _maximumJumpSpeed / 100;
        // _speedUpJumper = Mathf.Clamp(_speedUpJumper, 0, _maximumJumpSpeed);
        // _rigidbodyJumper.AddForce(new Vector3(positionX, 1, 0) * _speedUpJumper);
    }
}
