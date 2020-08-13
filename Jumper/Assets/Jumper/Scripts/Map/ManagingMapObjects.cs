using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данный скрипт задает парамерты объектам на карте (Нужно доработать, не подходит для генерации (Нужно доработать))
/// </summary>
public class ManagingMapObjects : MonoBehaviour
{
    [SerializeField] [Header("Скрипт, который отвечает за проигрывание игрока")]
    private GameOverPlayer _gameOverPlayer = null;


    public void AddAdditionalParametersCheckCollider(CheckCollider checkCollider)
    {
        checkCollider.GameOverPlayer = _gameOverPlayer;
    }
    
}
