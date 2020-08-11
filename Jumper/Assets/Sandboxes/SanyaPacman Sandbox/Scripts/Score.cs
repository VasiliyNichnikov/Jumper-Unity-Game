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
    
    
    public int CapacityForLastObjects=10;
    public float Distance;
    public int[] CoinReward;
    public float[] TimeSteps;
    int CurrentCoins = 0;    
    float CurrTime=0;
    List<GameObject> lastObjects;
    void Start()
    {
        lastObjects = new List<GameObject>();
        RefreshTime();
    }

    // Update is called once per frame
    void Update()
    {               
        CurrTime += Time.deltaTime;

        Distance = Mathf.Abs( transform.position.x);
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
                AddCoins();
                RefreshTime();
                if (lastObjects.Count>CapacityForLastObjects)                
                    lastObjects.RemoveAt(0);                
            }
        }

    }

    private void RefreshTime()
    {
        ScoreText.text = String.Format("Счет: {0}", CurrentCoins);
        CurrTime = 0;
    }
    private void AddCoins()
    {
        for (int i = 0; i < TimeSteps.Length - 2; i++)
        {
            Debug.Log(String.Format("время {0} {1} {2}", TimeSteps[i], CurrTime, TimeSteps[i + 1]));            
            if (CurrTime > TimeSteps[i] && CurrTime < TimeSteps[i + 1])
            {
                CurrentCoins += CoinReward[i];
                Debug.Log(String.Format("<color=red> заработано {0} коинов</color>", CoinReward[i]));
                return;
            }
        }
        CurrentCoins += CoinReward[CoinReward.Length - 1];       
        Debug.Log(String.Format("<color=green> заработано {0} коинов</color>", CoinReward[CoinReward.Length - 1]));        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Vector3.down);        
    }
}