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
	protected float minOffset = 1f;
	[SerializeField]
	protected float maxOffset = 3f;

	[SerializeField]
	protected float maxRot = 90f;

	[SerializeField]
	protected Transform generatePos;

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
	
		// choose a child 
		int seed = Random.Range(0, children.Length);
		GameObject obj = Instantiate(children[seed]) as GameObject;

		// set angle
		float rot = Random.Range(-maxRot, maxRot);
		this.transform.rotation = Quaternion.Euler (new Vector3(0f, 0f, rot));

		// set distance from center
		float offset = Random.Range(minOffset, maxOffset);
		generatePos.transform.localPosition = new Vector3(0f, offset, 0f);

		obj.transform.position = generatePos.position;
	}
	
	public void Quicken(){
		if(interval > interval_min){
			interval *= 0.92f;
		}

	}
}
