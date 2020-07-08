﻿using System;
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

    [Header("Максимальный процент экрана, который нужен для максимальной скорости джампера")] [Range(0, 100)]
    public int MaximumPercentScreenForMaximumSpeedJumper = 0;
    
    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>

    // Максимальная высота верхней части джампера (Задается при старте)
    private float _maximumHeightJumper = .0f;
    
    private float _percentHeightJumper = .0f;
    private float _percentAngleJumper = .0f;
    
    public float GerPercentHeightJumper
    {
        get { return MaximumPercentScreenForMaximumSpeedJumper - Mathf.Abs(_percentHeightJumper); }
    }

    public float GetPercentAngleJumper
    {
        get { return Mathf.Abs(_percentAngleJumper); }
    }
    
    // Для настроек джампера(Начало)
    public float ChangeAngleInclination
    {
        set
        {
            if (value <= 90) _maximumAngleInclination = value;
        }

        get
        {
            return _maximumAngleInclination;
        }
    }
    // Для настроек джампера(Конец)
    
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
        var percentageScreenHeight = ConversionValuesPercent(nowPositionY, startPositionY);
        _percentHeightJumper = Mathf.Clamp(percentageScreenHeight, 0, MaximumPercentScreenForMaximumSpeedJumper);
        //print(_percentHeightJumper);
        var positionUpperPartJumperY =
            InterestValue(percentageScreenHeight, _minimumHeightUpperPart, _maximumHeightJumper);
        return positionUpperPartJumperY;
    }
    
    // Данный метод меняем угол джампера в зависимости от расположения пальца 
    private float GetAngleInclinationJumper(float nowPositionX, float startPositionX)
    {
        var percentageScreenWidth = (startPositionX - nowPositionX) * MaximumPercentScreenForMaximumSpeedJumper;
        var angleInclination = InterestValue(percentageScreenWidth, -_maximumAngleInclination, _maximumAngleInclination);
        _percentAngleJumper = Mathf.Abs(angleInclination) * MaximumPercentScreenForMaximumSpeedJumper / _maximumAngleInclination;
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
    }
    
    // Данный метод вычисляет проценты от 0 до 100
    private float ConversionValuesPercent(float nowValue, float maximum)
    {
        return MaximumPercentScreenForMaximumSpeedJumper * nowValue / maximum;
    }

    // Данный метод преобразует проценты в значение
    private float InterestValue(float percentNow, float minimum, float maximum)
    {
        return Mathf.Clamp(maximum * percentNow / MaximumPercentScreenForMaximumSpeedJumper, minimum, maximum);
    }
}
