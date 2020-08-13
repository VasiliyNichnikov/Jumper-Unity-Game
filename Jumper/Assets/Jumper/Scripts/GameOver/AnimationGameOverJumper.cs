using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class AnimationGameOverJumper : MonoBehaviour
{
    // Верхняя часть джампера
    private GameObject _upperPartJumper = null;
    
    // Нижняя часть джампера
    private GameObject _bottomPartJumper = null;

    private Rigidbody _rigidbodyJumper = null;
    private CapsuleCollider _capsuleCollider = null;

    private Rigidbody _rigidbodyUpperPartJumper = null;
    private Rigidbody _rigidbodyLowerPartJumper = null;

    private void Start()
    {
        _rigidbodyJumper = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }
    
    // Данный метод передает параметр UpperPart и BottomPart джампера
    public void ChangeUpperAndBottomPartsJumper(GameObject upperPart, GameObject bottomPart)
    {
        _upperPartJumper = upperPart;
        _bottomPartJumper = bottomPart;
    }
    
    public void StartAnimationGameOver()
    {
        _rigidbodyJumper.useGravity = false;
        _capsuleCollider.isTrigger = true;
        _rigidbodyJumper.constraints = RigidbodyConstraints.FreezeAll;

        if (_upperPartJumper.GetComponent<Rigidbody>() == null)
            _upperPartJumper.AddComponent<Rigidbody>();
        if (_bottomPartJumper.GetComponent<Rigidbody>() == null)
            _bottomPartJumper.AddComponent<Rigidbody>();

        _upperPartJumper.GetComponent<BoxCollider>().isTrigger = false;
        _bottomPartJumper.GetComponent<CapsuleCollider>().enabled = true;

        _rigidbodyUpperPartJumper = _upperPartJumper.GetComponent<Rigidbody>();
        _rigidbodyLowerPartJumper = _bottomPartJumper.GetComponent<Rigidbody>();

        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        float z = Random.Range(0f, 1f);

        _rigidbodyUpperPartJumper.AddForce(new Vector3(x, y, z) * Random.Range(1f, 5f), ForceMode.Impulse);
        _rigidbodyLowerPartJumper.AddForce(new Vector3(-x, -y, -z) * Random.Range(1f, 5f), ForceMode.Impulse);
    }
}
