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
        // [Header("Слайдер")] public Slider SliderUI = null;
        // [Header("Текст рядом со слайдером")] public Text TextUI = null;
        // public float Value
        // {
        //     get
        //     {
        //         if (SliderUI != null) return SliderUI.value;
        //         else return .0f;
        //     }
        // }
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
            // rectTransform.offsetMin = new Vector2(0, positionMinY); // Смещение нижнего левого угла прямоугольника относительно нижнего левого якоря.
            
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
            // if (!_saveStart)
            // {
            //     saveSettings.Mass = _settingsBlocksClass[i].Value;
            //     _flightJumper.ChangeMassJumper = _settingsBlocksClass[i].Value;
            // }
            // else
            //     saveSettings.Mass = _flightJumper.ChangeMassJumper;
            break;
            
        case AllParametersSettings.AngleInclination:
            resultNumber = _calculatingAngleHeightJumper.ChangeAngleInclination;
            // if (!_saveStart)
            // {
            //     saveSettings.AngleInclination = _settingsBlocksClass[i].Value;
            //     _calculatingAngleHeightJumper.ChangeAngleInclination = _settingsBlocksClass[i].Value;
            // }
            // else
            //     saveSettings.AngleInclination = _calculatingAngleHeightJumper.ChangeAngleInclination;
            break;
        
        case AllParametersSettings.MaximumSpeed:
            resultNumber = _flightJumper.ChangeMaximumSpeedJumper;
            // if (!_saveStart)
            // {
            //     _flightJumper.ChangeMaximumSpeedJumper = _settingsBlocksClass[i].Value;
            //     saveSettings.MaximumSpeed = _settingsBlocksClass[i].Value;
            // }
            // else
            //     saveSettings.MaximumSpeed = _flightJumper.ChangeMaximumSpeedJumper;
            break;
            
        case AllParametersSettings.Sensitivity:
            resultNumber = _clickTracking.ChangeSensitivityJumper;
            // if (!_saveStart)
            // {
            //     saveSettings.Sensitivity = _settingsBlocksClass[i].Value;
            //     _clickTracking.ChangeSensitivityJumper = _settingsBlocksClass[i].Value;
            // }else
            //     saveSettings.Sensitivity = _clickTracking.ChangeSensitivityJumper;
            break;
            
        case AllParametersSettings.DistanceXCamera:
            resultNumber = _cameraTracking.ChangeGetOffsetX;
            // if (!_saveStart)
            // {
            //     saveSettings.DistanceXCamera = _settingsBlocksClass[i].Value;
            //     _cameraTracking.ChangeGetOffsetX = _settingsBlocksClass[i].Value;
            // }else
            //     saveSettings.DistanceXCamera = _cameraTracking.ChangeGetOffsetX;
            break;
            
        case AllParametersSettings.DistanceYCamera:
            resultNumber = _cameraTracking.ChangeGetOffsetY;
            // if (!_saveStart)
            // {
            //     saveSettings.DistanceYCamera = _settingsBlocksClass[i].Value;
            //     _cameraTracking.ChangeGetOffsetY = _settingsBlocksClass[i].Value;
            // }else
            //     saveSettings.DistanceYCamera = _cameraTracking.ChangeGetOffsetY;
            break;
            
        case AllParametersSettings.DistanceZCamera:
            resultNumber = _cameraTracking.ChangeGetOffsetZ;
            // if (!_saveStart)
            // {
            //     saveSettings.DistanceZCamera = _settingsBlocksClass[i].Value;
            //     _cameraTracking.ChangeGetOffsetZ = _settingsBlocksClass[i].Value;
            // }
            // else
            //     saveSettings.DistanceZCamera = _cameraTracking.ChangeGetOffsetZ;
            break;
            
        case AllParametersSettings.SpeedCamera:
            resultNumber = _cameraTracking.ChangeGetSpeed;
            // if (!_saveStart)
            // {
            //     saveSettings.SpeedCamera = _settingsBlocksClass[i].Value;
            //     _cameraTracking.ChangeGetSpeed = _settingsBlocksClass[i].Value;
            // }
            // else
            //     saveSettings.SpeedCamera = _cameraTracking.ChangeGetSpeed;
            break;
            
        case AllParametersSettings.AngleRotationCameraX:
            resultNumber = _cameraTracking.ChangeAngleRotationX;
            // if (!_saveStart)
            // {
            //     saveSettings.AngleRotationCameraX = _settingsBlocksClass[i].Value;
            //     _cameraTracking.ChangeAngleRotationX = _settingsBlocksClass[i].Value;
            // }
            // else
            //     saveSettings.AngleRotationCameraX = _cameraTracking.ChangeAngleRotationX;
            break;
            
        case AllParametersSettings.AngleRotationCameraY:
            resultNumber = _cameraTracking.ChangeAngleRotationY;
            // if (!_saveStart)
            // {
            //     saveSettings.AngleRotationCameraY = _settingsBlocksClass[i].Value;
            //     _cameraTracking.ChangeAngleRotationY = _settingsBlocksClass[i].Value;
            // }
            // else
            //     saveSettings.AngleRotationCameraY = _cameraTracking.ChangeAngleRotationY;
            break;
            
        case AllParametersSettings.AngleRotationCameraZ:
            resultNumber = _cameraTracking.ChangeAngleRotationZ;
            // if (!_saveStart)
            // {
            //     saveSettings.AngleRotationCameraZ = _settingsBlocksClass[i].Value;
            //     _cameraTracking.ChangeAngleRotationZ = _settingsBlocksClass[i].Value;
            // }
            // else
            //     saveSettings.AngleRotationCameraZ = _cameraTracking.ChangeAngleRotationZ;
            break;
        }

        return resultNumber;
    }
    
    // private bool _saveStart = false;
    
    // public void SaveChanges()
    // {
    //     for (int i = 0; i < _settingsBlocksClass.Length; i++)
    //     {
    //         switch (_settingsBlocksClass[i].ParameterSettings)
    //         {
    //     case AllParametersSettings.Mass:
    //         if (!_saveStart)
    //         {
    //             saveSettings.Mass = _settingsBlocksClass[i].Value;
    //             _flightJumper.ChangeMassJumper = _settingsBlocksClass[i].Value;
    //         }
    //         else
    //             saveSettings.Mass = _flightJumper.ChangeMassJumper;
    //         break;
    //         
    //     case AllParametersSettings.AngleInclination:
    //         if (!_saveStart)
    //         {
    //             saveSettings.AngleInclination = _settingsBlocksClass[i].Value;
    //             _calculatingAngleHeightJumper.ChangeAngleInclination = _settingsBlocksClass[i].Value;
    //         }
    //         else
    //             saveSettings.AngleInclination = _calculatingAngleHeightJumper.ChangeAngleInclination;
    //         break;
    //     
    //     case AllParametersSettings.MaximumSpeed:
    //         if (!_saveStart)
    //         {
    //             _flightJumper.ChangeMaximumSpeedJumper = _settingsBlocksClass[i].Value;
    //             saveSettings.MaximumSpeed = _settingsBlocksClass[i].Value;
    //         }
    //         else
    //             saveSettings.MaximumSpeed = _flightJumper.ChangeMaximumSpeedJumper;
    //         break;
    //         
    //     case AllParametersSettings.Sensitivity:
    //         if (!_saveStart)
    //         {
    //             saveSettings.Sensitivity = _settingsBlocksClass[i].Value;
    //             _clickTracking.ChangeSensitivityJumper = _settingsBlocksClass[i].Value;
    //         }else
    //             saveSettings.Sensitivity = _clickTracking.ChangeSensitivityJumper;
    //         break;
    //         
    //     case AllParametersSettings.DistanceXCamera:
    //         if (!_saveStart)
    //         {
    //             saveSettings.DistanceXCamera = _settingsBlocksClass[i].Value;
    //             _cameraTracking.ChangeGetOffsetX = _settingsBlocksClass[i].Value;
    //         }else
    //             saveSettings.DistanceXCamera = _cameraTracking.ChangeGetOffsetX;
    //         break;
    //         
    //     case AllParametersSettings.DistanceYCamera:
    //         if (!_saveStart)
    //         {
    //             saveSettings.DistanceYCamera = _settingsBlocksClass[i].Value;
    //             _cameraTracking.ChangeGetOffsetY = _settingsBlocksClass[i].Value;
    //         }else
    //             saveSettings.DistanceYCamera = _cameraTracking.ChangeGetOffsetY;
    //         break;
    //         
    //     case AllParametersSettings.DistanceZCamera:
    //         if (!_saveStart)
    //         {
    //             saveSettings.DistanceZCamera = _settingsBlocksClass[i].Value;
    //             _cameraTracking.ChangeGetOffsetZ = _settingsBlocksClass[i].Value;
    //         }
    //         else
    //             saveSettings.DistanceZCamera = _cameraTracking.ChangeGetOffsetZ;
    //         break;
    //         
    //     case AllParametersSettings.SpeedCamera:
    //         if (!_saveStart)
    //         {
    //             saveSettings.SpeedCamera = _settingsBlocksClass[i].Value;
    //             _cameraTracking.ChangeGetSpeed = _settingsBlocksClass[i].Value;
    //         }
    //         else
    //             saveSettings.SpeedCamera = _cameraTracking.ChangeGetSpeed;
    //         break;
    //         
    //     case AllParametersSettings.AngleRotationCameraX:
    //         if (!_saveStart)
    //         {
    //             saveSettings.AngleRotationCameraX = _settingsBlocksClass[i].Value;
    //             _cameraTracking.ChangeAngleRotationX = _settingsBlocksClass[i].Value;
    //         }
    //         else
    //             saveSettings.AngleRotationCameraX = _cameraTracking.ChangeAngleRotationX;
    //         break;
    //         
    //     case AllParametersSettings.AngleRotationCameraY:
    //         if (!_saveStart)
    //         {
    //             saveSettings.AngleRotationCameraY = _settingsBlocksClass[i].Value;
    //             _cameraTracking.ChangeAngleRotationY = _settingsBlocksClass[i].Value;
    //         }
    //         else
    //             saveSettings.AngleRotationCameraY = _cameraTracking.ChangeAngleRotationY;
    //         break;
    //         
    //     case AllParametersSettings.AngleRotationCameraZ:
    //         if (!_saveStart)
    //         {
    //             saveSettings.AngleRotationCameraZ = _settingsBlocksClass[i].Value;
    //             _cameraTracking.ChangeAngleRotationZ = _settingsBlocksClass[i].Value;
    //         }
    //         else
    //             saveSettings.AngleRotationCameraZ = _cameraTracking.ChangeAngleRotationZ;
    //         break;
    // }
    //     }
    //     //SaveData(saveSettings);
    // }

    // private float GetValue(AllParametersSettings parameterSettings)
    // {
    //     float valueResult = .0f;
    //     
    //     switch (parameterSettings)
    //     {
    //         case AllParametersSettings.Mass:
    //             valueResult = PlayerPrefs.GetString("Mass");
    //             break;
    //                 
    //         case AllParametersSettings.AngleInclination:
    //             valueResult = saveSettings.AngleInclination;
    //             break;
    //             
    //         case AllParametersSettings.MaximumSpeed:
    //             valueResult = saveSettings.MaximumSpeed;
    //             break;
    //                 
    //         case AllParametersSettings.Sensitivity:
    //             valueResult = saveSettings.Sensitivity;
    //             break;
    //                 
    //         case AllParametersSettings.DistanceXCamera:
    //             valueResult = saveSettings.DistanceXCamera;
    //             break;
    //                 
    //         case AllParametersSettings.DistanceYCamera:
    //             valueResult = saveSettings.DistanceYCamera;
    //             break;
    //                 
    //         case AllParametersSettings.DistanceZCamera:
    //             valueResult = saveSettings.DistanceZCamera;
    //             break;
    //                 
    //         case AllParametersSettings.SpeedCamera:
    //             valueResult = saveSettings.SpeedCamera;
    //             break;
    //                 
    //         case AllParametersSettings.AngleRotationCameraX:
    //             valueResult = saveSettings.AngleRotationCameraX;
    //             break;
    //                 
    //         case AllParametersSettings.AngleRotationCameraY:
    //             valueResult = saveSettings.AngleRotationCameraY;
    //             break;
    //                 
    //         case AllParametersSettings.AngleRotationCameraZ:
    //             valueResult = saveSettings.AngleRotationCameraZ;
    //             break;
    //     }
    //
    //     return valueResult;
    // }
    //
    private void ChangeParameters(bool getParameters=false)
    {
        // for (int i = 0; i < _allSliders.Length; i++)
        // {
        //     switch (_allSliders[i].ParameterSettings)
        //     {
        //         case AllParametersSettings.Mass:
        //             if (!getParameters)
        //             {
        //                 _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_flightJumper.ChangeMassJumper}";
        //                 _flightJumper.ChangeMassJumper = _allSliders[i].SliderUI.value;
        //                 _saveSettings.Mass = _allSliders[i].SliderUI.value;;
        //             }
        //             else
        //             {
        //                 _allSliders[i].SliderUI.value = _saveSettings.Mass;
        //             }
        //             break;
        //         
        //         case AllParametersSettings.AngleInclination:
        //             if (!getParameters)
        //             {
        //                 _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_calculatingAngleHeightJumper.ChangeAngleInclination}";
        //                 _calculatingAngleHeightJumper.ChangeAngleInclination = _allSliders[i].SliderUI.value;
        //                 _saveSettings.AngleInclination = _allSliders[i].SliderUI.value;
        //             }
        //             else
        //             {
        //                 _allSliders[i].SliderUI.value = _saveSettings.AngleInclination;
        //             }
        //             break;
        //         
        //         case  AllParametersSettings.Sensitivity:
        //             if (!getParameters)
        //             {
        //                 _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_clickTracking.ChangeSensitivityJumper}";
        //                 _clickTracking.ChangeSensitivityJumper = _allSliders[i].SliderUI.value;
        //                 _saveSettings.Sensitivity = _allSliders[i].SliderUI.value;
        //             }
        //             else
        //             {
        //                 _allSliders[i].SliderUI.value = _saveSettings.Sensitivity;
        //             }
        //             break;
        //         
        //         case AllParametersSettings.MaximumSpeed:
        //             if (!getParameters)
        //             {
        //                 _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_flightJumper.ChangeMaximumSpeedJumper}";
        //                 _flightJumper.ChangeMaximumSpeedJumper = _allSliders[i].SliderUI.value;
        //                 _saveSettings.MaximumSpeed = _allSliders[i].SliderUI.value;
        //             }
        //             else
        //             {
        //                 _allSliders[i].SliderUI.value = _saveSettings.MaximumSpeed;
        //             }
        //             break;
        //         
        //         case AllParametersSettings.DistanceXCamera:
        //             if (!getParameters)
        //             {
        //                 _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_cameraTracking.ChangeGetOffsetX}";
        //                 _cameraTracking.ChangeGetOffsetX = _allSliders[i].SliderUI.value;
        //                 _saveSettings.DistanceXCamera = _allSliders[i].SliderUI.value;
        //             }
        //             else
        //             {
        //                 _allSliders[i].SliderUI.value = _saveSettings.DistanceXCamera;
        //             }
        //             break;
        //         
        //         case AllParametersSettings.DistanceYCamera:
        //             if (!getParameters)
        //             {
        //                 _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_cameraTracking.ChangeGetOffsetY}";
        //                 _cameraTracking.ChangeGetOffsetY = _allSliders[i].SliderUI.value;
        //                 _saveSettings.DistanceYCamera = _allSliders[i].SliderUI.value;
        //             }
        //             else
        //             {
        //                 _allSliders[i].SliderUI.value = _saveSettings.DistanceYCamera;
        //             }
        //             break;
        //         
        //         case AllParametersSettings.DistanceZCamera:
        //             if (!getParameters)
        //             {
        //                 _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_cameraTracking.ChangeGetOffsetZ}";
        //                 _cameraTracking.ChangeGetOffsetZ = _allSliders[i].SliderUI.value;
        //                 _saveSettings.DistanceZCamera = _allSliders[i].SliderUI.value;
        //             }
        //             else
        //             {
        //                 _allSliders[i].SliderUI.value = _saveSettings.DistanceZCamera;
        //             }
        //             break;
        //         
        //         case AllParametersSettings.AngleRotationCameraY:
        //             if (!getParameters)
        //             {
        //                 _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_cameraTracking.ChangeAngleRotationY}";
        //                 _cameraTracking.ChangeAngleRotationY = _allSliders[i].SliderUI.value;
        //                 _saveSettings.AngleRotationCameraY = _allSliders[i].SliderUI.value;
        //             }
        //             else
        //             {
        //                 _allSliders[i].SliderUI.value = _saveSettings.AngleRotationCameraY;
        //             }
        //             break;
        //         
        //         case AllParametersSettings.AngleRotationCameraX:
        //             if (!getParameters)
        //             {
        //                 _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_cameraTracking.ChangeAngleRotationX}";
        //                 _cameraTracking.ChangeAngleRotationX = _allSliders[i].SliderUI.value;
        //                 _saveSettings.AngleRotationCameraX = _allSliders[i].SliderUI.value;
        //             }
        //             else
        //             {
        //                 _allSliders[i].SliderUI.value = _saveSettings.AngleRotationCameraX;
        //             }
        //             break;
        //         
        //         case AllParametersSettings.SpeedCamera:
        //             if (!getParameters)
        //             {
        //                 _allSliders[i].TextUI.text = $"{_allSliders[i].NameSlider}: {_cameraTracking.ChangeGetSpeed}";
        //                 _cameraTracking.ChangeGetSpeed = _allSliders[i].SliderUI.value;
        //                 _saveSettings.SpeedCamera = _allSliders[i].SliderUI.value;
        //             }
        //             else
        //             {
        //                 _allSliders[i].SliderUI.value = _saveSettings.SpeedCamera;
        //             }
        //             break;
        //     }
        //     
        // }
        //
        // SaveData();
    }

    // private SaveSettings GetInfoSettings()
    // {
    //     var jsonSettings = Resources.Load<TextAsset>("Settings");
    //     _saveSettings = JsonUtility.FromJson<SaveSettings>(jsonSettings.ToString());
    //     return _saveSettings;
    // }
    
    // Сохранение в Json Файл
    // private void SaveData(SaveSettings saveSettings)
    // {
    //     print("Save Data");
    //     string json = JsonUtility.ToJson(saveSettings);
    //     StreamWriter sw = new StreamWriter("C:/Users/vnich/YandexDisk/Unity Projects/Jumper-Unity-Game/Jumper/Assets/Resources/Settings.json");
    //     sw.WriteLine(json);
    //     sw.Close();
    // }
    
    
    
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
