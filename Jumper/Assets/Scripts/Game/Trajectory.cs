using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public GameObject sphere = null;
    
    private LineRenderer _lineRenderer = null;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void ShowTrajectory(Vector3 origin, Vector3 speed, ref bool jump, float angle)
    {
        //GameObject jumper = Instantiate(JumperPrefab, origin, Quaternion.identity);
        //jumper.GetComponent<Rigidbody>().AddForce(speed, ForceMode.VelocityChange);

        //Physics.autoSimulation = false;
        //print(speed / 2 * Physics.gravity * Physics.gravity);
        //print((speed * speed * Math.Sin(Mathf.Abs(angle)) * Math.Sin(Mathf.Abs(angle))) / (2 * Physics.gravity * Physics.gravity));
        Vector3[] points = new Vector3[100];
        _lineRenderer.positionCount = points.Length;
        float yMax = 0;
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            //Physics.Simulate(0.1f);
            //points[i] = jumper.transform.position;
            points[i] = origin + speed * time + Physics.gravity * time * time / 2f;
            if (points[i].y > yMax)
                yMax = points[i].y;
            if (points[i].y < 0 && jump)
            {
                Instantiate(sphere, points[i], Quaternion.identity);
                jump = false;
            }
            
        }
        //print(yMax);
        _lineRenderer.SetPositions(points);
        
        //Physics.autoSimulation = true;
        //Destroy(jumper.gameObject);
        
    }
    
}
