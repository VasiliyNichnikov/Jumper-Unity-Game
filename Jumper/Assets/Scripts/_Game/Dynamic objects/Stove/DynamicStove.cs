using System.Collections;
using UnityEngine;

public class DynamicStove : MonoBehaviour
{
    // Скрипт, который отвечает за поражение игрока
    private GameOverPlayer _gameOverPlayer = null;

    // Компонент, который за акцивацию динамического блока
    private bool _activeBlock = false;
    
    // Скрипт, который отвечает за информацию о блоке
    private ObjectInfo _objectInfo;
    
    private SpriteRenderer[] _spriteRenderersLights;

    void Start()
    {
        _gameOverPlayer = FindObjectOfType<GameOverPlayer>();
        _objectInfo = GetComponentInParent<ObjectInfo>();
        _spriteRenderersLights = GetComponentsInChildren<SpriteRenderer>();
        if(!_objectInfo.StartObject)
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
        if (other.CompareTag("Bottom Part") && !ClickTracking.GameOverPlayer && _activeBlock && !_objectInfo.StartObject)
            _gameOverPlayer.GameOverPlayerMethod();
    }
}