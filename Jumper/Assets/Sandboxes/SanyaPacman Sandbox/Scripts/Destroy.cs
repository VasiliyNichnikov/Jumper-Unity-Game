using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private GameObject player;
    public float destroy_distantion=10;
    // Start is called before the first frame update
    void Start()
    {
        player= GameObject.FindGameObjectWithTag("Jumper");
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player);
        if (player == null) return;
        //Debug.Log(transform.position.x - player.transform.position.x);
        if (player.transform.position.x- transform.position.x> destroy_distantion)
        {
            Destroy(gameObject);
        }
    }
}
