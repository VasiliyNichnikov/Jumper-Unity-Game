using UnityEngine;
using Random = UnityEngine.Random;

public class AnimationGameOverJumper : MonoBehaviour
{
    // Верхняя часть джампера
    private GameObject _upperPartJumper;
    
    // Нижняя часть джампера
    private GameObject _bottomPartJumper;

    private Rigidbody _rigidbodyJumper;
    private CapsuleCollider _capsuleCollider;

    private Rigidbody _rigidbodyUpperPartJumper;
    private Rigidbody _rigidbodyBottomPartJumper;

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

        _rigidbodyUpperPartJumper = _upperPartJumper.GetComponent<Rigidbody>();
        _rigidbodyBottomPartJumper = _bottomPartJumper.GetComponent<Rigidbody>();
        
        _upperPartJumper.GetComponent<BoxCollider>().isTrigger = false;
        _bottomPartJumper.GetComponent<CapsuleCollider>().enabled = true;
        
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        float z = Random.Range(0f, 1f);

        _rigidbodyUpperPartJumper.AddForce(new Vector3(x, y, z) * Random.Range(1f, 5f), ForceMode.Impulse);
        _rigidbodyBottomPartJumper.AddForce(new Vector3(-x, -y, -z) * Random.Range(1f, 5f), ForceMode.Impulse);
        
        _rigidbodyUpperPartJumper.interpolation = RigidbodyInterpolation.Interpolate;
        //_rigidbodyBottomPartJumper.interpolation = RigidbodyInterpolation.Interpolate;
    }

}
