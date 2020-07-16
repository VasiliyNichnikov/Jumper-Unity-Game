using System;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [Header("Джампер, который мы запускаем")]
    public GameObject JumperPrefab = null;
    
    [HideInInspector] // объект куда приземлился джампер
    public GameObject ObjectLandingJumper = null;
    
    [HideInInspector]
    public bool CheckJumperStop = false; // Проверяем сталкнулся джампер или нет
    
    private LineRenderer _lineRenderer = null;

    [SerializeField] [Header("Скрипт, который хранит все объекты на карте")]
    private ListModelsTest _listModelsTest = null;
    
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if(ObjectLandingJumper != null)
            Gizmos.DrawWireMesh(ObjectLandingJumper.GetComponent<MeshFilter>().sharedMesh, ObjectLandingJumper.transform.position, ObjectLandingJumper.transform.rotation);
    }

    // Данный метод отображает траекторию полтета джампера
    public Vector3 ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        GameObject newJumper = Instantiate(JumperPrefab, origin, Quaternion.identity);
        Vector3[] points = new Vector3[150];
        //_lineRenderer.positionCount = points.Length;
        
        points[0] = newJumper.transform.position;
        newJumper.GetComponent<Rigidbody>().AddForce(speed, ForceMode.Impulse);
        JumperAutoSimulation jumperAutoSimulation = newJumper.GetComponent<JumperAutoSimulation>();
        jumperAutoSimulation.Trajectory = this;

        Physics.autoSimulation = false;
        Vector3 endPosition = Vector3.zero;

        float pointNowY = .0f;
        for (int i = 1; i < points.Length; i++)
        {
            if (pointNowY > newJumper.transform.position.y)
            {
                newJumper.GetComponent<JumperAutoSimulation>().FoundPointMax = true;
            }
            else
                pointNowY = newJumper.transform.position.y;
            
            if (CheckJumperStop)
            {
                //_lineRenderer.positionCount = i;
                Transform objectNearby = _listModelsTest.GetEmptyTransform(ObjectLandingJumper);
                if(objectNearby != null)
                    endPosition = _listModelsTest.GetEmptyTransform(ObjectLandingJumper).position;
                break;
            }
                
            Physics.Simulate(0.02f);
            points[i] = newJumper.transform.position;
        }
        //_lineRenderer.SetPositions(points);
        Physics.autoSimulation = true;
        Destroy(newJumper.gameObject);
        CheckJumperStop = false;
        
        return endPosition;
        
    }

}
