using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDifficle : MonoBehaviour
{

  public Generation gen;
  public float MaxDistantion = 1000;
  public float MaxIncreseBetweenObj = 1;

  private float TargetX;
  private float ScaleDiff;
  private float StartMin;
  private float StartMax;
  // Start is called before the first frame update
  void Start()
  {
    StartMin = gen.minDistance;
    StartMax = gen.maxDistance;
  }

  // Update is called once per frame
  void Update()
  {
    TargetX = Mathf.Abs(transform.position.x);
    if (TargetX > MaxDistantion)
      ScaleDiff = 1;
    else
      ScaleDiff = TargetX / MaxDistantion;
    gen.minDistance = ScaleDiff * MaxIncreseBetweenObj + StartMin;
    gen.maxDistance = ScaleDiff * MaxIncreseBetweenObj + StartMax;
  }
}
