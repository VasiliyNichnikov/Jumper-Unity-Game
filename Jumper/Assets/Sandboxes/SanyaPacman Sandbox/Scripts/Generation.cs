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
        //Control();
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
            transform, false);
        
        var bounds = pre_last_pref.GetComponent<Collider>().bounds;
        var GOinfo = pre_last_pref.GetComponent<ObjectInfo>();
        float x1 = GOinfo.transform.position.x+ GOinfo.sizeX/2;

        //Debug.Log("x1= " + x1);
        float x2 = Random.Range(min_dist_between_pref, max_dist_between_pref);
        //Debug.Log("x2= " + x2);
        GOinfo = last_pref.GetComponent<ObjectInfo>();
        float x3 = Mathf.Abs(GOinfo.transform.position.x - GOinfo.sizeX/2);
        //Debug.Log("x3= " + x3);
        //Debug.Log("Summ(x1 +x2 +x3)=" + (x1+ x2+ x3));
        last_pref.transform.position += new Vector3(x1+x2+x3, 0, 0);  //new Vector3((last_pref.GetComponent<Collider>().bounds.center.x - last_pref.GetComponent<Collider>().bounds.ClosestPoint(end).x),0,0); 

        SpawnZoneInfo SpawnZone = last_pref.GetComponent<SpawnZoneInfo>();
        if (SpawnZone != null)
        {
            SpawnZone.SpawnOnZone();
        }
    }
    void SpawnFirst()
    {
        last_pref = Instantiate(
            prefabs[(int)Random.Range(0, prefabs.Length)],
            Vector3.zero,
            new Quaternion());
    }
}
