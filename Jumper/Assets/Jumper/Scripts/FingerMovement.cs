using System.Collections;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Данный скрипт создает движения пальца. 
/// </summary>
public class FingerMovement : MonoBehaviour
{
	struct GUILine
	{
		public Vector2 startPt;
		public Vector2 endPt;
	}

	private GUILine newline;
	private bool mouseDown;
	private bool mouseUp;
	private bool shift;
	private ArrayList lines;
	private float length;

	[SerializeField] [Header("Текстура линии")]
	private Texture _textureDragCircle = null;

	[SerializeField] [Header("Скрипт, который управляет джампером во время полета")]
	private FlightJumper _flightJumper = null;
	
	void Start ()
	{
		lines = new ArrayList (); 
	}
	
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			newline = new GUILine ();
			newline.startPt = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			//mouseDown = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			length = 0;
		}
	}

	void OnGUI ()
	{
		if (Input.GetMouseButton(0))
		{
			newline.endPt = new Vector2 (Input.mousePosition.x, Input.mousePosition.y); 
			//print(newline.endPt);
			SetLinePoints(newline);
			DrawLine(newline.startPt, newline.endPt);
		}
	}

	Vector2 setPoint (Vector2 point)
	{
		point.x = (int)point.x;
		point.y = Screen.height - (int)point.y;
		return point;
	}

	void SetLinePoints (GUILine line)
	{ 
		var averageSpeedX = _flightJumper.GetAverageSpeedJumper;
		if (averageSpeedX < 2)
		{
			line.startPt = setPoint (line.startPt);
			line.endPt = setPoint (line.endPt);
			length = (line.startPt - line.endPt).magnitude; 
		}
		
	}

	private void DrawLine (Vector2 pointA, Vector2 pointB)
	{
		pointA = setPoint(pointA);
		pointB = setPoint(pointB);
		Texture2D lineTex = new Texture2D(1, 1);
		float width = 30.0f; 	 	   	
		GUI.color = Color.red;  		
		float angle = Mathf.Atan2 (pointB.y - pointA.y, pointB.x - pointA.x) * 180f / Mathf.PI;

		GUIUtility.RotateAroundPivot (angle, pointA);
		GUI.DrawTexture (new Rect (pointA.x, pointA.y, width, width), _textureDragCircle);
		GUI.DrawTexture (new Rect (pointA.x, pointA.y, length, width), _textureDragCircle);
		
	}
    
}
