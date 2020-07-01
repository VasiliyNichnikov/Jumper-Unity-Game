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

    bool bug = false;
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
        Shuffle(ref prefabs);
        foreach (var obj in prefabs)
        { 
            if (!(obj.GetComponent<ObjectInfo>().sizeY - last_pref.GetComponent<ObjectInfo>().sizeY > heghtDifference ||
                last_objects.Contains(obj)))
                    return obj;
        }       
        
        return null;
    }
    void Spawn()
    {  
        var pre_last_pref = last_pref;
       
        var bounds = pre_last_pref.GetComponent<Collider>().bounds;
        var obj = getOBJ();
        if (obj == null)
        {
            Debug.Log("Ошибка, не найден подходящий объект");
            return;
        }
        last_pref = Instantiate(obj, transform, false);
        var GOinfo = pre_last_pref.GetComponent<ObjectInfo>();
        float x1 = GOinfo.transform.position.x+ GOinfo.sizeX/2;

       
        float x2 = Random.Range(min_dist_between_pref, max_dist_between_pref);
       
        GOinfo = last_pref.GetComponent<ObjectInfo>();
        float x3 = Mathf.Abs(GOinfo.transform.position.x - GOinfo.sizeX/2);
       
        last_pref.transform.position += new Vector3(x1+x2+x3, 0, 0); 

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
    public static void Shuffle<T>(ref List<T> list)
    {
        for (int i = list.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0,i + 1);
            var tmp = list[j];
            list[j] = list[i];
            list[i] = tmp;
        }
    }
    public static void Shuffle<T>( ref T[] arr)
    { 
        for (int i = arr.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var tmp = arr[j];
            arr[j] = arr[i];
            arr[i] = tmp;
        }
    }
}
