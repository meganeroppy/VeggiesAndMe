using UnityEngine;
using System.Collections;

public class FlyingObject : MonoBehaviour {

	[SerializeField]
	protected float mspd = 10;
	
	[SerializeField]
	protected Vector3 rspd = new Vector3(0f, 10f, 0f);
	
	[SerializeField]
	protected GameObject[] fragments;
	
	protected GameObject graphic;
	
	protected virtual void Awake(){
		graphic = this.transform.GetChild(0).gameObject;
	}
	
	protected virtual void Update(){
		this.transform.Translate(0,0, -mspd * Time.deltaTime);
		graphic.transform.Rotate(rspd * Time.deltaTime); 
	}
	
	protected virtual void OnTriggerEnter(Collider col){
		if(!col.tag.Equals("Player") && !col.tag.Equals("DeadZone")){
			return;
		}
		if(col.tag.Equals("Player")){
			
		}
		Die();
		
	}

	public virtual void Die(){
		Destroy(this.gameObject);
	}
	

}
