using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFall : MonoBehaviour
{

    private Transform _thisTransform = null;
    private float _positionSaveY = .0f;

    private void Start()
    {
        _thisTransform = transform;
    }

    private void Update()
    {
        float positionNowY = _thisTransform.position.y;

        if (Math.Round(positionNowY, 2) != Math.Round(_positionSaveY, 2))
        {
            print("Игрок падает");
            _positionSaveY = positionNowY;
        }
        else
        {
            print("Игрок упал");
            print($"Position Now Y - {Math.Round(positionNowY, 8)}");
            print($"Position Save Y - {Math.Round(_positionSaveY, 8)}");
        }
        
        
    }
}
