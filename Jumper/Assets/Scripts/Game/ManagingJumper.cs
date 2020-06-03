using System;
using System.Collections;
using UnityEngine;

// данный класс управляет джампером 
public class ManagingJumper : MonoBehaviour
{
    [Header("Главная камера")] [SerializeField]
    private Camera _mainCamera = null;

    [Header("Основная часть джампера")] [SerializeField]
    private GameObject _mainJumper = null;
    
    [Header("Верхняя часть джемпера")] [SerializeField]
    private Transform _upperPartJumper = null;

    [Header("RigidBody джампера")] [SerializeField]
    private Rigidbody _rigidbodyJumper = null;
    
    //[Header("Скрипт, который управляет камерой")] [SerializeField]
   // private FollowingCamera _followingCamera = null;
    
    [Header("Максимальный угол наклона по модулю")] [SerializeField]
    private float _maximumAngleInclination = .0f;

    [Header("Максимальная скорость прыжка")] [SerializeField] [Range(0, 100)]
    private float _maximumJumpSpeed = .0f;
    
    [Header("Минимальное сжатие джампа")] [SerializeField] [Range(0f, 1f)]
    private float _minimumJumpCompression = .0f;
        
    [Header("Максимальное сжатие джампа")] [SerializeField] [Range(0f, 1f)]
    private float _maximumJumpCompression = .0f;
    
    // Разница координат по X (Нужно для понимания, куда идет палец (Право/Влево))
    private float _differenceX = .0f;

    //"Скорость поднятия на высоту джампера"
    private float _speedUpJumper = .0f;
    
    // Возвращает скорость джампера
    // public float GetSpeedUpJumper
    // {
    //     get { return _speedUpJumper; }
    // }
    //
    // public float GetDifferenceX
    // {
    //     get { return _differenceX; }
    // }
    
    // Стартовые позиции
    private Vector2 _startingPosition = Vector2.zero;

    // Данная переменная, проверяет анимируется ли верхняя часть джампера или нет
    private bool _animationTopPartJumper = false;
    
    public void ChangeRigidbodyKinematic(bool condition)
    {
        _rigidbodyJumper.isKinematic = condition;
    }

    public void StartAnimationTopJumper()
    {
        if(_upperPartJumper.localPosition.y != 1 && !_animationTopPartJumper)
            StartCoroutine(ReturnStartingTopJumperPosition(1, 1));
    }
    

    private void OnMouseDown()
    {
        if (InspectionGround.IsGround)
        {
            _startingPosition = new Vector2(_mainCamera.ScreenToViewportPoint(Input.mousePosition).x, 
            _mainCamera.ScreenToViewportPoint(Input.mousePosition).y);
        }
    }

    private void OnMouseDrag()
    {
        print("Drag");
        if (InspectionGround.IsGround)
        {
            print("IsGround");
            _upperPartJumper.localPosition = new Vector3(
                _upperPartJumper.localPosition.x, 
                GetPositionJumper(), 
                _upperPartJumper.localPosition.z);

            _rigidbodyJumper.gameObject.transform.localRotation = Quaternion.Euler(
                _upperPartJumper.localRotation.x,
                _upperPartJumper.localRotation.y, 
                GetAngleRotationJumper());
        }
    }
    
    private void OnMouseUp()
    {
        if (InspectionGround.IsGround)
        {
            AddSpeedJumperY(GetPositionX(), GetPositionJumper());
            if(!_animationTopPartJumper)
                StartCoroutine(ReturnStartingTopJumperPosition(GetPositionTopPartJumper(), 1.5f));
            StartCoroutine(ReturnStartingTopJumperRotation());
            //StartCoroutine(_followingCamera.AnimationCamera(_differenceX));
            InspectionGround.IsGround = false;
        }
    }
    
    // Возвращает до какой позиции надо сдвигать высшую ножку джампера
    private float GetPositionTopPartJumper()
    {
        var percent = _speedUpJumper * 100 / _maximumJumpSpeed;
        //print(percent);
        if (percent > 30)
            return 0.8f;
        return 1f;
    }
    
    // Сделать изменение в зависимости от скорости
    private float GetPositionX()
    {
        float posX = 1f;
        //print(GetAngleRotationJumper());
        if (GetAngleRotationJumper() > 3f)
            posX = -1f;
        else if (GetAngleRotationJumper() <= 3f && GetAngleRotationJumper() >= 0)
            posX = 0.0f;
        return posX;
    }
    
    // Возвращает сжатие прыжка
    private float GetPositionJumper()
    {
        float positionNowY = _mainCamera.ScreenToViewportPoint(Input.mousePosition).y;
        float differenceY = _startingPosition.y - positionNowY;
        float percentageScreenY = 100 - differenceY * 100;
        float jumpCompressionY = percentageScreenY * _maximumJumpCompression / 100;
        jumpCompressionY = Mathf.Clamp(jumpCompressionY, _minimumJumpCompression, _maximumJumpCompression);
        return jumpCompressionY;
    }

    // Возвращает нужный поворот (Доделать)
    private float GetAngleRotationJumper()
    {
        float positionNowX = _mainCamera.ScreenToViewportPoint(Input.mousePosition).x;
        float differenceX = _startingPosition.x - positionNowX;
        _differenceX = differenceX;

        float percentageScreenX = differenceX * 100;
        
        var angleInclination = percentageScreenX * _maximumAngleInclination / 100;
        angleInclination = Mathf.Clamp(angleInclination, -_maximumAngleInclination, _maximumAngleInclination);
        return angleInclination;
    }
    
    private IEnumerator ReturnStartingTopJumperPosition(float posY, float speed)
    {
        _animationTopPartJumper = true;
        while (_upperPartJumper.localPosition.y != posY)
        {
            _upperPartJumper.localPosition = Vector3.MoveTowards(_upperPartJumper.localPosition,
                new Vector3(_upperPartJumper.localPosition.x, posY,
                    _upperPartJumper.localPosition.z), speed * Time.deltaTime);
            //print("work");
            yield return null;
        }
        _animationTopPartJumper = false;
    }

    private IEnumerator ReturnStartingTopJumperRotation()
    {
        var timeCount = .0f;
        while (_mainJumper.transform.rotation != new Quaternion(0, 0, 0,1f))
        {
            _mainJumper.transform.rotation = Quaternion.Slerp(_mainJumper.transform.rotation, new Quaternion(0, 0, 0,1f), timeCount);
            // Должно умножаться в зависимости от скорости
            timeCount += Time.deltaTime * AlignmentSpeedJumper();
            //print(AlignmentSpeedJumper());
            yield return null;
        }

        _rigidbodyJumper.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //print("End");
    }

    private float AlignmentSpeedJumper()
    {
        return (_maximumJumpSpeed - _speedUpJumper) / 200;
    }
    
    private void AddSpeedJumperY(float positionX, float positionY)
    {
        // Выключаем кинематику
        _rigidbodyJumper.isKinematic = false;
        // Вычисляем скорость
        var percentagePositionY = 100 - positionY * 100;
        _speedUpJumper = percentagePositionY * _maximumJumpSpeed / 100;
        _speedUpJumper = Mathf.Clamp(_speedUpJumper, 0, _maximumJumpSpeed);
        _rigidbodyJumper.AddForce(new Vector3(positionX, 1, 0) * _speedUpJumper);
    }
}
