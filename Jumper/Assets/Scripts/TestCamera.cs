using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCamera : MonoBehaviour
{
    [SerializeField] private Text _textFPS = null;
    
    void Update()
    {
        int fps = Mathf.RoundToInt(1.0f / Time.deltaTime);
        _textFPS.text = $"FPS: {fps.ToString()}";

    }
  
}
