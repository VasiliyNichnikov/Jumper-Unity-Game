using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] [Header("Объект, за которым смотрим")]
    private Transform _player = null;

    [SerializeField] [Header("Скрипт, который хранит все позиции моделей")]
    private ListModelsTest _listModelsTest = null;

    [SerializeField] [Header("Главная камера (Transform)")]
    private Transform _mainCamera = null;
    
    [SerializeField] [Range(-50, 50)] [Header("Расстояние по оси X")]
    private float _offsetX = .0f;

    [SerializeField] [Range(-50, 50)] [Header("Расстояние по оси Y")]
    private float _offsetY = .0f;

    [SerializeField] [Range(0, 100)] [Header("Скорость движения камеры")]
    private float _speedCamera = .0f;

    [SerializeField] [Range(0, 10)] [Header("Скорость движения камеры по оси Y")]
    private float _speedCameraAxesY = .0f;

    [SerializeField] [Range(-45, 45)] [Header("Угол камеры по оси Y")]
    private float _angleY = .0f;

    [SerializeField] [Range(-45, 45)] [Header("Угол наклона по оси X")]
    private float _angleX = .0f;

    [HideInInspector]
    public float PositionY = .0f;
    
    private Transform _thisTransform = null;

    
    // Получение и изменение расстояние между камерой и игроком по оси X
    public float ChangeGetOffsetX
    {
        get { return _offsetX; }
        set
        { 
            _offsetX = value;
        }
    }
    
    // Получение и изменение расстояние между камерой и игроком по оси Y 
    public float ChangeGetOffsetY
    {
        get { return _offsetY; }
        set
        { 
            _offsetY = value;
        }
    }

    // Получение и изменение скорости камеры
    public float ChangeGetSpeed
    {
        get { return _speedCamera; }
        set { _speedCamera = value; }
    }
    
    // Получение и изменение поворота камеры по оси Y
    public float ChangeAngleRotationY
    {
        get { return _angleY; }
        set
        { 
            _angleY = value;
        }
    }
    
    // Получение и изменение поворота камеры по оси X
    public float ChangeAngleRotationX
    {
        get { return _angleX; }
        set
        { 
            _angleX = value;
        }
    }
    
    private void Start()
    {
        _thisTransform = transform;
        PositionY = _listModelsTest.GetSelectObject(_player).position.y;
    }
    
    private void Update()
    {
        _mainCamera.rotation = Quaternion.Euler(_angleX, _angleY, _mainCamera.eulerAngles.z);
        // _mainCamera.transform.position = new Vector3(_player.position.x - _offsetX, _player.position.y + _offsetY, _mainCamera.position.z);
        
        // _mainCamera.position = Vector3.MoveTowards(_mainCamera.position,
        //     new Vector3(_player.position.x - _offsetX, _mainCamera.position.y, _mainCamera.position.z), _speedCamera * Time.deltaTime);

        _thisTransform.position = new Vector3(
            Mathf.MoveTowards(_thisTransform.position.x, _player.position.x - _offsetX, _speedCamera * Time.deltaTime),
            Mathf.MoveTowards(_thisTransform.position.y, PositionY + _offsetY, _speedCameraAxesY * Time.deltaTime),
            _thisTransform.position.z);

        // _thisTransform.position = Vector3.Lerp(_thisTransform.position,
        //     new Vector3(_player.position.x - _offsetX, PositionY, _thisTransform.position.z), _speedCamera * Time.deltaTime);

    }
}
