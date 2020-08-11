using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortObjects : MonoBehaviour
{
    GameObject last_pref;
    public void bubbleSortHeigth(GameObject[] mas)
    {
        for (int i = 0; i < mas.Length-1; i++)        
            for (int j = i+1; j < mas.Length; j++)            
                if (mas[i].GetComponent<ObjectInfo>().sizeY> mas[j].GetComponent<ObjectInfo>().sizeY)
                {
                    var temp = mas[i];
                    mas[i] = mas[j];
                    mas[j] = temp;
                } 
    }
    public void bubbleSortWidth(GameObject[] mas)
    {
        for (int i = 0; i < mas.Length - 1; i++)
            for (int j = i + 1; j < mas.Length; j++)
                if (mas[i].GetComponent<ObjectInfo>().sizeX > mas[j].GetComponent<ObjectInfo>().sizeX)
                {
                    var temp = mas[i];
                    mas[i] = mas[j];
                    mas[j] = temp;
                }
    }
    GameObject[] objects;
    public void SortObjectByHeight()
    {
        objects = GameObject.FindGameObjectsWithTag("Object");
        RefreshPos();
        bubbleSortHeigth(objects);
        Spawn();
    }
    public void SortByWidth()
    {
        objects = GameObject.FindGameObjectsWithTag("Object");
        RefreshPos();
        bubbleSortWidth(objects);
        Spawn();
    }
    private void RefreshPos()
    {
        foreach (var item in objects)
        {
            item.transform.position = Vector3.zero;
        }
    }
    private void Spawn()
    {
        // last_pref = Instantiate(objects[0], transform, false); ;
        var last_pref = objects[0];
        last_pref.transform.position = Vector3.zero;
        for (int i = 1; i < objects.Length; i++)
        {
            var pre_last_pref = last_pref;
            last_pref = objects[i];
            //last_pref = Instantiate(obj, transform, false);
            var GOinfo = pre_last_pref.GetComponent<ObjectInfo>();
            // граница последнего объекта
            float x1 = GOinfo.transform.position.x - GOinfo.sizeX / 2;
            // расстояние между объектами
            float x2 = -1;
            GOinfo = last_pref.GetComponent<ObjectInfo>();
            float x3 = -Mathf.Abs(GOinfo.transform.position.x - GOinfo.sizeX / 2);
            last_pref.transform.position += new Vector3(x1 + x2 + x3, 0, 0);
        }
    }
    
}
