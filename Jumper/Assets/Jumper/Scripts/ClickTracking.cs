using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Данный скрипт отслеживает нажатия пользователя.
/// Проверяет, когда пользователь нажал на экран и когда пользователь отпустил палец с него.
/// </summary>
public class ClickTracking : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    /// <summary>
    /// Переменные, которые отображаются в инспекторе 
    /// </summary>
    [Header("Главная камера на сцене")] [SerializeField]
    private Camera _mainCamera = null;
    
    [Header("Максимальный процент экрана, который нужен для максимальной скорости джампера")] [Range(0, 100)] [SerializeField]
    private int _maximumPercentScreenForMaximumSpeedJumper = 0;
    
    [Header("Чувствительность джампера")] [SerializeField] [Range(1, 10)]
    private float _sensitivityJumper = .0f;
    
    [Header("Скрипт, который высчитывает угол и высоту джампера")] [SerializeField]
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper = null;

    [Header("Скрипт, который придает скорость джамперу")] [SerializeField]
    private FlightJumper _flightJumper = null;

    [Header("Скрипт, который рисует трессер")] [SerializeField]
    private FingerMovement _fingerMovement = null;
    
    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>

    [HideInInspector] // Нажимал пользователь на экран или нет (Нужно для проверки, чтобы завершить цикл посадки)
    public bool FingerInputScreen = false;
    
    // Переменная хранит время движения пальца
    //private float _stopwatchMoveFinger = .0f;
    
    // Прыгает пользователь или нет
    public static bool JumpPlayer = false;
    
    // Проиграл пользователь или нет (Для теста)
    public static bool GameOverPlayer = false;

    // Переменная хранит начальную позицию пальца при нажатии на экран
    private Vector2 _startPositionFinger = Vector2.zero;

    // Переменная хранит позицию на которой находится палец сейчас
    private Vector2 _nowPositionFinger = Vector2.zero;

    // Двигал пользователь палец или нет
    private bool _playerDragFinger = false;

    // Настройки джампера (Начало)
    public float ChangeSensitivityJumper
    {
        get { return _sensitivityJumper;}
        set { if(value <= 10) _sensitivityJumper = value;}
    }
    // Настройки джампера (Конец)

    private void Start()
    {
        JumpPlayer = false;
        GameOverPlayer = false;
        _playerDragFinger = false;
    }

    // Позиции пальца без переводов в экранные разрешения
    private Vector3 _inputStart = Vector3.zero;
    private Vector3 _inputEnd = Vector3.zero;
    
    // Нажатие на экран
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!JumpPlayer && !GameOverPlayer)
        {
            //_stopwatchMoveFinger = 0;
            _startPositionFinger = new Vector2(_mainCamera.ScreenToViewportPoint(Input.mousePosition).x,
                _mainCamera.ScreenToViewportPoint(Input.mousePosition).y);
            FingerInputScreen = true;
            _inputStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }
    }
    
    // Движение пальца по экрану
    public void OnDrag(PointerEventData eventData)
    {
        if (!JumpPlayer && !GameOverPlayer)
        {
            //_stopwatchMoveFinger += Time.deltaTime;
            // if (_stopwatchMoveFinger > 0 && _sensitivityJumper > 0)
            //     _sensitivityJumper *= _stopwatchMoveFinger;
            
            //_sensitivityJumper *= _stopwatchMoveFinger; 
            //print($"Секундомер - {_stopwatchMoveFinger};");
            //if (_calculatingAngleHeightJumper.GetPercentAngleJumper >= 60)
              // _sensitivityJumper = 0.0001f;
            // else
            //     _sensitivityJumper = 2;
            
            FingerInputScreen = true;
            _playerDragFinger = true;
            
            _nowPositionFinger = new Vector2(_mainCamera.ScreenToViewportPoint(Input.mousePosition).x,
                _mainCamera.ScreenToViewportPoint(Input.mousePosition).y);

            _calculatingAngleHeightJumper.ChangeHeightAngleInclinationJumper(_nowPositionFinger * _sensitivityJumper,
                _startPositionFinger * _sensitivityJumper, ChangingSensitivityPercentage());
            
            _inputEnd = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            // Отображение и обнавление линии
            _fingerMovement.ShowAndHideLine = true;
            _fingerMovement.UpdateLine(_inputStart, _inputEnd);
        }
    }
    
    // Отпуск пальца
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!JumpPlayer && !GameOverPlayer && _playerDragFinger)
        {
            FingerInputScreen = false;
            _playerDragFinger = false;
            _fingerMovement.ShowAndHideLine = false;
            _flightJumper.AddSpeedJumper();
            StartCoroutine(_flightJumper.AnimationStartJumper());
            StartCoroutine(_calculatingAngleHeightJumper.ReturnUpperPartJumper());
        }
    }
    
    // Перезагрузка игры
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Смена чувствительности
    private float ChangingSensitivityPercentage()
    {
        return _sensitivityJumper * _maximumPercentScreenForMaximumSpeedJumper / 100;
    }
    
    
}
