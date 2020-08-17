using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
  public int score
  {
    get
    {
      return _score;
    }
    private set
    {
      _score = value;
      UpdateText();
    }
  }
  public Text scoreText;
  private Transform _lastObject;
  private int _score = 0;
  private Generation _generation;

  void Start()
  {
    _generation = FindObjectOfType<Generation>();
    UpdateText();
    FindObjectOfType<FlightJumper>().OnJumperStop += OnJumperStopped;
  }

  private void OnJumperStopped(Collision other)
  {
    if (other.gameObject.tag == "Object")
    {
      if (_lastObject == null)
      {
        _lastObject = _generation.transform.GetChild(0);
      }
      if (_lastObject.position.x > other.transform.position.x)
      {
        int startIndex = 0;
        int lastIndex = 0;
        for (int i = 0; i < _generation.transform.childCount; i++)
        {
          Transform child = _generation.transform.GetChild(i);
          if (child == _lastObject)
          {
            startIndex = i;
          }
          if (child == other.transform)
          {
            lastIndex = i;
            _lastObject = child;
            break;
          }
        }
        score += lastIndex - startIndex;
      }
    }
  }

  private void UpdateText()
  {
    scoreText.text = $"Score - {score}";
  }
}