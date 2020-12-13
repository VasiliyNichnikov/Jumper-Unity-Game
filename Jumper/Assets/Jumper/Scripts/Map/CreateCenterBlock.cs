using UnityEngine;
using System.Collections;

public class CreateCenterBlock : MonoBehaviour
{
    [Header("Позиция по центру")] public Vector3 PositionCenter;

    [Header("Массив с объектами, которые будут по центру")]
    public GameObject[] ArrayObjectsCenter;

    // Проверка, можем/не можем создать объект по центру
    private bool CheckCreateObjectCenter()
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber >= 50)
            return true;
        return false;
    }
    
    private void Start()
    {
        if (CheckCreateObjectCenter())
        {
            GameObject newObject = Instantiate(ArrayObjectsCenter[Random.Range(0, ArrayObjectsCenter.Length)],
                transform, false);
            newObject.transform.localPosition = PositionCenter;
        }
    }
}
