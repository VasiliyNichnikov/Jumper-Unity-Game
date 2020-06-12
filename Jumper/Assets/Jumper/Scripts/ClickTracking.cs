using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

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
    
    // Переменная хранит начальную позицию пальца при нажатии на экран
    private Vector2 _startingPositionFinger = Vector2.zero;

    // Переменная хранит позицию на которой находится палец сейчас
    private Vector2 _nowPositionFinger = Vector2.zero;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _startingPositionFinger = new Vector2(_mainCamera.ScreenToViewportPoint(Input.mousePosition).x, 
            _mainCamera.ScreenToViewportPoint(Input.mousePosition).y);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _nowPositionFinger = new Vector2(_mainCamera.ScreenToViewportPoint(Input.mousePosition).x, 
            _mainCamera.ScreenToViewportPoint(Input.mousePosition).y);

        _calculatingAngleHeightJumper.ChangeHeightAngleInclinationJumper(_nowPositionFinger, _startingPositionFinger);
        //print($"Стартовая позиция: {_startingPositionFinger}; Позиция сейчас: {_nowPositionFinger};");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("Add Speed");
        _flightJumper.AddSpeedJumper();
        StartCoroutine(_calculatingAngleHeightJumper.ReturnUpperPartJumper());
    }
}
