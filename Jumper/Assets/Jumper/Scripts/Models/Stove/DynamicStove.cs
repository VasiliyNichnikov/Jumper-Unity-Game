using System.Collections;
using UnityEngine;

public class DynamicStove : MonoBehaviour
{
    // Скрипт, который отвечает за поражение игрока
    private GameOverPlayer _gameOverPlayer = null;

    // Компонент, который за акцивацию динамического блока
    private bool _activeBlock = false;

    private SpriteRenderer[] _spriteRenderersLights;

    void Start()
    {
        _gameOverPlayer = FindObjectOfType<GameOverPlayer>();
        _spriteRenderersLights = GetComponentsInChildren<SpriteRenderer>();
        StartCoroutine(StartDynamicBlock());
    }

    private void ChangeMeshRenderedEnabled(bool condition)
    {
        for (int i = 0; i < _spriteRenderersLights.Length; i++)
        {
            _spriteRenderersLights[i].enabled = condition;
        }
    }

    // Запуск динамического блока
    private IEnumerator StartDynamicBlock()
    {
        while (true)
        {
            _activeBlock = false;
            ChangeMeshRenderedEnabled(false);
            yield return new WaitForSeconds(3);
            _activeBlock = true;
            ChangeMeshRenderedEnabled(true);
            yield return new WaitForSeconds(3);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Bottom Part") && !ClickTracking.GameOverPlayer && _activeBlock)
            _gameOverPlayer.GameOverPlayerMethod();
    }
}