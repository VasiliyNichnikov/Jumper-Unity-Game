using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsTiming : MonoBehaviour
{
  public Text textView;
  public static Dictionary<string, List<float>> objectsData;
  private Transform _lastObject;
  private Generation _generation;
  private float _lastTime;

  void Start()
  {
    _generation = FindObjectOfType<Generation>();
    if (objectsData == null) {
      objectsData = new Dictionary<string, List<float>>();
      for (int i = 0; i < _generation.prefabs.Length; i++)
      {
        objectsData.Add(_generation.prefabs[i].GetComponent<ObjectInfo>().Name, new List<float>());
      }
    }
    FindObjectOfType<FlightJumper>().OnJumperStop += OnJumperStopped;
    _lastTime = Time.time;
  }

  private void OnJumperStopped(Collision other)
  {
    if (other.gameObject.tag == "Object")
    {
      if (_lastObject == null)
      {
        _lastObject = _generation.transform.GetChild(0);
      }
      if (_lastObject != other.transform)
      {
        string name = _lastObject.GetComponent<ObjectInfo>().Name;
        objectsData[name].Add((float)System.Math.Round(Time.time - _lastTime, 2));
        _lastTime = Time.time;
        _lastObject = other.transform;
      }
    }
    
  }

  public void SetObjectsData()
  {
    textView.text = "";
    foreach (KeyValuePair<string, List<float>> objectData in objectsData)
    {
      textView.text += $"{objectData.Key}\n";
      float[] valuesArr = objectData.Value.ToArray();
      string values = String.Join("  ", valuesArr);
      textView.text += $"Times: {values}\n";
      float totalValue = valuesArr.Aggregate(0f, (acc, val) => acc + val);
      textView.text += $"AVG: {totalValue/valuesArr.Length}\n";
      textView.text += String.Concat(Enumerable.Repeat("-", 30)) + "\n";
    }
  }
}

