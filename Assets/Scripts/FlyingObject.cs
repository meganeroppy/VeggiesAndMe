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
	
	protected AudioSource myAudio;
	
	[SerializeField]
	protected GameObject effect;
	
	[SerializeField]
	protected AudioClip se;
	
	protected virtual void Awake(){
		graphic = this.transform.GetChild(0).gameObject;
		myAudio = GetComponent<AudioSource>();
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
			myAudio.PlayOneShot(se);
			GameObject obj = Instantiate(effect, transform.position, transform.rotation) as GameObject;
			obj.transform.SetParent(this.transform);
		}else{
		//	GameManager.score--;
		}
		Die();
		
	}

	public virtual void Die(){
	
		graphic.SetActive(false);
		Destroy(this.gameObject, 0.25f);
	}
	

}
