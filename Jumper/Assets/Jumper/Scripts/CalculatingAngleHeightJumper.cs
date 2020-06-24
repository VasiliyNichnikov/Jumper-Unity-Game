using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данный скрипт считает с какой скоростью и под каким углом кинуть Джампер.
/// Все это зависит от того насколько сильно пользователь потянет палец.
/// </summary>
public class CalculatingAngleHeightJumper : MonoBehaviour
{
    ///<summary>
    /// Переменные, которые отображаются в инспекторе 
    ///</summary>
    
    [Header("Верхняя часть джампера")] [SerializeField]
    private GameObject _upperPartJumper = null;

    [Header("Минимальная высота до которой может сжиматься джампер")] [SerializeField] [Range(0, 1)]
    private float _minimumHeightUpperPart = .0f;

    [Header("Максимальный угол наклона джампера (По модулю)")] [SerializeField] [Range(0, 180)]
    private float _maximumAngleInclination = .0f;

    [Header("Скорость с которой джампер разжимается")] [SerializeField] [Range(0, 100)]
    private float _speedDecompressedJumper = .0f;

    // [Header("Вектор для теста")] [SerializeField]
    // private Transform _vector3Start = null;

    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>

    // Максимальная высота верхней части джампера (Задается при старте)
    private float _maximumHeightJumper = .0f;
    
    private float _percentHeightJumper = .0f;
    private float _percentAngleJumper = .0f;
    
    public float GerPercentHeightJumper
    {
        get { return 100 - Mathf.Abs(_percentHeightJumper); }
    }

    public float GetPercentAngleJumper
    {
        get { return 100 - Mathf.Abs(_percentAngleJumper); }
    }
    
    // transform верхней части джампера
    private Transform _transformUpperPartJumper = null;

    private Transform _thisTransform = null;

    private void Start()
    {
        _transformUpperPartJumper = _upperPartJumper.transform;
        _maximumHeightJumper = _transformUpperPartJumper.localPosition.y;
        _thisTransform = GetComponent<Transform>();
    }
    
    // Данный метод меняет высоту и угол наклона джампера, в зависимости от нахождения пальца
    public void ChangeHeightAngleInclinationJumper(Vector2 nowPosition, Vector2 startPosition)
    {
        _transformUpperPartJumper.localPosition = 
            new Vector3(0, GetHeightUpperPartJumper(nowPosition.y, startPosition.y), 0);
        _thisTransform.localRotation = Quaternion.Euler(0, 0, GetAngleInclinationJumper(nowPosition.x, startPosition.x));
    }
    
    // Данный метод возвращает высоту до которой нужно опустить верхнюю часть джампера
    private float GetHeightUpperPartJumper(float nowPositionY, float startPositionY)
    {
        //print($"Now pos Y - {Mathf.Abs(nowPositionY)}");
        var percentageScreenHeight = ConversionValuesPercent(nowPositionY, startPositionY);//startPositionY);
        _percentHeightJumper = Mathf.Clamp(percentageScreenHeight, 0, 100);
        //print($"Процент высоты: {percentageScreenHeight}");
        var positionUpperPartJumperY =
            InterestValue(percentageScreenHeight, _minimumHeightUpperPart, _maximumHeightJumper);
        return positionUpperPartJumperY;
    }
    
    // Данный метод меняем угол джампера в зависимости от расположения пальца 
    private float GetAngleInclinationJumper(float nowPositionX, float startPositionX)
    {
        // print(ConversionValuesPercent(nowPositionX, nowPositionX));
        var percentageScreenWidth = (startPositionX - nowPositionX) * 100;
        //var percentageScreenWidth = ConversionValuesPercent(nowPositionX, startPositionX);
        //_percentAngleJumper = Mathf.Clamp(percentageScreenWidth, 0, 100);
        //print(percentageScreenWidth);
        //print($"Процент угла: {_percentAngleJumper}");
        var angleInclination = InterestValue(percentageScreenWidth, -_maximumAngleInclination, _maximumAngleInclination);
        percentageScreenWidth = Mathf.Abs(angleInclination) * 100 / _maximumAngleInclination;
        return angleInclination;
    }
    
    // Возвращает высоту верхней части джампера к исходному положению
    public IEnumerator ReturnUpperPartJumper(float subtractHeight=0)
    {
        Vector3 heightJumper = new Vector3(_transformUpperPartJumper.localPosition.x, _maximumHeightJumper - subtractHeight, 
            _transformUpperPartJumper.localPosition.z);
        while (_transformUpperPartJumper.localPosition.y != heightJumper.y)
        {
            _transformUpperPartJumper.localPosition = Vector3.MoveTowards(_transformUpperPartJumper.localPosition, heightJumper,
                _speedDecompressedJumper * Time.deltaTime);
            yield return null;
        }
        //print("Stop Height");
    }
    
    // Данный метод вычисляет проценты от 0 до 100
    private float ConversionValuesPercent(float nowValue, float maximum)
    {
        return 100 * nowValue / maximum;
    }

    // Данный метод преобразует проценты в значение
    private float InterestValue(float percentNow, float minimum, float maximum)
    {
        return Mathf.Clamp(maximum * percentNow / 100, minimum, maximum);
    }

    private void Update()
    {
        //print(transform.rotation.eulerAngles);
       // print(Vector3.right + transform.position);
    }
    
}
