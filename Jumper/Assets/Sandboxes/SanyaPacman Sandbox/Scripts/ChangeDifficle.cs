using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDifficle : MonoBehaviour
{
    
    public Generation gen;
    public float MaxDistantion=1000;
    public float MaxIncreseBetweenObj=1;

    private float TargetX;
    private float ScaleDiff;
    private float StartMin;
    private float StartMax;
    // Start is called before the first frame update
    void Start()
    {
        StartMin = gen.min_dist_between_pref;
        StartMax = gen.max_dist_between_pref;
    }

    // Update is called once per frame
    void Update()
    {
        TargetX = Mathf.Abs(transform.position.x);
        if (TargetX > MaxDistantion)
            ScaleDiff = 1;
        else
            ScaleDiff = TargetX/MaxDistantion;
        gen.min_dist_between_pref = ScaleDiff * MaxIncreseBetweenObj + StartMin;
        gen.max_dist_between_pref = ScaleDiff * MaxIncreseBetweenObj + StartMax;
    }
}
