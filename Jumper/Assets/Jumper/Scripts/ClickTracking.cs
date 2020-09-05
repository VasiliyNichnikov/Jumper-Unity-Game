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
    private Camera _mainCamera;

    [Header("Максимальный процент экрана, который нужен для максимальной скорости джампера")]
    [Range(0, 100)]
    [SerializeField]
    private int _maximumPercentScreenForMaximumSpeedJumper;
    
    [Header("Чувствительность джампера")] [SerializeField] [Range(1, 10)]
    private float _sensitivityJumper;
    
    [Header("Скрипт, который высчитывает угол и высоту джампера")] [SerializeField]
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper;

    [Header("Скрипт, который придает скорость джамперу")] [SerializeField]
    private FlightJumper _flightJumper;

    [Header("Скрипт, который рисует трессер")] [SerializeField]
    private FingerMovement _fingerMovement;
    
    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>

    // Нажимал пользователь на экран или нет (Нужно для проверки, чтобы завершить цикл посадки)
    public static bool FingerInputScreen;

    // Прыгает пользователь или нет
    public static bool JumpPlayer;
    
    // Проиграл пользователь или нет (Для теста)
    public static bool GameOverPlayer;

    // Переменная хранит начальную позицию пальца при нажатии на экран
    private Vector2 _startPositionFinger = Vector2.zero;

    // Переменная хранит позицию на которой находится палец сейчас
    private Vector2 _nowPositionFinger = Vector2.zero;

    // Двигал пользователь палец или нет
    private bool _playerDragFinger;
    
    // Игрок нажал на экран
    private bool _playerDownFinger;

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
        FingerInputScreen = false;
    }

    // Позиции пальца без переводов в экранные разрешения
    private Vector3 _inputStart = Vector3.zero;
    private Vector3 _inputEnd = Vector3.zero;
    
    // Нажатие на экран
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!JumpPlayer && !GameOverPlayer)
        {
            print("Down");
            _playerDownFinger = true;
            _startPositionFinger = new Vector2(_mainCamera.ScreenToViewportPoint(Input.mousePosition).x,
                _mainCamera.ScreenToViewportPoint(Input.mousePosition).y);
            FingerInputScreen = true;
            _inputStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }
    }
    
    // Движение пальца по экрану
    public void OnDrag(PointerEventData eventData)
    {
        if (!JumpPlayer && !GameOverPlayer && _playerDownFinger)
        {
            print("Drag");
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
            print("Up");
            FingerInputScreen = false;
            _playerDownFinger = false;
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
