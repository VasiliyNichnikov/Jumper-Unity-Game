using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
  [Header("Родитель в котором создаются обои и полы")]
  public Transform ParentPrefabWallpaperBlockFloorObjects = null;

  [Header("Дополнительный скрипт, который добавляет доп. компоненты объектам")]
  public ManagingMapObjects ManagingMapObjects = null;

  [Header("Скрипт, который управляет камерой")]
  public CameraTracking CameraTracking = null;

  [Header("Префаб обоев и пола (блок)")] public GameObject PrefabWallpaperBlockFloor = null;
  
  
  //public KeyCode spawnKey;
  public GameObject player;
  public ObjectInfo[] prefabs;
  public int CountlastPrefabs;
  public float distantionSpawn = 20;
  private float MAX_JAMPER_VELOCITY = 9.6f;
  [Header("Difficulty")]
  public float maxDifficultyByDistance;
  public float minDistance;
  public float startMaxDistance;
  private float _endMaxDistance;
  public float minHeight;
  private float _maxHeight;

  private int _maxAmountSpawnedObjects = 8;
  private GameObject last_pref;
  private List<GameObject> last_objects;
  private List<GameObject> _wallpaperBlockAndFloorObjectsList = new List<GameObject>();

  
  private int _positionXWallpaperBlockFloor = 0;
  void Start()
  {
    _positionXWallpaperBlockFloor = 16;
    last_objects = new List<GameObject>();
    _endMaxDistance = GetMaxDistance();
    _maxHeight = GetMaxHeight();
    SpawnFirst();
    GenerationBackground();
  }
  
  void Update()
  { 
    ChekPlayerPosition();
  }

  
  
  private void GenerationBackground(int number = 8)
  {
    for (int i = 0; i < number; i++)
    {
      GameObject newWallpaperBlockFloor = Instantiate(PrefabWallpaperBlockFloor, ParentPrefabWallpaperBlockFloorObjects, false);
      newWallpaperBlockFloor.transform.position = new Vector3(_positionXWallpaperBlockFloor, 0, -7.2f);
      _positionXWallpaperBlockFloor -= 16;
      _wallpaperBlockAndFloorObjectsList.Add(newWallpaperBlockFloor);
    }
  }
  
  
  void ChekPlayerPosition()
  {
    if (Mathf.Abs(last_pref.transform.position.x - player.transform.position.x) < distantionSpawn)
    {
      Spawn();
      if (transform.childCount > _maxAmountSpawnedObjects)
      {
        Destroy(transform.GetChild(0).gameObject);
        // Destroy(_wallpaperBlockAndFloorObjectsList[0]);
        // _wallpaperBlockAndFloorObjectsList.RemoveAt(0);
        GenerationBackground(1);
      }
    }
  }

  GameObject getOBJ()
  {
    Shuffle(ref prefabs);
    ObjectInfo lastPrefInfo = last_pref.GetComponent<ObjectInfo>();
    foreach (var objectInfo in prefabs)
    {
      if (objectInfo.sizeY - lastPrefInfo.sizeY < GetDifficultyByValue(minHeight, _maxHeight) &&
        !last_objects.Contains(objectInfo.gameObject))
      {
        return objectInfo.gameObject;
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
    ManagingMapObjects.AddAdditionalParametersCheckCollider(last_pref.GetComponent<CheckCollider>());
    var GOinfo = pre_last_pref.GetComponent<ObjectInfo>();
    // граница последнего объекта
    float x1 = GOinfo.transform.position.x - GOinfo.sizeX / 2;
    // расстояние между объектами
    float x2 = -Random.Range(minDistance, GetDifficultyByValue(startMaxDistance, _endMaxDistance));

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
    var obj = prefabs[Random.Range(0, prefabs.Length)];
    last_objects.Add(obj.gameObject);
    
    last_pref = Instantiate(
        obj.gameObject,
        Vector3.zero,
        new Quaternion(), transform);
    ManagingMapObjects.AddAdditionalParametersCheckCollider(last_pref.GetComponent<CheckCollider>());
    CameraTracking.PositionY = last_pref.transform.GetChild(0).transform.position.y;
  }

  float GetDifficultyPercent()
  { 
    return Mathf.Clamp01(Mathf.Abs(player.transform.position.x) / maxDifficultyByDistance);
  }

  float GetDifficultyByValue(float min, float max)
  {
    return Mathf.Lerp(min, max, GetDifficultyPercent());
  }

  float GetMaxDistance()
  {
    float angle = 45f;
    return (MAX_JAMPER_VELOCITY * MAX_JAMPER_VELOCITY * Mathf.Sin(2 * angle)) / Physics.gravity.magnitude;
  }

  float GetMaxHeight()
  {
    float angle = 45f;
    return (MAX_JAMPER_VELOCITY * MAX_JAMPER_VELOCITY * Mathf.Sin(angle) * Mathf.Sin(angle)) / (Physics.gravity.magnitude * 2);
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

