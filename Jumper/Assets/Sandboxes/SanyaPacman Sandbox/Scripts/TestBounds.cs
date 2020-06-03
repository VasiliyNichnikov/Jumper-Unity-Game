using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBounds : MonoBehaviour
{
    public GameObject clone;
    public KeyCode CloneObj;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Control();
    }
    void Control()
    {
        var col = GetComponent<Collider>();
        var center = col.bounds.center;
        if (Input.GetKeyDown(CloneObj))
        {
            //Instantiate(clone, col.bounds.ClosestPoint(new Vector3(Mathf.Infinity, center.y, center.z)),Quaternion.identity);
            //Instantiate(clone, col.bounds.ClosestPoint(new Vector3(center.x, center.y, Mathf.Infinity)), Quaternion.identity);
            Instantiate(clone, col.bounds.ClosestPoint(new Vector3(center.x, Mathf.Infinity, center.z)), Quaternion.identity);

            //Instantiate(clone, col.bounds.ClosestPoint(new Vector3(-Mathf.Infinity, center.y, center.z)), Quaternion.identity);
            //Instantiate(clone, col.bounds.ClosestPoint(new Vector3(center.x, center.y, -Mathf.Infinity)), Quaternion.identity);
            //Instantiate(clone, col.bounds.ClosestPoint(new Vector3(center.x, -Mathf.Infinity, center.z)), Quaternion.identity);
        }
    }
}
 