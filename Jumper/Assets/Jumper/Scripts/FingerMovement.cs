using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


/// <summary>
/// Данный скрипт создает линию движения пальца. 
/// </summary>
public class FingerMovement : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	private RawImage _rawImage = null;
	private float _slideWidth = .0f;
	private Transform _thisTransform = null;
	
	private void Start()
	{
		_rawImage = GetComponent<RawImage>();
		_slideWidth = GetComponent<RectTransform>().rect.width;
		_thisTransform = transform;
		ShowAndHideLine = false;
	}

	// Отображение и скрывание линии
	public bool ShowAndHideLine
	{
		set{_rawImage.gameObject.SetActive(value);}
	}
	
    public void UpdateLine(Vector3 startPositionFinger, Vector3 endPositionFinger)
    {
	    Vector3 centerPosition = (startPositionFinger + endPositionFinger) / 2f;
	    _thisTransform.position = centerPosition;
	    Vector3 direction = endPositionFinger - startPositionFinger;
	    direction = Vector3.Normalize(direction);
	    _thisTransform.right = direction;
	    float scale = Vector3.Distance(startPositionFinger, endPositionFinger) / _slideWidth;
	    _rawImage.uvRect = new Rect(0, 0, scale, 1);
	    _thisTransform.localScale = new Vector3(scale, 1, 1);
    }
}
