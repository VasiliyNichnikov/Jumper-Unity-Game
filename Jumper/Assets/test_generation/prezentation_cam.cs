using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prezentation_cam : MonoBehaviour{
    public generation gener;
	public float speed;
    void FixedUpdate(){
        transform.position = Vector3.Lerp(transform.position, gener.pos, speed);
    }	
}
