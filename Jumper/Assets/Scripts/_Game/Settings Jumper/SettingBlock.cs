using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingBlock : MonoBehaviour
{
    [Header("Текст")] public Text NameSetting;

    [Header("Ввод значения")] public InputField InputValue;

    [Header("Слайдер")] public Slider SliderSetting;

    [HideInInspector] public AllParametersSettings ParameterSettings;

    [HideInInspector] public string KeyPlayerPrefs; // Ключ, который хранится в PlayerPrefs
    
    private ClickTracking _clickTracking = null; // Скрипт, который отслеживает прикосновения к экрану
    
    private FlightJumper _flightJumper = null; // Скрипт, который отвечает за полет джампера
    
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper = null; // Скрипт, который рассчитывает угол и высоту джампера
    
    private CameraTracking _cameraTracking = null; // Скрипт, который управляет камерой

    private void Awake()
    {
        _clickTracking = FindObjectOfType<ClickTracking>();
        _flightJumper = FindObjectOfType<FlightJumper>();
        _calculatingAngleHeightJumper = FindObjectOfType<CalculatingAngleHeightJumper>();
        _cameraTracking = FindObjectOfType<CameraTracking>();
    }

    public void FillingText(string text)
    {
        NameSetting.text = text;
    }

    public void FillingInputAndSlider()
    {
        float value = PlayerPrefs.GetFloat(KeyPlayerPrefs);
        InputValue.text = value.ToString();
        SliderSetting.value = value;
    }

    public void ChangeValueSlider()
    {
        string value = SliderSetting.value.ToString();
        InputValue.text = value;
        SaveParameter(float.Parse(value));
        ChangeSetting();
    }

    public void ChangeValueInput()
    {
        float value = float.Parse(InputValue.text);
        SliderSetting.value = value;
        SaveParameter(value);
        ChangeSetting();
    }

    // Сохранение параметра
    private void SaveParameter(float value)
    {
        PlayerPrefs.SetFloat(KeyPlayerPrefs, value);
    }

    private void ChangeSetting()
    {
        float resultNumber = .0f;
        switch (ParameterSettings)
        {
            case AllParametersSettings.Mass:
                _flightJumper.ChangeMassJumper = PlayerPrefs.GetFloat(KeyPlayerPrefs);
                break;

            case AllParametersSettings.AngleInclination:
                _calculatingAngleHeightJumper.ChangeAngleInclination =  PlayerPrefs.GetFloat(KeyPlayerPrefs);
                break;

            case AllParametersSettings.MaximumSpeed:
                _flightJumper.ChangeMaximumSpeedJumper =  PlayerPrefs.GetFloat(KeyPlayerPrefs);
                break;

            case AllParametersSettings.Sensitivity:
                _clickTracking.ChangeSensitivityJumper =  PlayerPrefs.GetFloat(KeyPlayerPrefs);
                break;

            case AllParametersSettings.DistanceXCamera:
                _cameraTracking.ChangeGetOffsetX =  PlayerPrefs.GetFloat(KeyPlayerPrefs);
                break;

            case AllParametersSettings.DistanceYCamera:
                _cameraTracking.ChangeGetOffsetY =  PlayerPrefs.GetFloat(KeyPlayerPrefs);
                break;

            case AllParametersSettings.DistanceZCamera:
                _cameraTracking.ChangeGetOffsetZ =  PlayerPrefs.GetFloat(KeyPlayerPrefs);
                break;

            case AllParametersSettings.SpeedCamera:
                _cameraTracking.ChangeGetSpeed =  PlayerPrefs.GetFloat(KeyPlayerPrefs);
                break;

            case AllParametersSettings.AngleRotationCameraX:
                _cameraTracking.ChangeAngleRotationX =  PlayerPrefs.GetFloat(KeyPlayerPrefs);
                break;

            case AllParametersSettings.AngleRotationCameraY:
                _cameraTracking.ChangeAngleRotationY =  PlayerPrefs.GetFloat(KeyPlayerPrefs);
                break;

            case AllParametersSettings.AngleRotationCameraZ:
                _cameraTracking.ChangeAngleRotationZ =  PlayerPrefs.GetFloat(KeyPlayerPrefs);
                break;
        }
    }
}