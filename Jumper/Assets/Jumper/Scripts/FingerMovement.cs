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
		// if (Input.GetMouseButtonUp (0)) {
		// 	mouseDown = false;
		// 	mouseUp = true;
		// } 
		// if (Input.GetMouseButtonUp (1)) { 
		// 	if (lines.Count != 0)
		// 		lines.RemoveAt (lines.Count - 1); 
		// }  
		// if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) {
		// 	shift = true;
		// }
		// if (Input.GetKeyUp (KeyCode.LeftShift) || Input.GetKeyUp (KeyCode.RightShift)) {
		// 	shift = false;
		// }
	}

	void OnGUI ()
	{
		
		//print($"SpeedX - {speedX}; SpeedY - {speedY}");
		if (Input.GetMouseButton(0))
		{
			newline.endPt = new Vector2 (Input.mousePosition.x, Input.mousePosition.y); 
			//print(newline.endPt);
			SetLinePoints(newline);
			DrawLine(newline.startPt, newline.endPt);
		}
		
		// if (mouseDown) { 
		// 	newline.endPt = new Vector2 (Input.mousePosition.x, Input.mousePosition.y); 
		// 	// if (shift) { 
		// 	// 	if (difference (newline.endPt.x, newline.startPt.x) > difference (newline.endPt.y, newline.startPt.y)) {
		// 	// 		newline.endPt = new Vector2 (Input.mousePosition.x, newline.startPt.y); 
		// 	// 	} else {
		// 	// 		newline.endPt = new Vector2 (newline.startPt.x, Input.mousePosition.y); 
		// 	// 	}
		// 	// }
		// 	//DrawLine (newline.startPt, newline.endPt);
		// 	//addGUILine (newline);
		// 	//mouseUp = false; 			
		// 	
		// 	//int lineCnt = 0; 
		// 	
		// }
		// if (mouseUp) { 
		// 	addGUILine (newline);
		// 	mouseUp = false; 			
		// }
		// print(lines.Count);
		// foreach (GUILine line in lines)
		// {
		// 	setLinePoints(line);
		// 	DrawLine(line.startPt, line.endPt);
		// 	//lineCnt++;
		// }
	}

	Vector2 setPoint (Vector2 point)
	{
		point.x = (int)point.x;
		point.y = Screen.height - (int)point.y;
		return point;
	}

	// float difference (float val1, float val2)
	// {
	// 	float diff = val1 - val2;
	// 	if (diff < 0)
	// 		diff = -diff;
	// 	return diff;	
	// }
	//
	// void addGUILine (GUILine line)
	// {
	// 	lines.Add (line);
	// }
	//

	void SetLinePoints (GUILine line)
	{ 
		var averageSpeedX = _flightJumper.GetAverageSpeedJumper;
		//print($"Average speed - {averageSpeedX}");
		//var speedY = _flightJumper.GetSpeedJumperY;
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
		//Matrix4x4 matrixBackup = GUI.matrix;
		float width = 30.0f; 	 	   	
		GUI.color = Color.red;  		
		float angle = Mathf.Atan2 (pointB.y - pointA.y, pointB.x - pointA.x) * 180f / Mathf.PI;

		GUIUtility.RotateAroundPivot (angle, pointA);
		GUI.DrawTexture (new Rect (pointA.x, pointA.y, width, width), _textureDragCircle);
		GUI.DrawTexture (new Rect (pointA.x, pointA.y, length, width), _textureDragCircle);
		
		
		
		//GUI.DrawTexture (new Rect ((int)Input.mousePosition.x, Screen.height - (int)Input.mousePosition.y, width, width), _textureDragCircle);
		//print(length / 30);
		// print(pointA.x / 2);
		// print(pointA.y / 2);
		// int numX = Mathf.RoundToInt(pointA.x / 2);
		// int numY = Mathf.RoundToInt(pointA.y / 2);
		// float offsetX = 0;
		// float offsetY = 0;
		// print(numX);
		// print(numY);
		
		// for (int i = 0; i < 10; i++)
		// {
		// 	GUI.DrawTexture (new Rect (offsetX, offsetY, length, width), _textureDragCircle);
		// 	offsetX += numX;
		// 	offsetY += numY;
		// }
		//GUI.matrix = matrixBackup;  
	}
    
}
