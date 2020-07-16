using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListModelsTest : MonoBehaviour
{
    [Serializable]
    public class Model
    {
        [Header("Имя модели")] public string Name = "None";
        [Header("Объект модели")] public GameObject Object = null;
        [Header("Пустышка объекта")] public Transform EmptyObjectTransform = null;
    }

    [SerializeField] [Header("Массив с моделями")]
    private Model[] _models = null;

    [HideInInspector] public GameObject SelectObjectNow = null;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < _models.Length; i++)
        {
            Vector3 startPos = _models[i].EmptyObjectTransform.position;
            Vector3 endPosition = Vector3.zero;;
            if (i + 1 < _models.Length)
            {
                endPosition = _models[i + 1].EmptyObjectTransform.position;
            }
            if(endPosition != Vector3.zero)
                Gizmos.DrawLine(startPos, endPosition);
        }
    }

    // public float GetPositionCameraY(Transform player)
    // {
        // if (SelectObjectNow == null)
        //     return -1;
        //GameObject selectObject = GetSelectObject(player);
        // if (selectObject == null)
        //     return -1;
        // Transform[] arrayObjects = GetTransforms(selectObject);
        // if (arrayObjects == null)
        //     return -1;
        // Vector3 positionNow = arrayObjects[0].position;
        // if (arrayObjects[1] == null)
        //     return -1;
        // Vector3 positionNext = arrayObjects[1].position;
        //print($"Position X: {positionNow.x}; Position Y: {positionNow.y}");
        //print($"Position X: {positionNext.x}; Position Y: {positionNext.y}");

        // var x1 = positionNow.x;
        // var y1 = positionNow.y;
        //
        // var x2 = positionNext.x;
        // var y2 = positionNext.y;
        //
        // var x = player.position.x;
        // var y = ((x - x1) * (y2 - y1) + y1 * (x2 - x1)) / (x2 - x1);
        // return y;
        // if(emptyNow != null)
        //     return emptyNow.position.y;
        // return -1.0f;
   // }

    public Transform GetEmptyTransform(GameObject objectCollider)
    {
        for (int i = 0; i < _models.Length; i++)
        {
            if (_models[i].Object == objectCollider)
            {
                return _models[i].EmptyObjectTransform;
            }
        }

        return null;
    }

    public Transform GetSelectObject(Transform player)
    {
        float minDis = 100000f;
        Transform returnObject = null;
        for (int i = 0; i < _models.Length; i++)
        {
            float dis = Vector3.Distance(player.position, _models[i].Object.transform.position);
            if (dis <= minDis)
            {
                minDis = dis;
                returnObject = _models[i].EmptyObjectTransform;
            }
        }
        return returnObject;
    }
    
    public GameObject GetSelectObject(GameObject player)
    {
        float minDis = 100000f;
        GameObject returnObject = null;
        for (int i = 0; i < _models.Length; i++)
        {
            float dis = Vector3.Distance(player.transform.position, _models[i].Object.transform.position);
            if (dis <= minDis)
            {
                minDis = dis;
                returnObject = _models[i].Object;
            }
        }
        return returnObject;
    }
    
    /*
     * Данный метод возвращает массив из двух объектов,
     * на котором находится джампер, а также объект, который идет следующим
     */
    private Transform[] GetTransforms(GameObject objectCollider)
    {
        Transform[] arrayTransforms = new Transform[2];
        
        for (int i = 0; i < _models.Length; i++)
        {
            if (_models[i].Object == objectCollider)
            {
                arrayTransforms[0] = _models[i].EmptyObjectTransform;
                if (i + 1 < _models.Length)
                    arrayTransforms[1] = _models[i + 1].EmptyObjectTransform;
                return arrayTransforms;
            }
        }

        return null;
    }
    
}
