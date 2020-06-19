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
    
    [Header("Скрипт, который высчитывает угол и высоту джампера")] [SerializeField]
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper = null;

    [Header("Скрипт, который придает скорость джамперу")] [SerializeField]
    private FlightJumper _flightJumper = null;

    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>

    [HideInInspector] // Хранит куротину, которая отвечает за прыжок джампера
    public IEnumerator CoroutineAnimationFlying = null;
    
    // Прыгает пользователь или нет
    public static bool JumpPlayer = false;
    
    // Проиграл пользователь или нет (Для теста)
    public static bool GameOverPlayer = false;
    
    // Переменная хранит начальную позицию пальца при нажатии на экран
    private Vector2 _startingPositionFinger = Vector2.zero;

    // Переменная хранит позицию на которой находится палец сейчас
    private Vector2 _nowPositionFinger = Vector2.zero;

    private void Start()
    {
        JumpPlayer = false;
        GameOverPlayer = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!JumpPlayer && !GameOverPlayer)
        {
            if (CoroutineAnimationFlying != null)
            {
                StopCoroutine(CoroutineAnimationFlying);
                CoroutineAnimationFlying = null;
            }
            
            _startingPositionFinger = new Vector2(_mainCamera.ScreenToViewportPoint(Input.mousePosition).x,
                _mainCamera.ScreenToViewportPoint(Input.mousePosition).y);
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!JumpPlayer && !GameOverPlayer)
        {
            _nowPositionFinger = new Vector2(_mainCamera.ScreenToViewportPoint(Input.mousePosition).x,
                _mainCamera.ScreenToViewportPoint(Input.mousePosition).y);

            _calculatingAngleHeightJumper.ChangeHeightAngleInclinationJumper(_nowPositionFinger,
                _startingPositionFinger);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!JumpPlayer && !GameOverPlayer)
        {
            print("Add Speed");
            _flightJumper.AddSpeedJumper();
            CoroutineAnimationFlying = _flightJumper.AnimationJumperLanding();
            StartCoroutine(CoroutineAnimationFlying);
            StartCoroutine(_calculatingAngleHeightJumper.ReturnUpperPartJumper(0.2f));
            JumpPlayer = true;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
