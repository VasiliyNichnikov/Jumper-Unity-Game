using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private GameObject player;
    public float destroy_distantion = 10;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Jumper");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        var objects = GameObject.FindGameObjectsWithTag("Object");
        foreach (var item in objects)
        {
            if (item.transform.position.x - player.transform.position.x > destroy_distantion)
            {
                Destroy(item);
            }
        }
    }
}
