using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum AllParametersSettings
{
    Mass,
    AngleInclination,
    MaximumSpeed,
    Sensitivity
}
public class ChangeSettings : MonoBehaviour
{
    [Serializable]
    private class NewSlider
    {
        [Header("Название слайдера")] public string NameSlider = null;
        [Header("Слайдер")] public Slider SliderUI = null;
        [Header("Текст рядом со слайдером")] public Text TextUI = null;
        [Header("Параметр")] public AllParametersSettings ParameterSettings = AllParametersSettings.Mass;
    }

    [SerializeField] [Header("Все слайдеры с параметрами")]
    private NewSlider[] _allSliders = null;

    [SerializeField] [Header("Скрипт, который отслеживает прикосновения к экрану")]
    private ClickTracking _clickTracking = null;
    
    [SerializeField] [Header("Скрипт, который отвечает за полет джампера")]
    private FlightJumper _flightJumper = null;

    [SerializeField] [Header("Скрипт, который рассчитывает угол и высоту джампера")]
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper = null;


    private void Start()
    {
        ChangeParameters(true);
    }

    private void Update()
    {
        ChangeParameters();
    }

    private void ChangeParameters(bool getParameters=false)
    {
        for (int i = 0; i < _allSliders.Length; i++)
        {
            switch (_allSliders[i].ParameterSettings)
            {
                case AllParametersSettings.Mass:
                    if (!getParameters)
                    {
                        _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_flightJumper.ChangeMassJumper}";
                        _flightJumper.ChangeMassJumper = _allSliders[i].SliderUI.value;
                    }
                    else
                    {
                        _allSliders[i].SliderUI.value = _flightJumper.ChangeMassJumper;
                    }
                    break;
                
                case AllParametersSettings.AngleInclination:
                    if (!getParameters)
                    {
                        _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_calculatingAngleHeightJumper.ChangeAngleInclination}";
                        _calculatingAngleHeightJumper.ChangeAngleInclination = _allSliders[i].SliderUI.value;
                    }
                    else
                    {
                        _allSliders[i].SliderUI.value = _calculatingAngleHeightJumper.ChangeAngleInclination;
                    }
                    break;
                
                case  AllParametersSettings.Sensitivity:
                    if (!getParameters)
                    {
                        _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_clickTracking.ChangeSensitivityJumper}";
                        _clickTracking.ChangeSensitivityJumper = _allSliders[i].SliderUI.value;
                    }
                    else
                    {
                        _allSliders[i].SliderUI.value = _clickTracking.ChangeSensitivityJumper;
                    }
                    break;
                
                case AllParametersSettings.MaximumSpeed:
                    if (!getParameters)
                    {
                        _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_flightJumper.ChangeMaximumSpeedJumper}";
                        _flightJumper.ChangeMaximumSpeedJumper = _allSliders[i].SliderUI.value;
                    }
                    else
                    {
                        _allSliders[i].SliderUI.value = _flightJumper.ChangeMaximumSpeedJumper;
                    }
                    break;
            }
        }
    }
    
    
    // private NewSlider GetNewSlider(AllParametersSettings allParametersSettings)
    // {
    //     for (int i = 0; i < _allSliders.Length; i++)
    //     {
    //         if (_allSliders[i].ParameterSettings == allParametersSettings)
    //             return _allSliders[i];
    //     }
    //
    //     return null;
    // }
    
    // [Header("Слайдер массы джампера")] [SerializeField]
    // private Slider _sliderMassJumper = null;
    //
    // [Header("Слайдер угла наклона джампера")] [SerializeField]
    // private Slider _sliderAngleInclination = null;
    //
    // [Header("Слайдер максимальной скорости прыжка джампера")] [SerializeField]
    // private Slider _sliderMaximumSpeed = null;
    //
    // [Header("Слайдер чувствительности джампера")] [SerializeField]
    // private Slider _sliderSensitivity = null;




}
