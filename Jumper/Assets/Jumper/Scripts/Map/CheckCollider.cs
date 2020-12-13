using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{    
    public enum TypeCollider
    {
        None = 0,
        Continuation = 1,
        Defeat = 2
    }


    [Serializable]    
    public class ColliderClass
    {
        [Header("Имя коллайлера")]
        public string NameCollider = "None";
        [Header("Коллайдер")]
        public Collider Collider = null;

        [Header("Тип коллайдера")] public TypeCollider TypeCollider = TypeCollider.None; 
        
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

    // Все Colliders, которые есть на объекте
    private Collider[] _colliders = null;

    // Точка куда приземлился джампер
    // private Vector3 _positionLandingPlayer = Vector3.zero;
    
    // Камера
    // private Camera _camera = null;
    
    [Header("Префаб шарика, который создается по краям")]
    public GameObject PrefabSpehere = null;

    [HideInInspector] public GameOverPlayer GameOverPlayer = null;
    
    private void Start()
    {
        // _colliders = GetComponents<Collider>();
        // // Получение точки, которая является ребенком объекта
        TransformEnemyObject = transform.GetChild(0).transform;
        // // _camera = Camera.main;;
        // for (int i = 0; i < _colliders.Length; i++)
        // {
        //     ColliderClass colliderClass = new ColliderClass();
        //     
        //     colliderClass.NameCollider = $"Collider - {i}";
        //     colliderClass.Collider = _colliders[i];
        //     var positionCenterBounds = _colliders[i].bounds.center;
        //
        //     var positionRightCollider = _colliders[i].bounds.ClosestPoint(new Vector3(Mathf.Infinity, positionCenterBounds.y, positionCenterBounds.z));
        //     var positionLeftCollider = _colliders[i].bounds.ClosestPoint(new Vector3(-Mathf.Infinity, positionCenterBounds.y, positionCenterBounds.z));
        //
        //     colliderClass.PositionRightCollider = new Vector3(positionRightCollider.x + 0.05f, positionRightCollider.y, positionRightCollider.z);
        //     colliderClass.PositionLeftCollider = new Vector3(positionLeftCollider.x - 0.05f, positionLeftCollider.y, positionLeftCollider.z);;
        //
        //     // GameObject rightSphere = Instantiate(PrefabSpehere, transform, false);
        //     // rightSphere.transform.position = positionRightCollider;
        //     //
        //     // GameObject leftSphere = Instantiate(PrefabSpehere, transform, false);
        //     // leftSphere.transform.position = positionLeftCollider;
        //     
        //     _collidersClass.Add(colliderClass);
        // }
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
            // print($"Angle - {angleVector}");
            // Нужно выбрать угол 
            if (angleVector > 40 && angleVector < 140)
            {
                return true;
            }
        }
        return false;
    }


    public bool OnOffClickTrackingJumpPlayer(Collision collision)
    {
        for (int i = 0; i < _collidersClass.Count; i++)
        {
            if (collision.collider == _collidersClass[i].Collider)
            {
                if (_collidersClass[i].TypeCollider == TypeCollider.Defeat)
                    return false;
            }
        }
        return true;
    }
    

    public void CheckGameOver(Collision collision)
    {

        //_positionLandingPlayer = collision.GetContact(0).point;
        
        // GameObject playerSphere = Instantiate(PrefabSpehere, transform, false);
        // playerSphere.transform.position = _positionLandingPlayer;

        // (Vector3, Vector3) pointsLeftRight = GetLeftRightPositionColliderPoints(collision.collider);
        // Vector3 rightPoint = pointsLeftRight.Item1;
        // Vector3 leftPoint = pointsLeftRight.Item2;
        //
        // if (_positionLandingPlayer.x < rightPoint.x && _positionLandingPlayer.x > leftPoint.x)
        // {
        //     print("Продолжаем игру");
        //     ClickTracking.JumpPlayer = false;
        // }
        //
        // else
        // {
        //     print("Заканчиваем игру");
        // }

        //_normals.Add(collision.GetContact(0).normal);
        //_points.Add(collision.GetContact(0).point);


        // print($"Name Object" + gameObject.name);
        // for (int i = 0; i < _normals.Count; i++)
        // {
        //     if (!ClickTracking.GameOverPlayer)
        //     {
        //         float angleVector = Vector3.Angle(_normals[i], Vector3.right);
        //         // print($"Angle Vector - {angleVector}");
        //         if (angleVector > 70 && angleVector < 130)
        //         {
        //             print("Continue to play");
        //         }
        //         else
        //         {
        //             //GameOverPlayer.GameOverPlayerMethod();
        //         }
        //     }
        // }
        // print("------------------------------");
    }

    // Метод возвращает правую и левую точку коллайдера, на который прилетел джампер
    private (Vector3, Vector3) GetLeftRightPositionColliderPoints(Collider colliderLanding)
    {
        (Vector3, Vector3) tupleRightLeftPoints = (Vector3.zero, Vector3.zero);
        for (int i = 0; i < _collidersClass.Count; i++)
        {
            if (_collidersClass[i].Collider == colliderLanding)
            {
                tupleRightLeftPoints.Item1 = _collidersClass[i].PositionRightCollider;
                tupleRightLeftPoints.Item2 = _collidersClass[i].PositionLeftCollider;
                return tupleRightLeftPoints;
            }
        }
    
        return tupleRightLeftPoints;
    }

    public void OffAllColliders()
    {
        foreach (var colliderClass in _collidersClass)
        {
            colliderClass.Collider.enabled = false;
        }
    }
    

}
