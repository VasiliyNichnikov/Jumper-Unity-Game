using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    [Header("Скрипт, который создает карту")] [SerializeField]
    private GenerationMap _generationMap = null;

    [Header("Игрок")] [SerializeField] private GameObject _player = null;

    [Header("Разницу между объектами")] [SerializeField]
    private float _differenceDistances = .0f;

    [Header("Скорость движения")] [SerializeField]
    private float _speedMove = .0f;
    
    private Transform _thisTransform = null;

    private void Start()
    {
        _thisTransform = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Block")
        {
            print("Destroy");
//            _generationMap.CreateObject();
            Destroy(other.gameObject);
        }
    }
    
    private void LateUpdate()
    {
        // Позиция, до которой нужно двигаться 
        float positionEndX = _player.transform.position.x - _differenceDistances;
        //float positionEndY = _transformTarget.transform.position.y + 3;
        _thisTransform.position = new Vector3(Mathf.MoveTowards(_thisTransform.position.x, positionEndX, _speedMove * Time.deltaTime), _thisTransform.position.y, _thisTransform.position.z);
        //    new Vector3(positionEndX, positionEndY, _thisTransform.localPosition.z), _speedCamera * Time.deltaTime);
        // _thisTransform.localPosition = new Vector3(Mathf.MoveTowards(_thisTransform.localPosition.x, positionEndX, 
        //     _speedCamera * Time.deltaTime), _thisTransform.localPosition.y, _thisTransform.localPosition.z); 
        
    }
    
}
