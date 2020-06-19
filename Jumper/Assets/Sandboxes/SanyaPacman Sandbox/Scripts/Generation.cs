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
    public float heghtDifference=1;
    public int CountlastPrefabs;
    public float distantionSpawn = 20;

    private GameObject last_pref;
    private List<GameObject> last_objects;
    private Vector3 end;
    private Vector3 start;

    
    // Start is called before the first frame update
    void Start()
    {
        last_objects = new List<GameObject>();   
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
    GameObject getOBJ()
    {
        List<int> obj_indexes = new List<int>();
        for (int i = 0; i < prefabs.Length; i++)
        {
            obj_indexes.Add(i);           
        }
        
        while  (obj_indexes.Count > 0)
        {
            int i = Random.Range(0, obj_indexes.Count - 1);
            var obj = prefabs[obj_indexes[i]];
            if (obj.GetComponent<ObjectInfo>().sizeY - last_pref.GetComponent<ObjectInfo>().sizeY > heghtDifference||
                last_objects.Contains(obj))
            {                
                obj_indexes.Remove(i);                
            }
            else
                return obj;
        }
        Debug.Log("Ошибка, не найден подходящий объект");
        return new GameObject();
    }
    void Spawn()
    {  
        var pre_last_pref = last_pref;
        //last_pref = Instantiate(
        //    prefabs[(int)Random.Range(0, prefabs.Length)],
        //    transform, false);
        
        var bounds = pre_last_pref.GetComponent<Collider>().bounds;
        var obj = getOBJ();
        last_pref = Instantiate(obj, transform, false);
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
        last_objects.Add(obj);
        if (last_objects.Count>CountlastPrefabs)
        {
            last_objects.RemoveAt(0);
        }
    }
    void SpawnFirst()
    {
        var obj = prefabs[(int)Random.Range(0, prefabs.Length)];
        last_objects.Add(obj);
        last_pref = Instantiate(
            obj,
            Vector3.zero,
            new Quaternion(),transform);
    }
}
