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

    [Header("Игрока")] [SerializeField] private GameObject _player = null;
    
    [Header("Расстояние между объектами")] public float Offset = 0.0f;
    
    // Позиция по X
    private float _positionX = 0.0f;

    private Transform _lastObject = null;
    
    private void Start()
    {
        //_positionX = 2;
        //CreateObject();
        for (int i = 0; i < 5; i++)
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
       // Vector2 sizeObject = new Vector2(classObject.Width, classObject.Width);
       if (prefab != null)
       {
           GameObject newObject = Instantiate(prefab, transform, false);
           newObject.name = prefab.name;
           //print(newObject.transform.localScale.x);
           if (_lastObject != null)
               _positionX += (_lastObject.transform.localScale.x / 2) + (newObject.transform.localScale.x / 2) + Offset;
           else
               _positionX += 6;
           newObject.transform.position = new Vector3(_positionX, newObject.transform.position.y, transform.position.z);
           _lastObject = newObject.transform;
       }else
           Debug.LogError("Error. Not Found Object Block");
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
        float heightBlock = 1;
        //print(_player.transform.position.y);
        // if (GetNearestHeightBlock() == 1) 
        //     heightBlock = 2;
        // else if (GetNearestHeightBlock() <= 5f && GetNearestHeightBlock() >= 4f)
        //     heightBlock = 5;
        print($"heightBlock - {heightBlock} posLastCube - {GetNearestHeightBlock()}");
        switch (GetNearestHeightBlock())
        {
            case 1:
                heightBlock = 2;
                break;
            
            case 2:
                heightBlock = 3;
                break;
            
            case 3:
                heightBlock = 4;
                break;
            
            case 4:
                heightBlock = 1;
                break;
        }
        int numberRandom = UnityEngine.Random.Range(0, _objects.Length);
        if (_objects[numberRandom].Height <= heightBlock)
        {
            return _objects[numberRandom].PrefabObject;
        }
        else
        {
            numberRandom = UnityEngine.Random.Range(0, _objects.Length);
            while (true)
            {
                if (_objects[numberRandom].Height <= heightBlock)
                {
                    return _objects[numberRandom].PrefabObject;
                }
                numberRandom = UnityEngine.Random.Range(0, _objects.Length);
            }
        }
    }
    
    // Возвращает высоту последнего блока
    private float GetNearestHeightBlock()
    {
        //GameObject objectEnd = transform.GetChild(-1).gameObject;

        if (_lastObject != null)
        {
            for (int i = 0; i < _objects.Length; i++)
            {
                if (_objects[i].Name == _lastObject.name)
                    return _objects[i].Height;
            }
        }
        //print("End");
        return 1f;
        // float minDistance = 100.0f;
        // GameObject resObject = null;
        // for (int i = 0; i < transform.childCount; i++)
        // {
        //     float dis = Vector3.Distance(transform.GetChild(i).position, _player.transform.position);
        //     if (dis < minDistance)
        //     {
        //         minDistance = dis;
        //         resObject = transform.GetChild(i).gameObject;
        //     }
        // }
        // return resObject;
    }
    
    
}
