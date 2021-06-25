using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public enum AllParametersSettings
{
    Mass,
    AngleInclination,
    MaximumSpeed,
    Sensitivity,
    DistanceXCamera,
    DistanceYCamera,
    DistanceZCamera,
    SpeedCamera,
    AngleRotationCameraX,
    AngleRotationCameraY,
    AngleRotationCameraZ
}
public class ChangeSettings : MonoBehaviour
{
    [Serializable]
    private class SettingBlockClass
    {
        [Header("Название слайдера")] public string NameSlider = null;
        [Header("Параметр")] public AllParametersSettings ParameterSettings = AllParametersSettings.Mass;
    }

    [SerializeField] [Header("Блок префаба настроек")]
    private GameObject _blockPrefab;

    [SerializeField] [Header("Родитель префабов")]
    private Transform _parentPrefabs;
    
    [SerializeField] [Header("Настройки")]
    private SettingBlockClass[] _settingsBlocksClass = null;

    [SerializeField] [Header("Скрипт, который отслеживает прикосновения к экрану")]
    private ClickTracking _clickTracking = null;
    
    [SerializeField] [Header("Скрипт, который отвечает за полет джампера")]
    private FlightJumper _flightJumper = null;

    [SerializeField] [Header("Скрипт, который рассчитывает угол и высоту джампера")]
    private CalculatingAngleHeightJumper _calculatingAngleHeightJumper = null;

    [SerializeField] [Header("Скрипт, который управляет камерой")]
    private CameraTracking _cameraTracking = null;

    [SerializeField] [Header("Сохранять/Не сохранять параметры, которые стоят при старте")]
    private bool _savingParameters;

    private void Start()
    {
        CreateBlocksSettings();
    }
    
    private void CreateBlocksSettings()
    {
        var positionMaxY = .0f;

        for (int i = 0; i < _settingsBlocksClass.Length; i++)
        {
            GameObject newBlock = Instantiate(_blockPrefab, _parentPrefabs, false);
            RectTransform rectTransform = newBlock.GetComponent<RectTransform>();
            rectTransform.offsetMax = new Vector2(0, positionMaxY); // Смещение верхнего правого угла прямоугольника относительно верхнего правого якоря.

            if(_savingParameters)
                SaveNowSettings(_settingsBlocksClass[i].ParameterSettings);
            
            SettingBlock settingBlock = newBlock.GetComponent<SettingBlock>();
            // _settingsBlocksClass[i].SliderUI = settingBlock.SliderSetting;
            settingBlock.ParameterSettings = _settingsBlocksClass[i].ParameterSettings;
            settingBlock.KeyPlayerPrefs = _settingsBlocksClass[i].ParameterSettings.ToString();
            settingBlock.FillingText(_settingsBlocksClass[i].NameSlider);
            settingBlock.FillingInputAndSlider();
            positionMaxY -= 250.0f;
        }
    }

    private void SaveNowSettings(AllParametersSettings parameterSettings)
    {
        PlayerPrefs.SetFloat(parameterSettings.ToString(), ReturnSetting(parameterSettings));
    }
    
    // Возвращает параметр, который нужно поменять 
    private float ReturnSetting(AllParametersSettings parameterSettings)
    {
        float resultNumber = .0f;
        switch (parameterSettings)
        {
            case AllParametersSettings.Mass:
                resultNumber = _flightJumper.ChangeMassJumper;
                break;
            
            case AllParametersSettings.AngleInclination:
                resultNumber = _calculatingAngleHeightJumper.ChangeAngleInclination;
                break;
            
            case AllParametersSettings.MaximumSpeed:
                resultNumber = _flightJumper.ChangeMaximumSpeedJumper;
                break;
                
            case AllParametersSettings.Sensitivity:
                resultNumber = _clickTracking.ChangeSensitivityJumper;
                break;
                
            case AllParametersSettings.DistanceXCamera:
                resultNumber = _cameraTracking.ChangeGetOffsetX;
                break;
                
            case AllParametersSettings.DistanceYCamera:
                resultNumber = _cameraTracking.ChangeGetOffsetY;
                break;
                
            case AllParametersSettings.DistanceZCamera:
                resultNumber = _cameraTracking.ChangeGetOffsetZ;
                break;
                
            case AllParametersSettings.SpeedCamera:
                resultNumber = _cameraTracking.ChangeGetSpeed;
                break;
                
            case AllParametersSettings.AngleRotationCameraX:
                resultNumber = _cameraTracking.ChangeAngleRotationX;
                break;
                
            case AllParametersSettings.AngleRotationCameraY:
                resultNumber = _cameraTracking.ChangeAngleRotationY;
                break;
                
            case AllParametersSettings.AngleRotationCameraZ:
                resultNumber = _cameraTracking.ChangeAngleRotationZ;
                break;
        }

        return resultNumber;
    }
}
