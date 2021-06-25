using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingJumperDistance : MonoBehaviour
{
    [SerializeField] [Header("Префаб джампера")]
    private GameObject _prefabJumper = null;

    [SerializeField] [Header("Скрипт, который отвечает за полет джампера")]
    private FlightJumper _flightJumper = null;

    [SerializeField] [Header("Стартовая позиция джампера")]
    private Vector3 _startPositionJumper = Vector3.zero;
    
    [SerializeField] [Header("Вектор, в который полетит джампер")]
    private Vector3 _vectorDifference = Vector3.zero;

    // Джампер созданный из префаба
    private GameObject _newJumper = null;
    
    // Rigidbody, который находится на новом джампере
    private Rigidbody _rigidbodyNewJumper = null;

    private void Start()
    {
        _newJumper = Instantiate(_prefabJumper, _startPositionJumper, Quaternion.identity);
        _rigidbodyNewJumper = _newJumper.GetComponent<Rigidbody>();
        var maximumSpeedJumper = _flightJumper.GetMaximumSpeedJumper;
        
        Vector3 startPositionJumper = _newJumper.transform.position;
        Vector3 endPositionJumper = _newJumper.transform.position;
        
        _rigidbodyNewJumper.AddForce(_vectorDifference * maximumSpeedJumper, ForceMode.Impulse);
        
        Physics.autoSimulation = false;

        for (int i = 1; i < 150; i++)
        {
            Physics.Simulate(0.02f);
        }
        Physics.autoSimulation = true;
        endPositionJumper = _newJumper.transform.position;
        
        print($"Distance of axes X - {endPositionJumper.x - startPositionJumper.x}");
        
        Destroy(_newJumper.gameObject);
        //TransformEnemyObject = null;
    }
}
