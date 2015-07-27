using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

	[SerializeField]
	protected float generateInterval = 0.5f;
	
	[SerializeField]
	protected Vector2 maxOffset = new Vector2(10f, 5f);
	
	
	[SerializeField]
	protected GameObject[] children;
	
	protected float timer = 0f;
	
	protected virtual void Update(){
	
		
		if(GameManager.time <= 0){
			return;
		}
	
		if(timer > generateInterval){
			timer = 0f;
			Generate();
		}else{
			timer += Time.deltaTime;
		}
	}
	
	protected virtual void Generate(){
	
		int seed = Random.Range(0, children.Length);
		GameObject obj = Instantiate(children[seed]) as GameObject;
		
		float offsetX = Random.Range(-maxOffset.x, maxOffset.x);
		float offsetY = Random.Range(-maxOffset.y, maxOffset.y);
		Vector3 offset = new Vector3(offsetX, offsetY, 0f);
		
		obj.transform.position = this.transform.position + offset;
		
	}
}
