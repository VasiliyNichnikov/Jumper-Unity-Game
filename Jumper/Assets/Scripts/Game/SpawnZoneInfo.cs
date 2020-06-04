using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZoneInfo : MonoBehaviour
{
    [Header("Префабы для генерации")] public GameObject[] PrefabsForSpawn;
    [Header("Граничная точка 1")] public Vector3 LeftPoint;
    [Header("Граничная точка 2")] public Vector3 RightPoint;

    

    // для отрисовки зоны спавны
    private float GizmosZone = 1f;

    //можно доработать, поле под последний предмет, для отсутсвия повторений.
    public static GameObject Last;
    /// <summary>
    /// Метод создает меш колайдер, собриает инфу о его размерах и удаляет его
    /// </summary>
    public void AutoSize()
    {
        
        var meshcol = gameObject.AddComponent<MeshCollider>();
        meshcol.sharedMesh = GetComponent<MeshFilter>().sharedMesh;
        Bounds bounds = GetComponent<Collider>().bounds;
        //Vector3 center = bounds.center;
        LeftPoint = bounds.ClosestPoint(new Vector3(-Mathf.Infinity, Mathf.Infinity, 0));
        RightPoint = bounds.ClosestPoint(new Vector3(Mathf.Infinity, Mathf.Infinity, 0));
        LeftPoint -= transform.position;
        RightPoint -= transform.position;
        LeftPoint.z = 0;
        RightPoint.z = 0;
        DestroyImmediate(GetComponent<MeshCollider>(),true);
       
    }
    /// <summary>
    /// спавн объекта в зоне
    /// </summary>
    public void SpawnOnZone()
    {
        if (PrefabsForSpawn == null|| PrefabsForSpawn.Length<=0)
        {
            Debug.Log("Нет объектов для спавна");
            return;
        }
        int rand = (int)Random.Range(0, PrefabsForSpawn.Length);
        GameObject placebleGO = PrefabsForSpawn[rand];
        ObjectInfo infoGO = placebleGO.GetComponent<ObjectInfo>();
        if (infoGO== null)
        {
            Debug.Log("На объекте нет ObjectInfo");
            return;
        }
        float p1x= LeftPoint.x;        
        p1x += infoGO.sizeX/2;
        float p2x = RightPoint.x;
        p2x -= infoGO.sizeX / 2;
        Vector3 place = new Vector3(Random.Range(p1x, p2x), LeftPoint.y, LeftPoint.z);
       
        var inst= Instantiate(infoGO, transform);
        inst.transform.localPosition = place;
    }
    /// <summary>
    /// отрисовка в инспекторе при выделении
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255f, 0f, 255f);
        Gizmos.DrawSphere(transform.position + LeftPoint, 0.05f);
        Gizmos.DrawSphere(transform.position + RightPoint, 0.05f);
        Vector3 p1 = transform.position + LeftPoint;
        Vector3 p2 = p1;
        p1.z -= GizmosZone;
        p2.z += GizmosZone;
        Vector3 p3 = transform.position + RightPoint;
        Vector3 p4 = p3;
        p3.z += GizmosZone;
        p4.z -= GizmosZone;
        Gizmos.DrawLine(p1,p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
}
