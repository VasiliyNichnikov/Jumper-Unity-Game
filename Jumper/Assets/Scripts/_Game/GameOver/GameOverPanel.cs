using System;    
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] [Header("Скорость анимации")] [Range(0, 100)]
    private float _speedAnimationPanel = .0f;
    private RectTransform _rectTransform = null;
    
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public IEnumerator AnimationPanel()
    {
        while (true)
        {
            _rectTransform.offsetMin =
                new Vector2(Mathf.Lerp(_rectTransform.offsetMin.x, 0, _speedAnimationPanel * Time.deltaTime), 0);
            _rectTransform.offsetMax =
                new Vector2(Mathf.Lerp(_rectTransform.offsetMax.x, 0, _speedAnimationPanel * Time.deltaTime), 0);
            yield return null;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Jumper and Generation Test");
    }
    
    
}
