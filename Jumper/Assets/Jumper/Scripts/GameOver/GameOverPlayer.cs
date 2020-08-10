﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPlayer : MonoBehaviour
{
    [SerializeField] [Header("Скрипт, который запускает анимацию поражения джампера")]
    private AnimationGameOverJumper _animationGameOverJumper = null;
    
    [SerializeField] [Header("Скрипт, который управляет панелью при поражении джампера")] 
    private GameOverPanel _gameOverPanel = null;
    
    // Данный метод включает поражения игрока
    public void GameOverPlayerMethod()
    {
        print("GameOver Player");
        ClickTracking.GameOverPlayer = true; 
        _animationGameOverJumper.StartAnimationGameOver();
        _gameOverPanel.gameObject.SetActive(true);
        StartCoroutine(_gameOverPanel.AnimationPanel());
    }
}