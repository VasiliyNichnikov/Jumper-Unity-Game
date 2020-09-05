using System.Collections;
using UnityEngine;

/// <summary>
/// Данный скрипт считает с какой скоростью и под каким углом кинуть Джампер.
/// Все это зависит от того насколько сильно пользователь потянет палец.
/// </summary>
public class CalculatingAngleHeightJumper : MonoBehaviour
{
    ///<summary>
    /// Переменные, которые отображаются в инспекторе 
    ///</summary>

    [Header("Минимальная высота до которой может сжиматься джампер")] [SerializeField] [Range(0, 3)]
    private float _minimumHeightUpperPart;

    [Header("Максимальный угол наклона джампера (По модулю)")] [SerializeField] [Range(0, 180)]
    private float _maximumAngleInclination;

    [Header("Скорость с которой джампер разжимается")] [SerializeField] [Range(0, 100)]
    private float _speedDecompressedJumper;

    /// <summary>
    /// Переменные, которые скрыты в инспекторе 
    /// </summary>

    // Позиция верхней части джампера во время полета
    [HideInInspector] public float PositionUpperPartJumperAxesYFlight;
    
    // Верхняя часть джампера
    private GameObject _upperPartJumper;

    // Максимальная высота верхней части джампера (Задается при старте)
    private float _maximumHeightJumper;
    
    private float _percentHeightJumper;
    private float _percentAngleJumper;
    
    // Максимальный угол по оси Z на данный момент 
    private float _maximumAngleInclinationAxesZ;
    
    // Минимальный угол по оси Z на данный момент
    private float _minimumAngleInclinationAxesZ;

    public GameObject ChoiseUpperPartJumper
    {
        set { _upperPartJumper = value; }
    }
    
    public float GerPercentHeightJumper
    {
        get { return _percentHeightJumper; }
    }

    public float GetPercentAngleJumper
    {
        get { return _percentAngleJumper; }
    }

    public float ChangeMinimumHeightUpperPart
    {
        set
        {
            if (value > 0) _minimumHeightUpperPart = value;
        }
    }
    
    // Для настроек джампера(Начало)
    public float ChangeAngleInclination
    {
        set
        {
            if (value <= 90) _maximumAngleInclination = value;
        }

        get
        {
            return _maximumAngleInclination;
        }
    }
    // Для настроек джампера(Конец)

    // transform верхней части джампера
    private Transform _transformUpperPartJumper;
    private Transform _thisTransform;
    
    private void Start()
    {
        // Инициализация дополнительных параметров
        _transformUpperPartJumper = _upperPartJumper.transform;
        _maximumHeightJumper = _transformUpperPartJumper.localPosition.y;
        _thisTransform = GetComponent<Transform>();

        _maximumAngleInclinationAxesZ = _maximumAngleInclination;
        _minimumAngleInclinationAxesZ = -_maximumAngleInclination;
    }

    // Данный метод меняет высоту и угол наклона джампера, в зависимости от нахождения пальца
    public void ChangeHeightAngleInclinationJumper(Vector2 nowPosition, Vector2 startPosition, float sensitivityJumper)
    {
        var heightJumperY = GetHeightUpperPartJumper(nowPosition.y, startPosition.y, sensitivityJumper);
        var angleJumperZ = GetAngleInclinationJumper(nowPosition.x, startPosition.x, sensitivityJumper);
        _transformUpperPartJumper.localPosition = new Vector3(0, heightJumperY, 0);
        _thisTransform.localRotation = Quaternion.Euler(0, 0, angleJumperZ);
    }
    
    // Данный метод возвращает высоту до которой нужно опустить верхнюю часть джампера
    private float GetHeightUpperPartJumper(float nowPositionY, float startPositionY, float sensitivityJumper)
    {
        var differenceDistance = Mathf.Abs(startPositionY - nowPositionY);
        var percentageScreenHeight =
            ConversionValuesPercent(differenceDistance, sensitivityJumper);
        _percentHeightJumper = Mathf.Clamp(percentageScreenHeight, 0, 100);
        var jumperHeightDifference =
            DifferenceLengthUpperPartJumper(_percentHeightJumper, _minimumHeightUpperPart, _maximumHeightJumper);
        return _maximumHeightJumper - jumperHeightDifference;
    }
    
    // Данный метод меняем угол джампера в зависимости от расположения пальца 
    private float GetAngleInclinationJumper(float nowPositionX, float startPositionX, float sensitivityJumper)
    {
        var differenceDistance = startPositionX - nowPositionX;
        var percentageScreenWidth = -ConversionValuesPercent(differenceDistance, sensitivityJumper);
        _percentAngleJumper = Mathf.Clamp(Mathf.Abs(percentageScreenWidth), 0, 100);    
        var angleInclination = InterestValue(percentageScreenWidth, _minimumAngleInclinationAxesZ, _maximumAngleInclinationAxesZ);
        return angleInclination;
    }
    
    // Возвращает высоту верхней части джампера к исходному положению
    public IEnumerator ReturnUpperPartJumper(bool returnHeightJumper=false)
    {
        Vector3 heightJumper = new Vector3(_transformUpperPartJumper.localPosition.x, PositionUpperPartJumperAxesYFlight, 
            _transformUpperPartJumper.localPosition.z);
        if (returnHeightJumper)
        {
            heightJumper = new Vector3(_transformUpperPartJumper.localPosition.x, _maximumHeightJumper - 0, 
                _transformUpperPartJumper.localPosition.z);
        }
        while (_transformUpperPartJumper.localPosition.y != heightJumper.y && !ClickTracking.FingerInputScreen)
        {
            _transformUpperPartJumper.localPosition = Vector3.MoveTowards(_transformUpperPartJumper.localPosition, heightJumper,
                _speedDecompressedJumper * Time.deltaTime);
            yield return null;
        }
    }


    // Метод блокирует/разблокирует угол наклона джампера
    public void LockingUnlockJumperAngle(bool lockUnlock = false)
    {
        if (!lockUnlock)
        {
            if (_thisTransform.localRotation.eulerAngles.z > 180)
                _minimumAngleInclinationAxesZ = -(360 - _thisTransform.localRotation.eulerAngles.z);
            else
                _maximumAngleInclinationAxesZ = _thisTransform.localRotation.eulerAngles.z;
        }
        else
        {
            _maximumAngleInclinationAxesZ = _maximumAngleInclination;
            _minimumAngleInclinationAxesZ = -_maximumAngleInclination;
        }
    }
    
    // Данный метод вычисляет проценты от 0 до 100
    private float ConversionValuesPercent(float nowValue, float maximum, bool onClamp=false)
    {
        if(onClamp)
            return Mathf.Clamp(100 * nowValue / maximum, 0, 100);
        return 100 * nowValue / maximum;
    }

    // Данный метод преобразует проценты в значение
    private float InterestValue(float percentNow, float minimum, float maximum)
    {
        return Mathf.Clamp(_maximumAngleInclination * percentNow / 100, minimum, maximum);
    }

    // Разница высоты верхний части джампера
    private float DifferenceLengthUpperPartJumper(float percentNow, float minimum, float maximum)
    {
        return Mathf.Clamp((maximum - minimum) * percentNow / 100, 0, maximum - minimum);
    }
    
}
