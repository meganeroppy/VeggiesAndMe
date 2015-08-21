using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour
{

	[SerializeField]
	protected float
		interval = 0.5f;
	[SerializeField]
	protected float
		interval_max = 3f;
	[SerializeField]
	protected float
		interval_min = 0.15f;
	[SerializeField]
	protected float
		minOffset = 1f;
	[SerializeField]
	protected float
		maxOffset = 3f;

	[SerializeField]
	protected float
		maxRot = 90f;

	[SerializeField]
	protected Transform
		generatePos;

	[SerializeField]
	protected GameObject[]
		children;

	[SerializeField]
	protected GameObject
		badItem;
	
	[SerializeField]
	protected GameObject
		postponeItem;
	
	protected float geneTimer;
	
	protected AudioSource myAudio;
	[SerializeField]
	protected AudioClip
		clip;
	
	protected virtual void Awake ()
	{
		myAudio = GetComponent<AudioSource> ();
	}

	public void Init ()
	{
		interval = interval_max;
		geneTimer = interval;
	}
	
	protected virtual void Update ()
	{

		if (GameManager.time <= 0) {
			return;
		}
	
		if (geneTimer > interval) {
			geneTimer = 0f;
			Generate ();
		} else {
			geneTimer += Time.deltaTime;
		}
	}
	
	protected virtual void Generate ()
	{

		myAudio.PlayOneShot (clip);
	
		bool done = false;
		bool withSpecial = false;
		do {
			GameObject obj;
			// choose a child 
			
			int seed = 2;// withSpecial ? Random.Range(2, 36) : Random.Range(2, 36);
			switch (seed) {
			case 0:
				obj = Instantiate (badItem) as GameObject;
				withSpecial = true;
				break;
			case 1:
				obj = Instantiate (postponeItem) as GameObject;
				withSpecial = true;
				break;
			default:
				int seed2 = Random.Range (0, children.Length);
				obj = Instantiate (children [seed2]) as GameObject;
				done = true;
				break;
			}


			// set angle
			float rot = Random.Range (-maxRot, maxRot);
			this.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, rot));

			// set distance from center
			float offset = Random.Range (minOffset, maxOffset);
			generatePos.transform.localPosition = new Vector3 (0f, offset, 0f);

			obj.transform.position = generatePos.position;
		
		} while(!done);
	}
	
	public void Quicken ()
	{
		if (interval > interval_min) {
			interval *= 0.94f;			
		}
	}
}
