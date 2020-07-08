using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCamera : MonoBehaviour
{
    [SerializeField] private Transform _player = null;

    [SerializeField] [Range(0, 100)] [Header("Расстояние по оси X")] private float _offsetX = .0f;
    [SerializeField] [Range(0, 100)] [Header("Расстояние по оси Y")] private float _offsetY = .0f;

    [SerializeField] [Range(0, 100)] [Header("Скорость движения камеры")] private float _speedCamera = .0f;

    [SerializeField] private Text _textFPS = null;

    private Transform _thisTransform = null;
    
    // Start is called before the first frame update
    void Start()
    {
        _thisTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _thisTransform.position = Vector3.MoveTowards(_thisTransform.position,
            new Vector3(_player.position.x - _offsetX, _player.position.y + _offsetY, _thisTransform.position.z), _speedCamera * Time.deltaTime);
        //_thisTransform.LookAt(_player);
        
        //_thisTransform.Translate(new Vector3(_player.position.x - _offsetX, _player.position.y + _offsetY, _thisTransform.position.z) * _speedCamera * Time.deltaTime);

        int fps = Mathf.RoundToInt(1.0f / Time.deltaTime);
        _textFPS.text = $"FPS: {fps.ToString()}";

    }
}
