using System;
using UnityEngine;

/// <summary>
/// Данный скрипт создает игрока с случайным скином при старте (Случайный скин для теста)
/// </summary>

public class CreateJumperStart : MonoBehaviour
{
    [Serializable]
    public class Jumper
    {
        [Header("Название джампера")] public string NameJumper = "None";

        [Header("Префаб верхней части джампера")]
        public GameObject PrefabUpperPartJumper = null;

        [Header("Позиция джампера по оси Y")]
        public float PositionUpperPartJumperAxesY = .0f;

        [Header("Позиция верхней части джампера во время полета")]
        public float PositionUpperPartJumperAxesYFlight = .0f;
        
        [Header("Префаб нижний части джампера")]
        public GameObject PrefabBottomPartJumper = null;
        
        [Header("Минимальное сжатие джампера")]
        public float MinimumHeightUpperPart = .0f;
    }

    [Header("Класс игрока")] [SerializeField]
    private GameObject _playerObject = null;

    [Header("Префаб пустышки игрока")] [SerializeField]
    private GameObject _prefabEmptyVector3HeightAngleObject = null;
    
    [Header("Массив с джамперами")] [SerializeField]
    private Jumper[] _jumpers = null;

    private void Awake()
    {
        CreateNewJumper();
    }
    
    // Данный метод создает новый джампер
    private void CreateNewJumper()
    {
        // Выбор джампера из класса
        Jumper jumper = _jumpers[UnityEngine.Random.Range(0, _jumpers.Length)];

        // Получение скрипта CalculatingAngleHeightJumper
        CalculatingAngleHeightJumper calculatingAngleHeightJumper =
            _playerObject.GetComponent<CalculatingAngleHeightJumper>();
        
        // Получение скрипта FlightJumper
        FlightJumper flightJumper = _playerObject.GetComponent<FlightJumper>();
        
        // Получение скрипта AnimationGameOverJumper
        AnimationGameOverJumper animationGameOverJumper = _playerObject.GetComponent<AnimationGameOverJumper>();
        
        // Создание верхней и нижний части джампера
        GameObject newUpperPartJumper = Instantiate(jumper.PrefabUpperPartJumper, _playerObject.transform, false);
        GameObject newBottomPartJumper = Instantiate(jumper.PrefabBottomPartJumper, _playerObject.transform, false);
        
        // Передача параметров в CalculatingAngleHeightJumper
        calculatingAngleHeightJumper.ChangeMinimumHeightUpperPart = jumper.MinimumHeightUpperPart;
        calculatingAngleHeightJumper.ChoiseUpperPartJumper = newUpperPartJumper;
        calculatingAngleHeightJumper.PositionUpperPartJumperAxesYFlight = jumper.PositionUpperPartJumperAxesYFlight;
        
        // Переименование частей и выбор локальной позиции верхней части
        newUpperPartJumper.transform.localPosition = new Vector3(0, jumper.PositionUpperPartJumperAxesY, 0);
        newUpperPartJumper.name = "Jumper the upper part";
        newBottomPartJumper.name = "Jumper the bottom part";

        // Создание пустышки на верхнюю часть джампера
        Transform createVector3HeightAngle = Instantiate(_prefabEmptyVector3HeightAngleObject, newUpperPartJumper.transform, false).transform;
        createVector3HeightAngle.position = new Vector3(createVector3HeightAngle.position.x, 6.9f,
            createVector3HeightAngle.position.z);
        
        // Передача параметров в FlightJumper
        flightJumper.ChoiceVector3TransformHeightAngle = createVector3HeightAngle;
        
        // Передача параметров в AnimationGameOverJumper
        animationGameOverJumper.ChangeUpperAndBottomPartsJumper(newUpperPartJumper, newBottomPartJumper);
    }
    
}
