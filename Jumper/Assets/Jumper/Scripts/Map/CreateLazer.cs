using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Создание лазера около объекта, костыль
/// </summary>

public class CreateLazer : MonoBehaviour
{
   [Header("Префаб лазера")] public GameObject PrefabLazer;
   
   [Header("Вероятность создания лазера")]
   public int ProbabilityCreatingLaser;
   
   [Header("Позиция лазера по оси Z")] public float PositionLazerZ;

   [Header("Минимальная позиция по оси X")]
   public float MinPositionLazerX;
   [Header("Максимальная позиция по оси X")]
   public float MaxPositionLazerX;
   
   [Header("Минимальная позиция по оси X")]
   public float MinPositionLazerY;
   [Header("Максимальная позиция по оси X")]
   public float MaxPositionLazerY;
   
   
   // Проверка, можем/не можем создать объект по центру
   private bool CheckCreateLazer()
   {
      int randomNumber = Random.Range(0, 100);
      if (randomNumber <= ProbabilityCreatingLaser)
         return true;
      return false;
   }
   
   private void Start()
   {
      var positionLazer = new Vector3(Random.Range(MinPositionLazerX, MaxPositionLazerX), Random.Range(MinPositionLazerY, MaxPositionLazerY), PositionLazerZ);
      if (CheckCreateLazer())
      {
         GameObject newObject = Instantiate(PrefabLazer, transform, false);
         newObject.transform.localPosition = positionLazer;
      }
   }
}
