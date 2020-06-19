using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CheckModels : MonoBehaviour
{
    [SerializeField] [Header("Все модели")] 
    private List<GameObject> _allModels = new List<GameObject>();
    
    [SerializeField] private Text _textFPS = null;
    

    private GameObject _last_object = null;

    private int numberModel = 0;
    
    void Start()
    {
        CreateModel();
    }

    private void Update()
    {
        float fps = 1.0f / Time.deltaTime;
        _textFPS.text = $"FPS: {fps.ToString()}";
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Change Models"))
        {
            print("You clicked the button!");
            numberModel++;
            if (numberModel >= _allModels.Count)
                numberModel = 0;
            CreateModel();
        }
    }

    private void CreateModel()
    {
        if(_last_object != null)
            Destroy(_last_object);
        _last_object = Instantiate(_allModels[numberModel], transform, false);
    }
    
}
