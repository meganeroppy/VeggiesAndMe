using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

	[SerializeField]
	private float LifeTime = 2.0f;
	private float timer = 0;
	
	private void Update(){
	if(timer >= LifeTime){
			Destroy(this.gameObject);
	}else{
	timer += Time.deltaTime;
	}
	}
}
