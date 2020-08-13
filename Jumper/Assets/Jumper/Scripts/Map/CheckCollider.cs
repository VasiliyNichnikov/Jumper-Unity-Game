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
    
    [Header("Префаб шарика, который создается по краям")]
    public GameObject PrefabSpehere = null;

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

    public void CheckGameOver(Collision collision)
    {
         Vector3[] arrayPoints = GetLeftRightPositionColliderPoints(collision.collider);
         //print($"arrayPoints - {arrayPoints}");
         Vector3 positionRightCollider = arrayPoints[0];
         Vector3 positionLeftCollider = arrayPoints[1];

         _positionLandingPlayer = _camera.transform.InverseTransformPoint(collision.GetContact(0).point);
         
         //print($"Position Right Collider - {positionRightCollider}");
         //print($"Position Left Collider - {positionLeftCollider}");
         
         // print($"Check One - {Mathf.Abs(_positionLandingPlayer.x) > Mathf.Abs(positionRightCollider.x)}");
         // print($"Check Two - {Mathf.Abs(_positionLandingPlayer.x) < Mathf.Abs(positionLeftCollider.x)}");
         // print($"Position Landing Player - {Mathf.Abs(_positionLandingPlayer.x)}");
         // print($"Position Left Collider - {Mathf.Abs(positionLeftCollider.x)}");
         // print($"Position Right Collider - {Mathf.Abs(positionRightCollider.x)}");
         
        if (_positionLandingPlayer.x < positionRightCollider.x && // - 0.1f
            _positionLandingPlayer.x > positionLeftCollider.x) //+ 0.1f // _positionLandingPlayer.y > _positionCenterBounds.y &&
        {
            print("Player in the game;");
        }
        else
        {
            print("Game OVER");
            GameOverPlayer.GameOverPlayerMethod();
        } 
    }
    
    //private void OnCollisionEnter(Collision other)
    //{
        // if (!ClickTracking.GameOverPlayer && other.collider.tag == "Player")// && ClickTracking.JumpPlayer)
        // {
        //     //ClickTracking.JumpPlayer = false;
        //     //ClickTracking.JumpPlayer = false;
        //     ContactPoint contactPoint = other.contacts[0];
        //     Vector3[] arrayPoints = GetLeftRightPositionColliderPoints(contactPoint.thisCollider);
        //     if(arrayPoints == null)
        //         Debug.LogError("Ошибка, не найден коллайлер");
        //     else
        //     {
        //         Vector3 positionRightCollider = arrayPoints[0];
        //         Vector3 positionLeftCollider = arrayPoints[1];
        //
        //         _positionLandingPlayer = other.GetContact(0).point;
        //         
        //         if (_positionLandingPlayer.x < positionRightCollider.x && // - 0.1f
        //             _positionLandingPlayer.x > positionLeftCollider.x) //+ 0.1f // _positionLandingPlayer.y > _positionCenterBounds.y &&
        //         {
        //             print("Player in the game;");
        //         }
        //         else
        //         {
        //             GameOverPlayer.GameOverPlayerMethod();
        //         } 
        //     }
        // }
     // }
    
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
