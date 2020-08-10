using UnityEngine;

/// <summary>
/// Данный скрипт задает парамерты объектам на карте (Нужно доработать, не подходит для генерации (Нужно доработать))
/// </summary>
public class ManagingMapObjects : MonoBehaviour
{
    [SerializeField] [Header("Массив с объектами на карте")]
    private GameObject[] _objectsModels = null;

    //[SerializeField] [Header("Скрипт, который дает импульс двум частям джампера")]
    //private AnimationGameOverJumper _animationGameOverJumper = null;

    //[SerializeField] [Header("Скрипт, который запускает анимацию панели с проигрышом")]
    //private GameOverPanel _gameOverPanel = null;
    
    [SerializeField] [Header("Скрипт, который отвечает за проигрывание игрока")]
    private GameOverPlayer _gameOverPlayer = null;
    
    private void Awake()
    {
        for (int i = 0; i < _objectsModels.Length; i++)
        {
            if(_objectsModels[i].GetComponent<CheckCollider>() == null)
                _objectsModels[i].AddComponent<CheckCollider>();
            CheckCollider checkCollider = _objectsModels[i].GetComponent<CheckCollider>();
            //checkCollider.AnimationGameOverJumper = _animationGameOverJumper;
            //checkCollider.GameOverPanel = _gameOverPanel;
            checkCollider.GameOverPlayer = _gameOverPlayer;
        }
    }
}
