using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text ScoreText;
    public Text TimeText;
    public Text DistanceText;
    public int CoinReward;
    public float TimeForObj;
    public int CapacityForLastObjects=10;
    public float Distance=0;
    int CurrentCoins = 0;    
    float CurrTime;
    public List<GameObject> lastObjects;

    Vector3 RayCenter;
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
        if (transform.position.x>0)
            Distance = transform.position.x;



        TimeText.text =String.Format("время на объекте {0:f1}",CurrTime);
        DistanceText.text = String.Format("Рекорд в расстоянии {0:f1}", Distance);
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position,Vector3.down, out hit, Mathf.Infinity, 9 ))
        {
            RayCenter = hit.point;
            var obj = hit.transform.gameObject;            
            if (obj.tag!="Object")            
                return;
            
            if (!lastObjects.Contains(obj))
            {
                lastObjects.Add(obj);
                RefreshTime();
                if (lastObjects.Count>CapacityForLastObjects)                
                    lastObjects.RemoveAt(0);                
            }
        }

    }

    private void RefreshTime()
    {
        if (CurrTime>0)
            CurrentCoins += CoinReward;
        else
            CurrentCoins += CoinReward/2;

        CurrTime = TimeForObj;        
        ScoreText.text = String.Format("Счет: {0}", CurrentCoins);
        Debug.Log(CurrentCoins);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Vector3.down);
        Gizmos.DrawWireSphere(RayCenter, 1);
    }
}
