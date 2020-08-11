using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Панель, которая появляется во время проигрыша")]
    [SerializeField]
    private GameObject _panelGameOver = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Object")
        {
            print("GameOver");
            ClickTracking.GameOverPlayer = true;
            _panelGameOver.SetActive(true);
        }
    }
}
