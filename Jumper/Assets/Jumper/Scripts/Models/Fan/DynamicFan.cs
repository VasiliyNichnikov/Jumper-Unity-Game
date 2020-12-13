using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicFan : MonoBehaviour
{
    [Header("Скрипт, который отвечает за вращение вентилятора")]
    public RotationFan _rotationFan;
    
    // Скрипт, который отвечает за поражение игрока
    private GameOverPlayer _gameOverPlayer = null;

    private FlightJumper _flightJumper = null;
    
    // Максимальная высота
    //public float MaximumHeight;

    // Скрипт, отвечающий за полет игрока
    //private FlightJumper _flightJumper;
    
    // Триггер блока
    //private BoxCollider _boxCollider;
    
    // Компонент, который отвечает за акцивацию динамического блока
    private bool _activeBlock = false;

    [Header("Текстура ветра")] public SpriteRenderer SpriteRendererWind;
    
    
    private void Start()
    {
        _gameOverPlayer = FindObjectOfType<GameOverPlayer>();
        _flightJumper = FindObjectOfType<FlightJumper>();
        StartCoroutine(StartDynamicBlock());
    }
    
    // Включение/Выключение ветра
    private void OnOffWind(bool condition)
    {
        SpriteRendererWind.enabled = condition;
    }
    
    // Запуск динамического блока
    private IEnumerator StartDynamicBlock()
    {
        while (true)
        {
            OnOffWind(false);
            _rotationFan.OnAndOffFan = false;
            _activeBlock = false;
            //_boxCollider.enabled = false;
            yield return new WaitForSeconds(3);
            OnOffWind(true);
            _rotationFan.OnAndOffFan = true;
            _activeBlock = true;
            //_boxCollider.enabled = true;
            yield return new WaitForSeconds(3);
        }
    }

    private bool _jump = false;
    private void OnTriggerStay(Collider other)
    {
        // Forse - (-4.5, 5.7, 0.0)
        if (other.CompareTag("Bottom Part") && !ClickTracking.GameOverPlayer && _activeBlock && !_jump)
        {
            print("Jump Trigger Stay");
            //ClickTracking.DynamicObjectActive = true;
            _flightJumper.AddSpeedJumper(new Vector3(-6f, 5.7f, 0.0f));
            _jump = true;
        }

        //_gameOverPlayer.GameOverPlayerMethod();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bottom Part") && !ClickTracking.GameOverPlayer)
        {
            _jump = false;
        }
    }
}