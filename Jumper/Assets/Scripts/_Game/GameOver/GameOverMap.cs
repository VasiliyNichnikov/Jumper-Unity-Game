using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMap : MonoBehaviour
{
    [SerializeField] [Header("Скрипт, который отвечает за проигрывание игрока")]
    private GameOverPlayer _gameOverPlayer = null;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player") && !ClickTracking.GameOverPlayer)
        {
            _gameOverPlayer.GameOverPlayerMethod();
        }
    }
}
