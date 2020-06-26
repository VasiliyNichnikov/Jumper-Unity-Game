using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCamera : MonoBehaviour
{
    [SerializeField] private Transform _player = null;

    [SerializeField] [Range(0, 100)] private float _offset = .0f;

    [SerializeField] [Range(0, 20)] private float _speedCamera = .0f;

    [SerializeField] private Text _textFPS = null;

    private Transform _thisTransform = null;   

    [SerializeField] private float _hudRefreshRate = 1f;
    private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        _thisTransform = transform;       
    }
    
    private float fps = 0;
    // Update is called once per frame
    void Update()
    {
        _thisTransform.position = Vector3.Lerp(_thisTransform.position,
            new Vector3(_player.position.x - _offset, _player.position.y + _offset, _thisTransform.position.z), _speedCamera * Time.deltaTime);
        _thisTransform.LookAt(_player);        
        
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _textFPS.text = "FPS: " + fps;
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }
  
}
