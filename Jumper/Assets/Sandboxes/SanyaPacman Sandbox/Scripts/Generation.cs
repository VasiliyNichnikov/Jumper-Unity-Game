using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public KeyCode spawnKey;
    public GameObject player;
    public GameObject[] prefabs;
    public float min_dist_between_pref;
    public float max_dist_between_pref;
    private GameObject last_pref;

    private Vector3 end;
    private Vector3 start;

    private float distantionSpawn=20;
    // Start is called before the first frame update
    void Start()
    {
        SpawnFirst();
        end = new Vector3(Mathf.Infinity, 0, 0);
        start = new Vector3(-Mathf.Infinity, 0, 0);       
    }

    // Update is called once per frame
    void Update()
    {
        ChekPlayerPosition();
    }
    void ChekPlayerPosition()
    {
        if (last_pref.transform.position.x- player.transform.position.x< distantionSpawn)
        {
            Spawn();
        }
    }
    void Control()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            Spawn();
        }
    }
    void Spawn()
    {  
        var pre_last_pref = last_pref;
        last_pref = Instantiate(
            prefabs[(int)Random.Range(0, prefabs.Length)],
            Vector3.zero,
            new Quaternion());
        var bounds = pre_last_pref.GetComponent<Collider>().bounds;
        float x1 = bounds.ClosestPoint(end).x;
        //Debug.Log("x1= " + x1);
        float x2 = Random.Range(min_dist_between_pref, max_dist_between_pref);
        //Debug.Log("x2= " + x2);
        bounds = last_pref.GetComponent<Collider>().bounds;
        float x3 = Mathf.Abs( bounds.ClosestPoint(start).x);
        //Debug.Log("x3= " + x3);
        //Debug.Log("Summ(x1 +x2 +x3)=" + (x1+ x2+ x3));
        last_pref.transform.position += new Vector3(x1+x2+x3, 0, 0);  //new Vector3((last_pref.GetComponent<Collider>().bounds.center.x - last_pref.GetComponent<Collider>().bounds.ClosestPoint(end).x),0,0); 
    }
    void SpawnFirst()
    {
        last_pref = Instantiate(
            prefabs[(int)Random.Range(0, prefabs.Length)],
            Vector3.zero,
            new Quaternion());
    }
}
