using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScore : MonoBehaviour
{
    public Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target==null)
        {
            Debug.Log("не неазначена цель преследования");
            return;
        }
        if (Target.position.x>transform.position.x)
        {
            transform.position = new Vector3( Target.position.x,transform.position.y,transform.position.z);
        }
    }
}
