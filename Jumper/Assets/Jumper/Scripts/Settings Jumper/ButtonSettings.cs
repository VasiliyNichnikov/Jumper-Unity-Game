using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSettings : MonoBehaviour
{
    [Header("Панель с настройками джампера")] [SerializeField]
    private GameObject _panelSettings = null;

    private void Start()
    {
        _panelSettings.SetActive(false);
    }

    public void InputButton()
    {
        _panelSettings.SetActive(!_panelSettings.activeSelf);
    }
    
}
