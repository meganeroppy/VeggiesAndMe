using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
	
	private const int MaxHealth = 30;
	public static int curHealth{get; set;}
	
	
	private void Awake(){
		curHealth = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("HP:" + curHealth.ToString());
		
	}


	public void ReduceHealth(int val){
		curHealth -= val;
		
		if(curHealth <= 0){
			Die();
		}
	}

	private void Die(){
		Debug.Log("Die()");
	}
	
}
