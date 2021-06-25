using UnityEngine;
using System.Collections;

public class CreateCenterBlock : MonoBehaviour
{
    [Header("Позиция по центру")] public Vector3 PositionCenter;
    
    [Header("Вероятность создания объекта")]
    public int ProbabilityObjectCreation;
    
    [Header("Массив с объектами, которые будут по центру")]
    public GameObject[] ArrayObjectsCenter;
    
    // Скрипт, который отвечает за информацию о блоке
    private ObjectInfo _objectInfo;
    
    // Проверка, можем/не можем создать объект по центру
    private bool CheckCreateObjectCenter()
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber <= ProbabilityObjectCreation)
            return true;
        return false;
    }
    
    private void Start()
    {
        _objectInfo = GetComponent<ObjectInfo>();
        if (CheckCreateObjectCenter() && !_objectInfo.StartObject)
        {
            GameObject newObject = Instantiate(ArrayObjectsCenter[Random.Range(0, ArrayObjectsCenter.Length)],
                transform, false);
            newObject.transform.localPosition = PositionCenter;
        }
    }
}
