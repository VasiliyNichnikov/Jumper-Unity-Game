using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLazer : MonoBehaviour
{
        // Mesh Renderer объекта
        private MeshRenderer _meshRenderer;
        
        // Скрипт, который отвечает за поражение игрока
        private GameOverPlayer _gameOverPlayer = null;
    
        // Компонент, который за акцивацию динамического блока
        private bool _activeBlock = false;
    
        void Start()
        {
            _gameOverPlayer = FindObjectOfType<GameOverPlayer>();
            _meshRenderer = GetComponent<MeshRenderer>();
            StartCoroutine(StartDynamicBlock());
        }
    
        // Запуск динамического блока
        private IEnumerator StartDynamicBlock()
        {
            while (true)
            {
                _activeBlock = false;
                _meshRenderer.enabled = false;
                yield return new WaitForSeconds(4);
                _activeBlock = true;
                _meshRenderer.enabled = true;
                yield return new WaitForSeconds(2);
            }
        }
    
        private void OnTriggerStay(Collider other)
        {
            if ((other.CompareTag("Top Part") || other.CompareTag("Bottom Part")) && !ClickTracking.GameOverPlayer && _activeBlock)
                _gameOverPlayer.GameOverPlayerMethod();
        }
}
