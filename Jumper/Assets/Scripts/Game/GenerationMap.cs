using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public class GenerationMap : MonoBehaviour
{
    [Serializable]
    public class Object
    {
        [Header("Название объекта")] public string Name = "None";
        //[Header("Высота объекта")] public float Height = 0;
        //[Header("Ширина объекта")] public float Width = 0;
        [Header("Объект")] public GameObject PrefabObject = null;
    }

    [Header("Скрипт, который находится на игроке")] [SerializeField]
    private InspectionGround _inspectionGround = null;
    
    [Header("Объекты, которые создаются на сцены")] [SerializeField]
    private Object[] _objects = null;

    [Header("Игрок")] [SerializeField] private GameObject _player = null;
    
    [Header("Расстояние между объектами")] public float Offset = 0.0f;

    // Массив, который хранит полседнии 4 объекта
    private GameObject[] _arrayCreatedObjects = new GameObject[]{null, null, null, null};
    // Номер ячейки, куда нужно положить новый созданный объект
    
    // Позиция по X
    private float _positionX = 0.0f;

    private Transform _lastObject = null;
    
    private void Start()
    {
        //_positionX = 2;
        //CreateObject();
        for (int i = 0; i < 20; i++)
        {
            CreateObject();
        }
    }

    private void Update()
    {
        //print(GetNearestBlock().name);
    }

    public void CreateObject()
    { 
        GameObject prefab = GetObject(); 
        if (prefab != null) 
        {
           GameObject newObject = Instantiate(prefab, transform, false);
           newObject.name = prefab.name;
           if (_lastObject != null)
           {
               _positionX += (_lastObject.GetComponent<Collider>().bounds.size.x / 2) + 
                             (newObject.GetComponent<Collider>().bounds.size.x / 2) + Offset;
           }
           else
               _positionX += 6;
           newObject.transform.position = new Vector3(_positionX, newObject.transform.position.y, transform.position.z);
           _lastObject = newObject.transform; 
        }else
           Debug.LogError("Error. Not Found Object Block");
    }

    
    //Возвращает объект, который нужно создать
    private GameObject GetObject()
    {
        float distanceObjectPrefabY = 0;
        if (_lastObject != null)
        {
            float distanceY = GetMaxHeightObjectCollider(_lastObject.gameObject);
            // Нужна в случаи, если цикл не сможет завершится
            int exitNum = 0;
            while (exitNum <= 1000)
            {
                GameObject objectPrefab = _objects[UnityEngine.Random.Range(0, _objects.Length)].PrefabObject;
                distanceObjectPrefabY = GetMaxHeightObjectCollider(objectPrefab);
                float differenceHeight = Mathf.Abs(distanceObjectPrefabY - distanceY);
                exitNum += 1;
                print(distanceObjectPrefabY);
                if ((differenceHeight <= 2.5f && CheckLastObjects(objectPrefab)) || (distanceY > distanceObjectPrefabY && CheckLastObjects(objectPrefab)))
                    return objectPrefab;
            }
            // for (int i = 0; i < _objects.Length; i++)
            // {
            //     GameObject objectPrefab = _objects[i].PrefabObject;
            //     distanceObjectPrefabY = GetMaxHeightObjectCollider(objectPrefab);
            //     float differenceHeight = Mathf.Abs(distanceObjectPrefabY - distanceY);
            //     if (differenceHeight <= 3 && CheckLastObjects(objectPrefab))
            //         return objectPrefab;
            // }
        }
        else
        {
            // Нужна в случаи, если цикл не сможет завершится
            int exitNum = 0;
            while (exitNum <= 1000)
            {
                GameObject objectPrefab = _objects[UnityEngine.Random.Range(0, _objects.Length)].PrefabObject;
                distanceObjectPrefabY = GetMaxHeightObjectCollider(objectPrefab);
                exitNum += 1;
                if (distanceObjectPrefabY <= 3 && CheckLastObjects(objectPrefab))
                    return objectPrefab;
            }
            // for (int i = 0; i < _objects.Length; i++)
            // {
            //     GameObject objectPrefab = _objects[i].PrefabObject;
            //     distanceObjectPrefabY = GetMaxHeightObjectCollider(objectPrefab);
            //     print(distanceObjectPrefabY);
            //     if (distanceObjectPrefabY <= 3)
            //         return objectPrefab;
            // }
        }
        Debug.LogError("Не найдены объекты!");
        return null;
    }

    private float GetMaxHeightObjectCollider(GameObject obj)
    {
        if (obj.GetComponent<Collider>() == null)
        {
            Debug.LogError("Коллайдер не найден. Ошибка");
            return -1f;
        }

        Collider collider = obj.GetComponent<Collider>();
        var center = collider.bounds.center;
        var pointColliderMaxY =
            collider.bounds.ClosestPoint(new Vector3(center.x, Mathf.Infinity, center.z));
        float distanceObjectPrefabY = Vector3.Project(pointColliderMaxY, Vector3.up).y;
        return distanceObjectPrefabY;
    }

    private bool CheckLastObjects(GameObject obj)
    {
        _arrayCreatedObjects = GetLastChildren();
        if (_arrayCreatedObjects == null)
            return true;
        foreach (GameObject objList in _arrayCreatedObjects)
        {
            print($"ObjList: {objList.name}; Obj: {obj.name}");
            if (objList.name == obj.name)
                return false;
        }
        return true;
    }

    private GameObject[] GetLastChildren()
    {
        int numberChild = transform.childCount;
        if (numberChild > 1)
        {
            if (numberChild > 4)
                numberChild = 4;
            GameObject[] arrayResult = new GameObject[numberChild];
            for (int i = 0; i < numberChild; i++)
            {
                print(numberChild - i);
                arrayResult[i] = transform.GetChild(transform.childCount - i - 1).gameObject;
            }

            return arrayResult;
        }
        //Debug.LogError("Недостаточно элементов в массиве.");
        return null;
    }
    
    // Возвращает высоту последнего блока
    // private float GetNearestHeightBlock()
    // {
    //     //GameObject objectEnd = transform.GetChild(-1).gameObject;
    //
    //     if (_lastObject != null)
    //     {
    //         for (int i = 0; i < _objects.Length; i++)
    //         {
    //             if (_objects[i].Name == _lastObject.name)
    //                 return _objects[i].Height;
    //         }
    //     }
    //     return 1f;
    // }
    
    
}
