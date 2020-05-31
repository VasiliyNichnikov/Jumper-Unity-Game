using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public KeyCode MoveRight;
    public float moveSpeed = 0.5f;
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
        if(Input.GetKey(MoveRight))
        {
            transform.position += new Vector3(moveSpeed*Time.deltaTime, 0, 0 );
        }
    }
}
