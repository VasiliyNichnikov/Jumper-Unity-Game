using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GenerationMap : MonoBehaviour
{
    [Serializable]
    public class Object
    {
        [Header("Название объекта")] public string Name = "None";
        [Header("Высота объекта")] public float Height = 0;
        [Header("Ширина объекта")] public float Width = 0;
        [Header("Объект")] public GameObject PrefabObject = null;
    }

    [Header("Скрипт, который находится на игроке")] [SerializeField]
    private InspectionGround _inspectionGround = null;
    
    [Header("Объекты, которые создаются на сцены")] [SerializeField]
    private Object[] _objects = null;

    [Header("Расстояние между объектами")] public float Offset = 0.0f;
    
    // Позиция по X
    private float _positionX = 0.0f;

    private Transform _lastObject = null;
    
    private void Start()
    {
        //_positionX = 2;
        for (int i = 0; i < _objects.Length; i++)
        {
            CreateObject(_objects[i]);
        }
    }

    private void CreateObject(Object classObject)
    {
        GameObject prefab = GetObject();
       // Vector2 sizeObject = new Vector2(classObject.Width, classObject.Width);
        GameObject newObject = Instantiate(prefab, transform, false);
        print(newObject.transform.localScale.x);
        if (_lastObject != null)
            _positionX += (_lastObject.transform.localScale.x / 2) + (newObject.transform.localScale.x / 2) + Offset;
        else
            _positionX += 5;
        newObject.transform.position = new Vector3(_positionX, 1, transform.position.z);
        _lastObject = newObject.transform; 
        
        //print(newObject.GetComponent<MeshFilter>().sharedMesh.bounds);
        //float dis = Vector3.Distance(newObject.GetComponent<MeshFilter>().sharedMesh.bounds.center,
        //    newObject.GetComponent<MeshFilter>().sharedMesh.bounds.extents);
        //print(dis);
        
        //print(newObject.GetComponent<MeshFilter>().sharedMesh);
        //newObject.GetComponentInChildren<BoxCollider>().gameObject.name =
         //   newObject.GetComponentInChildren<BoxCollider>().gameObject.transform.lossyScale.x.ToString();
    }

    // Возвращает объект, который нужно создать
    private GameObject GetObject()
    {
        if (_inspectionGround.GetHeight() <= 3)
        {
            int numberRandom = UnityEngine.Random.Range(0, _objects.Length);
            while (true)
            {
                if (_objects[numberRandom].Height <= 3)
                {
                    return _objects[numberRandom].PrefabObject;
                }
                numberRandom = UnityEngine.Random.Range(0, _objects.Length);
            }
            // for (int i = 0; i < _objects.Length; i++)
            // {
            //     if (_objects[i].Height <= 3)
            //         return _objects[i].PrefabObject;
            // }
        }

        return null;
    }
    
    
}
