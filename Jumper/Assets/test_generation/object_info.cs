using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_info : MonoBehaviour{
	[Header("тут высота и ширина обьекта в юнитах")]
	public float obj_height;
	public float obj_width;
	[Header("тут минимальное и максимальное расстояние")]//если 0,0 поставишь то должно строится в притык(можешь проверить правильность расстояния)
	public float min_dist;
	public float max_dist;
	[Header("на него можно ставить")]
	public bool big;
	[Header("его можно ставить")]
	public bool small;
	[Header("масив обьектов которые можно ставить")]//и зачем тогда bool small? //определенно не надо)
	public GameObject[] summon_obj;
	[Header("тут позиция х на обьекте если он big")]
	public float min_pos_x;
	public float max_pos_x;
	[Header("тут минимальная растояние summon_obj, если big")]
	public float min_dist_big;
	[Header("тут chanse_summon_mini, если big")]//чем больше тем меньше)
	public int chanse_summon_mini;
	[Header("минимальное растояние от начала")]
	public float min_dist_spawn;
	[HideInInspector]
	public GameObject hero;
	[Header("димтанция удаления !!!не меньше растояния до стены!!!")]
	public float dist_dell=10;
	
	void Start(){
		hero = GameObject.Find("Jumper");
		if(big){
			if(min_dist_big<transform.position.x){
				if(UnityEngine.Random.Range(0,chanse_summon_mini)==0){
					Instantiate(summon_obj[Random.Range(0,summon_obj.Length)], new Vector3(transform.position.x + Random.Range(min_pos_x,max_pos_x),transform.position.y+obj_height,transform.position.z) , Quaternion.identity);
				}
			}
		}
	}
	
	
}
