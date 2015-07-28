using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

	[SerializeField]
	protected float interval = 0.5f;
	[SerializeField]
	protected float interval_max = 3f;
	[SerializeField]
	protected float interval_min = 0.15f;
	[SerializeField]
	protected Vector2 maxOffset = new Vector2(10f, 5f);
	
	
	[SerializeField]
	protected GameObject[] children;
	
	protected float timer;
	
	protected AudioSource myAudio;
	[SerializeField]
	protected AudioClip clip;
	
	protected virtual void Awake(){
		myAudio = GetComponent<AudioSource>();
		interval = interval_max;
		timer = interval;
	}
	
	protected virtual void Update(){
		
		if(GameManager.time <= 0){
			return;
		}
	
		if(timer > interval){
			timer = 0f;
			Generate();
		}else{
			timer += Time.deltaTime;
		}
	}
	
	protected virtual void Generate(){
	
		myAudio.PlayOneShot(clip);
	
		int seed = Random.Range(0, children.Length);
		GameObject obj = Instantiate(children[seed]) as GameObject;
		
		float offsetX = Random.Range(-maxOffset.x, maxOffset.x);
		float offsetY = Random.Range(-maxOffset.y, maxOffset.y);
		Vector3 offset = new Vector3(offsetX, offsetY, 0f);
		
		obj.transform.position = this.transform.position + offset;
		
	}
	
	public void Quicken(){
	Debug.Log("Quicken");
		if(interval > interval_min){
			interval *= 0.9f;
		}

	}
}
