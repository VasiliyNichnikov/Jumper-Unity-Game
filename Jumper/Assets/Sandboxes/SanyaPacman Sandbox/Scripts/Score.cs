using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text ScoreText;
    public Text TimeText;
    public int MultiplierTimeStep;
    public float TimeForObj;
    public int CapacityForLastObjects=10;
    int CurrentScore = 0;    
    float CurrTime=0f;
    public List<GameObject> lastObjects;

    private Vector3 ContactPoint;
    // Start is called before the first frame update
    void Start()
    {
        lastObjects = new List<GameObject>();
        RefreshTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrTime>0)        
            CurrTime -= Time.deltaTime;   
        
        TimeText.text =String.Format("время на объекте {0:f1}",CurrTime);
        RaycastHit hit;
        if (Physics.Raycast(transform.position,Vector3.down, out hit,Mathf.Infinity,9))//8 - номер слоя, который игнорирует игрока
        {
            var obj = hit.transform.gameObject;
            ContactPoint = hit.point;
            if (obj.tag!="Object")
            {
                return;
            }
            Debug.Log(obj);
            if (!lastObjects.Contains(obj))
            {
                lastObjects.Add(obj);
                RefreshTime();
                if (lastObjects.Count>CapacityForLastObjects)
                {
                    lastObjects.RemoveAt(0);
                }
            }
        }
    }

    private void RefreshTime()
    {
        CurrentScore += MultiplierTimeStep * (int)(CurrTime / MultiplierTimeStep);
        CurrTime = TimeForObj;        
        ScoreText.text = String.Format("Счет: {0}", CurrentScore);
        Debug.Log(CurrentScore);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Vector3.down);
        if (ContactPoint!=null)
            Gizmos.DrawWireSphere(ContactPoint, 1);
    }
}
