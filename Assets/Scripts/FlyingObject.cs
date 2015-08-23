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
	protected GameObject[] effect_die;
	
	[SerializeField]
	protected AudioClip se;
	
	protected bool rebounding = false;
	protected bool dying = false;

	private Vector3 offset = new Vector3(0f,-1f, -0.5f);

	[SerializeField]
	private ScorePop scorePop;
	
	protected virtual void Awake(){
		graphic = this.transform.GetChild(0).gameObject;
		myAudio = GetComponent<AudioSource>();
	}
	
	protected virtual void Update(){

		if(GameManager.done){
			Die(GameManager.time > 0);
			
			return;
		}

		float speed = mspd;

		if(!rebounding){
			graphic.transform.Rotate(rspd * Time.deltaTime);
		}else{
			speed *= 2.5f;
		}

		int dir = !rebounding ? -1 : 1;
		
		this.transform.Translate(0,0, speed * dir * Time.deltaTime);
		
	}
	
	protected virtual void OnTriggerEnter(Collider col){
		if(!col.tag.Equals("Player") && !col.tag.Equals("DeadZone") && !col.tag.Equals("Boss")){
			return;
		}
		
		if(col.tag.Equals("Player")){
			myAudio.PlayOneShot(se);
			Instantiate(effect_rebound, transform.position, transform.rotation);
			Rebound();

		}else{
			if(col.tag.Equals("Boss")){
				col.transform.parent.GetComponent<Boss>().ReduceHealth();
				Die();
			}else{
				Die(false);
				return;
			}
		}
	}

	public virtual void Die(bool withEffect=true){
		if(dying){
			return;
		}
		dying = true;

		if(withEffect){
			int seed = Random.Range(0,3);
			Instantiate(effect_die[seed], transform.position + offset, transform.rotation);
		}
		
		
		if(withEffect){
			// score pop
			ScorePop obj = Instantiate(scorePop).GetComponent<ScorePop>();
			obj.transform.position = transform.position;
			obj.SetText("100");		
		}
		
		
		graphic.SetActive(false);
		Destroy(this.gameObject, 0.5f);
	}
	
	public virtual void  Rebound(float force=1){
		rebounding =true;
	}
	

}
