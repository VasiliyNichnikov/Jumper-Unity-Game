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
    public float Distance;
    int CurrentCoins = 0;    
    float CurrTime;
    public List<GameObject> lastObjects;

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

        Distance = transform.position.x;
        TimeText.text =String.Format("время на объекте {0:f1}",CurrTime);
        DistanceText.text = String.Format("Рекорд в расстоянии {0:f1}", Distance);
        RaycastHit hit;
        if (Physics.Raycast(transform.position,Vector3.down, out hit))
        {
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
        //Gizmos.DrawWireSphere(transform.position, 1);
    }
}
