using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    [Serializable]    
    public class ColliderClass
    {
        [Header("Имя коллайлера")]
        public string NameCollider = "None";
        [Header("Коллайдер")]
        public Collider Collider = null;
        [Header("Позиция правой точки коллайдера")]
        public Vector3 PositionRightCollider = Vector3.zero;
        [Header("Позиция левой точки коллайдера")]
        public Vector3 PositionLeftCollider = Vector3.zero;
    }
    
    
    // Данный скрипт проверяет прикосновение с игроком. Нужно дорабатывать. 

    [Header("Точка, которая нужна для движения камеры")]
    public Transform TransformEnemyObject = null;
    
    [SerializeField] [Header("Коллайдеры, которые есть на объекте")]
    private List<ColliderClass> _collidersClass = new List<ColliderClass>();

    // Все boxColliders, которые есть на объекте
    private Collider[] _colliders = null;

    // Точка куда приземлился джампер
    private Vector3 _positionLandingPlayer = Vector3.zero;
    
    // Камера
    private Camera _camera = null;
    
    //[Header("Префаб шарика, который создается по краям")]
    //public GameObject PrefabSpehere = null;

    [HideInInspector] public GameOverPlayer GameOverPlayer = null;
    
    private void Start()
    {
        _colliders = GetComponents<Collider>();
        // Получение точки, которая является ребенком объекта
        TransformEnemyObject = transform.GetChild(0).transform;
        _camera = Camera.main;;
        for (int i = 0; i < _colliders.Length; i++)
        {
            ColliderClass colliderClass = new ColliderClass();
            
            colliderClass.NameCollider = $"Collider - {i}";
            colliderClass.Collider = _colliders[i];
            //BoxCollider collider = _boxCollidersObject[i]; 
            var positionCenterBounds = _colliders[i].bounds.center;
            //_positionMaximumHeight = _boxColliders[i].bounds.ClosestPoint(new Vector3(_positionCenterBounds.x, Mathf.Infinity, _positionCenterBounds.z));
            
            var positionRightCollider = _colliders[i].bounds.ClosestPoint(new Vector3(Mathf.Infinity, positionCenterBounds.y, positionCenterBounds.z));
            var positionLeftCollider = _colliders[i].bounds.ClosestPoint(new Vector3(-Mathf.Infinity, positionCenterBounds.y, positionCenterBounds.z));

            // GameObject newSphereRight = Instantiate(PrefabSpehere, transform, false);
            // newSphereRight.transform.localPosition = positionRightCollider;
            //
            // GameObject newSphereLeft = Instantiate(PrefabSpehere, transform, false);
            // newSphereLeft.transform.localPosition = positionLeftCollider;
            
            colliderClass.PositionRightCollider = positionRightCollider;
            colliderClass.PositionLeftCollider = positionLeftCollider;
            
            _collidersClass.Add(colliderClass);
        }
    }


    private List<Vector3> _normals = new List<Vector3>();
    private List<Vector3> _points = new List<Vector3>();
    
    //private void OnCollisionEnter(Collision other)
    //{
        // _normals.Add(other.GetContact(0).normal);
        // _points.Add(other.GetContact(0).point);
        //
        // print($"Название объекта" + gameObject.name);
        // for (int i = 0; i < _normals.Count; i++)
        // {
        //     float angleVector = Vector3.Angle(_normals[i], Vector3.right);
        //     print($"Angle Vector - {angleVector}");
        //     if (angleVector > 80 && angleVector < 140)
        //     {
        //         print("Continue to play");
        //     }
        //     else
        //     {
        //         print("Gameover Player");
        //         GameOverPlayer.GameOverPlayerMethod();
        //     }
        // }
        // print("------------------------------");

    //}

    private void OnDrawGizmos()
    {
        if (_normals == null)
            return;
        
        for (int i = 0; i < _normals.Count; i++)
        {
            Vector3 newVector = _points[i] - -_normals[i];
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(newVector, _points[i]);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_points[i], new Vector3(_points[i].x + 4, _points[i].y, 0));
        }
        
    }


    public void CheckGameOver(Collision collision)
    {
        // _normals.Add(collision.GetContact(0).normal);
        // _points.Add(collision.GetContact(0).point);
        
        _normals.Add(collision.GetContact(0).normal);
        _points.Add(collision.GetContact(0).point);
        
        //print("Check GameOver");
        //float angleVector = Vector3.Angle(collision.GetContact(0).normal, Vector3.right);
        //print($"Angle Vector - {angleVector}");
        //print("-----------------------------");
        
        //print($"Название объекта" + gameObject.name);
        for (int i = 0; i < _normals.Count; i++)
        {
            if (!ClickTracking.GameOverPlayer)
            {
                float angleVector = Vector3.Angle(_normals[i], Vector3.right);
                print($"Angle Vector - {angleVector}");
                if (angleVector > 70 && angleVector < 130)
                {
                    print("Continue to play");
                }
                else
                {
                    GameOverPlayer.GameOverPlayerMethod();
                }
            }
        }
        // print("------------------------------");
        
         //Vector3[] arrayPoints = GetLeftRightPositionColliderPoints(collision.collider);
         //print($"arrayPoints - {arrayPoints}");
        //  Vector3 positionRightCollider = arrayPoints[0];
        //  Vector3 positionLeftCollider = arrayPoints[1];
        //
        //  _positionLandingPlayer = _camera.transform.InverseTransformPoint(collision.GetContact(0).point);
        //
        //  if (_positionLandingPlayer.x < positionRightCollider.x && // - 0.1f
        //      _positionLandingPlayer.x > positionLeftCollider.x) //+ 0.1f // _positionLandingPlayer.y > _positionCenterBounds.y &&
        // {
        //     print("Player in the game;");
        // }
        // else
        // {
        //     print("Game OVER");
        //     GameOverPlayer.GameOverPlayerMethod();
        // } 
    }

    // Метод возвращает правую и левую точку коллайдера, на который прилетел джампер
    private Vector3[] GetLeftRightPositionColliderPoints(Collider colliderLanding)
    {
        Vector3[] arrayPoints = new Vector3[2];
        for (int i = 0; i < _collidersClass.Count; i++)
        {
            if (_collidersClass[i].Collider == colliderLanding)
            {
                arrayPoints[0] = _collidersClass[i].PositionRightCollider;
                arrayPoints[1] = _collidersClass[i].PositionLeftCollider;
                return arrayPoints;
            }
        }

        return null;
    }
}
