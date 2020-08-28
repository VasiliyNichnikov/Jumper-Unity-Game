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
    // private Vector3 _positionLandingPlayer = Vector3.zero;
    
    // Камера
    // private Camera _camera = null;
    
    //[Header("Префаб шарика, который создается по краям")]
    //public GameObject PrefabSpehere = null;

    [HideInInspector] public GameOverPlayer GameOverPlayer = null;
    
    private void Start()
    {
        _colliders = GetComponents<Collider>();
        // Получение точки, которая является ребенком объекта
        TransformEnemyObject = transform.GetChild(0).transform;
        // _camera = Camera.main;;
        for (int i = 0; i < _colliders.Length; i++)
        {
            ColliderClass colliderClass = new ColliderClass();
            
            colliderClass.NameCollider = $"Collider - {i}";
            colliderClass.Collider = _colliders[i];
            var positionCenterBounds = _colliders[i].bounds.center;

            var positionRightCollider = _colliders[i].bounds.ClosestPoint(new Vector3(Mathf.Infinity, positionCenterBounds.y, positionCenterBounds.z));
            var positionLeftCollider = _colliders[i].bounds.ClosestPoint(new Vector3(-Mathf.Infinity, positionCenterBounds.y, positionCenterBounds.z));

            colliderClass.PositionRightCollider = positionRightCollider;
            colliderClass.PositionLeftCollider = positionLeftCollider;
            
            _collidersClass.Add(colliderClass);
        }
    }


    private List<Vector3> _normals = new List<Vector3>();
    private List<Vector3> _points = new List<Vector3>();
    

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
    
    
    // Данный метод провераяет, может игрок прыгать дальше или нет
    public bool CheckJumpPlayer(Collision collision)
    {
        var normal = collision.GetContact(0).normal;
        if (!ClickTracking.GameOverPlayer)
        {
            float angleVector = Vector3.Angle(normal, Vector3.right);
            //print($"Angle - {angleVector}");
            // Нужно выбрать угол 
            if (angleVector > 20 && angleVector < 140)
            {
                return true;
            }
        }
        return false;
    }
    

    public void CheckGameOver(Collision collision)
    {
        _normals.Add(collision.GetContact(0).normal);
        _points.Add(collision.GetContact(0).point);
       
        
        // print($"Name Object" + gameObject.name);
        for (int i = 0; i < _normals.Count; i++)
        {
            if (!ClickTracking.GameOverPlayer)
            {
                float angleVector = Vector3.Angle(_normals[i], Vector3.right);
                // print($"Angle Vector - {angleVector}");
                if (angleVector > 70 && angleVector < 130)
                {
                    print("Continue to play");
                }
                else
                {
                    //GameOverPlayer.GameOverPlayerMethod();
                }
            }
        }
        // print("------------------------------");
    }

    // Метод возвращает правую и левую точку коллайдера, на который прилетел джампер
    // private Vector3[] GetLeftRightPositionColliderPoints(Collider colliderLanding)
    // {
    //     Vector3[] arrayPoints = new Vector3[2];
    //     for (int i = 0; i < _collidersClass.Count; i++)
    //     {
    //         if (_collidersClass[i].Collider == colliderLanding)
    //         {
    //             arrayPoints[0] = _collidersClass[i].PositionRightCollider;
    //             arrayPoints[1] = _collidersClass[i].PositionLeftCollider;
    //             return arrayPoints;
    //         }
    //     }
    //
    //     return null;
    // }
}
