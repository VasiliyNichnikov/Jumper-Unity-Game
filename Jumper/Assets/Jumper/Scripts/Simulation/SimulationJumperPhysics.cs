using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationJumperPhysics : MonoBehaviour
{
    [SerializeField] [Header("Префаб джампера")]
    private GameObject _prefabJumper = null;
    
    [HideInInspector]
    public Transform TransformEmptyObject = null;

    public Dictionary<string, float> SimulationJumper(Vector3 origin, Vector3 speed)
    {
        Dictionary<string, float> dictionaryDifferenceAndDistance = new Dictionary<string, float>();
        
        GameObject newJumper = Instantiate(_prefabJumper, origin, Quaternion.identity);

        var endPositionJumper = Vector3.zero;
        var flightTimeJumper = .0f;
        
        newJumper.GetComponent<Rigidbody>().AddForce(speed, ForceMode.Impulse);
        JumperAutoSimulation jumperAutoSimulation = newJumper.GetComponent<JumperAutoSimulation>();
        jumperAutoSimulation.SimulationJumperPhysics = this;

        Physics.autoSimulation = false;
        Vector3 endPosition = Vector3.zero;
        
        float pointNowY = .0f;
        for (int i = 1; i < 150; i++)
        {
            flightTimeJumper += Time.fixedDeltaTime;
            if (pointNowY > newJumper.transform.position.y)
            {
                newJumper.GetComponent<JumperAutoSimulation>().FoundPointMax = true;
            }
            else
                pointNowY = newJumper.transform.position.y;
            
            if (TransformEmptyObject != null)
            {
                endPosition = TransformEmptyObject.position;
                break;
            } 
            Physics.Simulate(0.02f);
        }
        Physics.autoSimulation = true;
        endPositionJumper = newJumper.transform.position;
        Destroy(newJumper.gameObject);
        TransformEmptyObject = null;
        
        dictionaryDifferenceAndDistance["speed_rotation_jumper_flight"] =
            Mathf.Abs(endPositionJumper.x - origin.x) / flightTimeJumper;
        dictionaryDifferenceAndDistance["difference_distance_axes_x"] = Mathf.Abs(endPositionJumper.x - origin.x);
        dictionaryDifferenceAndDistance["finite_distance_axes_y"] = endPosition.y;
        
        return dictionaryDifferenceAndDistance;
        
    }
}
