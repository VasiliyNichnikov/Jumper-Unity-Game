using System;
using System.Collections;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] [Header("Объект, за которым смотрим")]
    private Transform _player = null;

    [SerializeField] [Header("Главная камера (Transform)")]
    private Transform _mainCamera = null;

    [SerializeField] [Range(-50, 50)] [Header("Расстояние по оси X")]
    private float _offsetX = .0f;

    [SerializeField] [Range(-50, 50)] [Header("Расстояние по оси Y")]
    private float _offsetY = .0f;

    [SerializeField] [Range(-50, 50)] [Header("Расстояние по оси Z")]
    private float _offsetZ = .0f;

    [SerializeField] [Range(0, 100)] [Header("Скорость движения камеры")]
    private float _speedCamera = 45f;

    [SerializeField] [Range(0, 10)] [Header("Скорость движения камеры по оси Y")]
    private float _speedCameraAxesY = .0f;

    [SerializeField] [Range(0, 100)] [Header("Скорость движения камеры во время анимации")]
    private float _speedCameraMoveAnimation;
    
    [SerializeField] [Range(0, 100)] [Header("Скорость вращения камеры во время анимации")]
    private float _speedCameraRotationAnimation;

    //[Header("Стартовая точка высоты камеры")]
    //public Transform StartTransformPoint;
    
    [Range(-45, 360)] [Header("Угол камеры по оси Y")]
    public float _angleY = 220;

    [Range(-45, 360)] [Header("Угол наклона по оси X")]
    public float _angleX = .0f;

    [Range(-45, 360)] [Header("Угол наклона по оси Z")]
    public float _angleZ = .0f;

    [HideInInspector] public float PositionY = .0f;
    
    private Transform _thisTransform = null;

    // Вектор куда должен двигаться игрок
    private Vector3 _positionPlayerMovement = Vector3.zero;

    // Получение и изменение расстояние между камерой и игроком по оси X
    public float ChangeGetOffsetX
    {
        get { return _offsetX; }
        set { _offsetX = value; }
    }

    // Получение и изменение расстояние между камерой и игроком по оси Y 
    public float ChangeGetOffsetY
    {
        get { return _offsetY; }
        set { _offsetY = value; }
    }

    // Получение и изменение расстояние между камерой и игроком по оси Z
    public float ChangeGetOffsetZ
    {
        get { return _offsetZ; }
        set { _offsetZ = value; }
    }

    // Получение и изменение скорости камеры
    public float ChangeGetSpeed
    {
        get { return _speedCamera; }
        set { _speedCamera = value; }
    }

    // Получение и изменение поворота камеры по оси X
    public float ChangeAngleRotationX
    {
        get { return _angleX; }
        set { _angleX = value; }
    }

    // Получение и изменение поворота камеры по оси Y
    public float ChangeAngleRotationY
    {
        get { return _angleY; }
        set { _angleY = value; }
    }

    // Получение и изменение поворота камеры по оси Z
    public float ChangeAngleRotationZ
    {
        get { return _angleZ; }
        set { _angleZ = value; }
    }

    // PositionY, когда будет генерация, нужно добавить первый объект, который создался
    private void Start()
    {
        _thisTransform = transform;
        // StartCoroutine(AnimationCameraStart());
        PositionY = _player.position.y;
        _loadingScene = true;
        // PositionY = _listModelsTest.GetSelectObject(_player).position.y;
    }

    private bool _loadingScene = false;
    
    private IEnumerator AnimationCameraStart()
    {
        //float positionX = PlayerPrefs.GetFloat("");

        Vector3 positionEnd = new Vector3(8.02f, 3.62f, 6f);
        Quaternion rotationEnd = Quaternion.Euler(_angleX, _angleY, _angleZ);
        
        while (_mainCamera.localPosition != positionEnd)
        {
            _mainCamera.localPosition = Vector3.MoveTowards(_mainCamera.localPosition, positionEnd,
                _speedCameraMoveAnimation * Time.deltaTime);
            _mainCamera.localRotation = Quaternion.Lerp(_mainCamera.localRotation, rotationEnd,
                _speedCameraRotationAnimation * Time.deltaTime);
            
            _positionPlayerMovement = new Vector3(
                Mathf.MoveTowards(_thisTransform.position.x, _player.position.x - _offsetX,
                    _speedCamera * Time.deltaTime),
                Mathf.MoveTowards(_thisTransform.position.y, PositionY + _offsetY, _speedCameraAxesY * Time.deltaTime),
                Mathf.MoveTowards(_thisTransform.position.z, _player.position.z - _offsetZ,
                    _speedCamera * Time.deltaTime));
            _thisTransform.position = _positionPlayerMovement;
            yield return null;
        }

        _loadingScene = true;
        print("Loading End");
    }

    private void Update()
    {
        if (_loadingScene)
        {
            _mainCamera.localRotation = Quaternion.Euler(_angleX, _angleY, _angleZ);
    
            if (ClickTracking.GameOverPlayer)
                _positionPlayerMovement = _thisTransform.position;
            else
            {
                _positionPlayerMovement = new Vector3(
                    Mathf.MoveTowards(_thisTransform.position.x, _player.position.x - _offsetX,
                        _speedCamera * Time.deltaTime),
                    Mathf.MoveTowards(_thisTransform.position.y, PositionY + _offsetY, _speedCameraAxesY * Time.deltaTime),
                    Mathf.MoveTowards(_thisTransform.position.z, _player.position.z - _offsetZ,
                        _speedCamera * Time.deltaTime));
            }
    
            _thisTransform.position = _positionPlayerMovement;
        }
        
    }
}