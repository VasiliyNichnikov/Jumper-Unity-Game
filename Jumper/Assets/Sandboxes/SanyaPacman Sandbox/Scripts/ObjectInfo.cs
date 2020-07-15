using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo: MonoBehaviour
{
    [Header("Название объекта")] [SerializeField] public string Name = "None";
    //[Header("Объект")] public GameObject PrefabObject = null;
    /// <summary>
    /// BotLeftBackPoint. Граничная точка 1 самая нижняя, левая, задняя точка.
    /// </summary>
    [Header("Граничная точка 1")] [SerializeField] public Vector3 BLB_Point;
    /// <summary>
    /// TopRightForward. Граничная точка 1 самая верхняя, правая, передняя точка
    /// </summary>
    [Header("Граничная точка 2")] [SerializeField] public Vector3 TRF_Point;

    /// <summary>
    /// размер объекта по Х
    /// </summary>
    [HideInInspector] [SerializeField]
    public float sizeX;
    /// <summary>
    /// размер объекта по Z
    /// </summary>
    [HideInInspector] [SerializeField]
    public float sizeZ;
    /// <summary>
    /// размер объекта по Y
    /// </summary>
    [HideInInspector] [SerializeField]
    public float sizeY;
    /// <summary>
    /// Центр не работает
    /// </summary>
    [HideInInspector] [SerializeField]
    public Vector3 center;
    public void AutoSize()
    {
        var meshcol = gameObject.AddComponent<MeshCollider>();
        meshcol.sharedMesh = GetComponent<MeshFilter>().sharedMesh;
        Bounds bounds = GetComponent<Collider>().bounds;
        //Vector3 center = bounds.center;
        BLB_Point = bounds.ClosestPoint(new Vector3(-Mathf.Infinity, -Mathf.Infinity, -Mathf.Infinity));
        TRF_Point = bounds.ClosestPoint(new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity));
        BLB_Point -= transform.position;
        TRF_Point -= transform.position;
        DestroyImmediate(GetComponent<MeshCollider>(),true);
        CalculateSizes();
    }
    /// <summary>
    /// ОБ
    /// </summary>
   public void CalculateSizes()
    {
        Vector3 size = TRF_Point - BLB_Point;
        sizeX = Mathf.Abs(size.x);
        sizeY = Mathf.Abs(size.y);
        sizeZ = Mathf.Abs(size.z);        
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255f, 255f, 255f);
        Gizmos.DrawSphere(transform.position+BLB_Point, 0.15f);
        Gizmos.DrawSphere(transform.position+TRF_Point, 0.15f);
        Gizmos.DrawLine(transform.position + BLB_Point, transform.position + new Vector3(TRF_Point.x, BLB_Point.y, BLB_Point.z));
        Gizmos.DrawLine(transform.position + BLB_Point, transform.position + new Vector3(BLB_Point.x, TRF_Point.y, BLB_Point.z));
        Gizmos.DrawLine(transform.position + BLB_Point, transform.position + new Vector3(BLB_Point.x, BLB_Point.y, TRF_Point.z));

        Gizmos.DrawLine(transform.position + TRF_Point, transform.position + new Vector3(BLB_Point.x, TRF_Point.y, TRF_Point.z));
        Gizmos.DrawLine(transform.position + TRF_Point, transform.position + new Vector3(TRF_Point.x, BLB_Point.y, TRF_Point.z));
        Gizmos.DrawLine(transform.position + TRF_Point, transform.position + new Vector3(TRF_Point.x, TRF_Point.y, BLB_Point.z));

        Gizmos.DrawLine(transform.position + new Vector3(BLB_Point.x, BLB_Point.y, TRF_Point.z), transform.position + new Vector3(BLB_Point.x, TRF_Point.y, TRF_Point.z));
        Gizmos.DrawLine(transform.position + new Vector3(BLB_Point.x, BLB_Point.y, TRF_Point.z), transform.position + new Vector3(TRF_Point.x, BLB_Point.y, TRF_Point.z));

        Gizmos.DrawLine(transform.position + new Vector3(TRF_Point.x, TRF_Point.y, BLB_Point.z), transform.position + new Vector3(TRF_Point.x, BLB_Point.y, BLB_Point.z));
        Gizmos.DrawLine(transform.position + new Vector3(TRF_Point.x, TRF_Point.y, BLB_Point.z), transform.position + new Vector3(BLB_Point.x, TRF_Point.y, BLB_Point.z));
        
        Gizmos.DrawLine(transform.position + new Vector3(BLB_Point.x, TRF_Point.y, TRF_Point.z), transform.position + new Vector3(BLB_Point.x, TRF_Point.y, BLB_Point.z));
        Gizmos.DrawLine(transform.position + new Vector3(TRF_Point.x, BLB_Point.y, BLB_Point.z), transform.position + new Vector3(TRF_Point.x, BLB_Point.y, TRF_Point.z));

    }
}
