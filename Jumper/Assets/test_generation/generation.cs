using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generation : MonoBehaviour{
	
	[Header("Игрок")]
	public GameObject hero;
	[HideInInspector]
	public Vector3 pos;
	[Header("Минимальное растояние между hero и pos")]//в основном сколько наперед спавнится 
	public float dist_go = 10;
	[Header("масив всех префабов которые можно ставить")]//поставь в порядке возростания возможности спавна от расстояния
	public GameObject[] summon_obj;
	[Header("масив шанса спавна префабов которые можно ставить")]//в суме не обязательно 100
	public int[] chanse_summon;
	
	[HideInInspector]
	public int max_random;
	[HideInInspector]
	public int random_value;
	[HideInInspector]
	public int number_summon;
	
	
	[Header("рекорд х игрока")]
	public float max_hero;
	[Header("стена за игроком чтоб он не мог ити назад")]
	public GameObject wall;
	[Header("расстояние до стены")]
	public float wall_dist = 5;
	
	
	void Start(){
		//test = chanse_summon.Length;
		
	}
	
	void FixedUpdate(){
		if(pos.x - hero.transform.position.x < dist_go){
			summon_new();
		}
		if(max_hero<hero.transform.position.x){
			max_hero=hero.transform.position.x;
		}
		wall.transform.position = new Vector3(max_hero-wall_dist,0,0); 
	}
	
	void summon_new(){
		max_random = 0;
		
		for(int i = 0; i < chanse_summon.Length; i++)
			max_random += chanse_summon[i];
		random_value = UnityEngine.Random.Range(0, max_random);
		
		int while_1 = 0;
		int value_1 = 0;
		
		while (value_1 <= random_value){
			value_1 += chanse_summon[while_1];
			while_1++;
		}
		number_summon = while_1-1;
		
		object_info obj_in = summon_obj[number_summon].GetComponent<object_info>();
		
		if(obj_in.min_dist_spawn<=pos.x){
			pos.x += UnityEngine.Random.Range( obj_in.min_dist , obj_in.max_dist );
			pos.x += obj_in.obj_width/2;
			Instantiate(summon_obj[number_summon], pos , Quaternion.identity);
			pos.x += obj_in.obj_width/2;
		}else{
			summon_new();
		}
		
	}
}
