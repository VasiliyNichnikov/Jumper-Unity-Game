using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
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

    [Header("Чувствительность джампера")] [SerializeField] [Range(1, 10)]
    private float _sensitivityJumper = .0f;
    
    [Header("Скрипт, который высчитывает угол и высоту джампера")] [SerializeField]
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper = null;

    [Header("Скрипт, который придает скорость джамперу")] [SerializeField]
    private FlightJumper _flightJumper = null;

    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>

    [HideInInspector] // Нажимал пользователь на экран или нет (Нужно для проверки, чтобы завершить цикл посадки)
    public bool FingerInputScreen = false;
    
    // Прыгает пользователь или нет
    public static bool JumpPlayer = false;
    
    // Проиграл пользователь или нет (Для теста)
    public static bool GameOverPlayer = false;
    
    // Переменная хранит начальную позицию пальца при нажатии на экран
    private Vector2 _startingPositionFinger = Vector2.zero;

    // Переменная хранит позицию на которой находится палец сейчас
    private Vector2 _nowPositionFinger = Vector2.zero;

    // Двигал пользователь палец или нет
    private bool _playerDragFinger = false;
    
    // Позиция мыши при нажатии
    private Vector2 _startMousePosition = Vector2.zero;

    public float ChangeSensitivityJumper
    {
        get { return _sensitivityJumper;}
        set { if(value <= 10) _sensitivityJumper = value;}
    }

    // public Vector2 GetStartPositionFinger
    // {
    //     get { return _startMousePosition; }
    // }
    //
    // public Vector3 GetNowPositionFinger
    // {
    //     get { return _nowPositionFinger; }
    // }
    
    private void Start()
    {
        JumpPlayer = false;
        GameOverPlayer = false;
        _playerDragFinger = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!JumpPlayer && !GameOverPlayer)
        {
            _startMousePosition = Input.mousePosition;
            _startingPositionFinger = new Vector2(_mainCamera.ScreenToViewportPoint(Input.mousePosition).x,
                _mainCamera.ScreenToViewportPoint(Input.mousePosition).y);
            FingerInputScreen = true;
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!JumpPlayer && !GameOverPlayer)
        {
            FingerInputScreen = true;
            
            _playerDragFinger = true;
            _nowPositionFinger = new Vector2(_mainCamera.ScreenToViewportPoint(Input.mousePosition).x,
                _mainCamera.ScreenToViewportPoint(Input.mousePosition).y);

            _calculatingAngleHeightJumper.ChangeHeightAngleInclinationJumper(_nowPositionFinger * _sensitivityJumper,
                _startingPositionFinger * _sensitivityJumper);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!JumpPlayer && !GameOverPlayer && _playerDragFinger)
        {
            JumpPlayer = true;
            FingerInputScreen = false;
            _playerDragFinger = false;
            _flightJumper.AddSpeedJumper();
            StartCoroutine(_flightJumper.AnimationStartJumper());
            StartCoroutine(_calculatingAngleHeightJumper.ReturnUpperPartJumper());
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
