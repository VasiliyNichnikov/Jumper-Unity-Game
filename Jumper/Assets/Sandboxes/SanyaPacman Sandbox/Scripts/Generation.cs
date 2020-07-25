using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
  public KeyCode spawnKey;
  public GameObject player;
  public GameObject[] prefabs;
  public int CountlastPrefabs;
  public float distantionSpawn = 20;
  [Header("Difficulty")]
  public float maxDifficultyByDistance;
  public float minDistance;
  public float maxDistance;
  public float minHeight;
  public float maxHeight;


  private GameObject last_pref;
  private List<GameObject> last_objects;
  // private Hashtable prefInfos = new Hashtable();

  bool bug = false;
  // Start is called before the first frame update
  void Start()
  {
    last_objects = new List<GameObject>();
    // foreach (GameObject item in prefabs)
    // {
    //   prefInfos.Add(item.name, 0);
    // }
    SpawnFirst();
  }

  // Update is called once per frame
  void Update()
  {
    ChekPlayerPosition();
    //Control();
  }

  void ChekPlayerPosition()
  {
    if (Mathf.Abs(last_pref.transform.position.x - player.transform.position.x) < distantionSpawn)
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
    float lastPrefSizeY = last_pref.GetComponent<ObjectInfo>().sizeY;
    foreach (var obj in prefabs)
    {
      if (obj.GetComponent<ObjectInfo>().sizeY - lastPrefSizeY < GetDifficultyByValue(minHeight, maxHeight) &&
        !last_objects.Contains(obj))
      {
        // prefInfos[obj.name] = (int)prefInfos[obj.name] + 1;
        // ICollection keys = prefInfos.Keys;
        // List<string> test = new List<string>();
        // foreach (string s in keys)
        //   test.Add(s + ": " + prefInfos[s]);
        // Debug.Log(string.Join(", ", test));
        return obj;
      }
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
    // граница последнего объекта
    float x1 = GOinfo.transform.position.x - GOinfo.sizeX / 2;
    // расстояние между объектами
    float x2 = -GetDifficultyByValue(minDistance, maxDistance);

    GOinfo = last_pref.GetComponent<ObjectInfo>();

    float x3 = -Mathf.Abs(GOinfo.transform.position.x - GOinfo.sizeX / 2);

    last_pref.transform.position += new Vector3(x1 + x2 + x3, 0, 0);

    SpawnZoneInfo SpawnZone = last_pref.GetComponent<SpawnZoneInfo>();
    if (SpawnZone != null)
    {
      SpawnZone.SpawnOnZone();
    }
    last_objects.Add(obj);
    if (last_objects.Count > CountlastPrefabs)
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
        new Quaternion(), transform);
  }

  float GetDifficultyPercent()
  {
    return Mathf.Clamp01(Mathf.Abs(player.transform.position.x) / maxDifficultyByDistance);
  }

  float GetDifficultyByValue(float min, float max)
  {
    return Mathf.Lerp(min, max, GetDifficultyPercent());
  }

  public static void Shuffle<T>(ref List<T> list)
  {
    for (int i = list.Count - 1; i >= 1; i--)
    {
      int j = Random.Range(0, i + 1);
      var tmp = list[j];
      list[j] = list[i];
      list[i] = tmp;
    }
  }

  public static void Shuffle<T>(ref T[] arr)
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

