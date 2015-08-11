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
	protected GameObject effect_rebound;
		[SerializeField]
	protected GameObject effect_die;
	
	[SerializeField]
	protected AudioClip se;
	
	protected bool rebounding = false;
	
	protected virtual void Awake(){
		graphic = this.transform.GetChild(0).gameObject;
		myAudio = GetComponent<AudioSource>();
	}
	
	protected virtual void Update(){
		graphic.transform.Rotate(rspd * Time.deltaTime); 
		
		int dir = !rebounding ? -1 : 1;
		
		this.transform.Translate(0,0, mspd * dir * Time.deltaTime);
		
	}
	
	protected virtual void OnTriggerEnter(Collider col){
		if(!col.tag.Equals("Player") && !col.tag.Equals("DeadZone") && !col.tag.Equals("Boss")){
			return;
		}
		
		if(col.tag.Equals("Player")){
			myAudio.PlayOneShot(se);
			Instantiate(effect_rebound, transform.position, transform.rotation);
		}else{
			if(col.tag.Equals("Boss")){
				Instantiate(effect_die, transform.position, transform.rotation);
				col.transform.parent.GetComponent<Boss>().ReduceHealth();
			}
			Die();
			return;
		}
		Rebound();
		
	}

	public virtual void Die(){
	
		graphic.SetActive(false);
		Destroy(this.gameObject, 0.5f);
	}
	
	public virtual void  Rebound(float force=1){
		rebounding =true;
		
	}
	

}
