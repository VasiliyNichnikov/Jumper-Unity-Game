using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : MonoBehaviour
{
    private List<Vector3> normals;
    private List<Vector3> points;
    // Start is called before the first frame update
    void Start()
    {
        normals = new List<Vector3>();
        points = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        normals.Add( collision.GetContact(0).normal.normalized);
        points.Add(collision.GetContact(0).point);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (normals==null)
            return;
        for (int i = 0; i < normals.Count; i++)
        {
            Gizmos.DrawLine(points[i] - normals[i]*25, points[i]); //хз почему минус
        }
    }
}
