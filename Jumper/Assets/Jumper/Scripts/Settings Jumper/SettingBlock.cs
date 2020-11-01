using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingBlock : MonoBehaviour
{
    [Header("Текст")] public Text NameSetting;

    [Header("Ввод значения")]
    public InputField InputValue;

    [Header("Слайдер")] public Slider SliderSetting;

    public void FillingText(string text)
    {
        NameSetting.text = text;
    }

    public void FillingInputAndSlider(float value)
    {
        InputValue.text = value.ToString();
        SliderSetting.value = value;
    }

    public void ChangeValueSlider()
    {
        string value = SliderSetting.value.ToString();
        InputValue.text = value;
    }
    
    public void ChangeValueInput()
    {
        float value = float.Parse(InputValue.text);
        SliderSetting.value = value;
    }
    
    
}
