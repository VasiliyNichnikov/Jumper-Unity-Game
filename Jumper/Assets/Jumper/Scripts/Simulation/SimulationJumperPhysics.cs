using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationJumperPhysics : MonoBehaviour
{
    [SerializeField] [Header("Префаб джампера")]
    private GameObject _jumperPrefab;

    [HideInInspector]
    public Transform TransformEnemyObject = null;
    
    public Vector3 SimulationJumper(Vector3 origin, Vector3 speed)
    {
        GameObject newJumper = Instantiate(_jumperPrefab, origin, Quaternion.identity);

        newJumper.GetComponent<Rigidbody>().AddForce(speed, ForceMode.Impulse);
        JumperAutoSimulation jumperAutoSimulation = newJumper.GetComponent<JumperAutoSimulation>();
        jumperAutoSimulation.SimulationJumperPhysics = this;

        Physics.autoSimulation = false;
        Vector3 endPosition = Vector3.zero;

        float pointNowY = .0f;
        for (int i = 1; i < 150; i++)
        {
            if (pointNowY > newJumper.transform.position.y)
            {
                newJumper.GetComponent<JumperAutoSimulation>().FoundPointMax = true;
            }
            else
                pointNowY = newJumper.transform.position.y;
            
            if (TransformEnemyObject != null)
            {
                endPosition = TransformEnemyObject.position;
                break;
            } 
            Physics.Simulate(0.02f);
        }
        Physics.autoSimulation = true;
        Destroy(newJumper.gameObject);
        TransformEnemyObject = null;
        
        return endPosition;
        
    }
}
