using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildrenNumber : MonoBehaviour
{
    public Text txt;

    // Update is called once per frame
    void Update()
    {
        if (txt!= null)
            txt.text = transform.childCount.ToString();
    }
}
