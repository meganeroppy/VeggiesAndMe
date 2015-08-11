using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Boss : MonoBehaviour {

	// status
	private enum BossState{
		Emerging,
		Greeting,
		Attacking,
		Escaping,
		Dying,
		Idling,
	};
	private  BossState curState = BossState.Idling;

	// reaction
	Dictionary<int, Vector3> punchDir = new Dictionary<int, Vector3>(){
		{0, Vector3.up},
		{1, Vector3.right},
		{2, Vector3.down},
		{3, Vector3.left},
	};

	[SerializeField]
	private int MaxHealth = 30;
	private  int curhealth = 0;

	// game manager
	[SerializeField]
	private GameManager gameManager;

	// wait shwn switch state
	[SerializeField]
	private  float wait = 0.7f;
	private  float timer = 0f;


	// about greeting
	[SerializeField]
	private GameObject sayingPrefab;
	private GameObject saying;
	[SerializeField]
	private Vector3 sayingOffset = new Vector3(-2f, 3f, -1f); 
	[SerializeField]
	private float greetDuration = 1.2f;
	private float greetTimer = 0f;

	// reaction
	[SerializeField]
	private GameObject spark;


	// about emerging
	[SerializeField] 
	private Vector3 emergePosOffset = new Vector3(0f, 10f, 0f);
	private Vector3	defaultPos;
	[SerializeField]
	private float emergeDuration = 3f;

	// about attacking
	private Generator generator;
	private SpriteRenderer graphic;

	// se
	private  AudioSource myAudio;
	[SerializeField]
	private AudioClip se_crash;

	private void Awake(){
		// assign to variables
		generator = this.transform.GetChild(0).GetComponent<Generator>();
		generator.enabled = false;
		graphic = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
		myAudio = this.GetComponent<AudioSource>();

		// about emerging
		defaultPos = this.transform.position;

		// set heatlh 
		curhealth = MaxHealth;
	}

	private void Start(){
		// about emerging : position
		this.transform.position = defaultPos + emergePosOffset;
		this.transform.DOMove(defaultPos, emergeDuration, false).SetEase(Ease.OutCirc)
			.OnComplete(delegate{
				curState = BossState.Greeting;
				timer = wait;
			});

		// about emerging : alpha
		graphic.material.DOFade(0f, 0f).OnComplete(delegate{
			graphic.material.DOFade(1f, emergeDuration).SetEase(Ease.InQuint);

		});


	}

	private void Update(){
		if(timer > 0f){
			timer -= Time.deltaTime;
			return;
		}

		switch(curState){
		case BossState.Emerging :
		break;
		case BossState.Greeting :

			if(saying == null){
				saying = Instantiate(sayingPrefab, graphic.transform.position + sayingOffset, graphic.transform.rotation) as GameObject;
				saying.transform.SetParent(this.transform);
				greetTimer = greetDuration;
			}

			if(greetTimer < 0f){
				Destroy(saying.gameObject);
				curState = BossState.Attacking;
				timer = wait;
			}else{
				greetTimer -= Time.deltaTime;
			}

			break;	
		case BossState.Attacking :

			if(!gameManager.started){
				gameManager.started = true;
			}

			if(!generator.enabled){
				generator.enabled = true;
			}
		break;	
		case BossState.Escaping :
			if(generator.enabled){
				generator.enabled = false;
			}

			if(saying == null){
				saying = Instantiate(sayingPrefab, graphic.transform.position + sayingOffset, graphic.transform.rotation) as GameObject;
				saying.transform.SetParent(this.transform);
			}

			this.transform.DOMove(this.transform.position + emergePosOffset, emergeDuration);
			graphic.DOFade(0f, emergeDuration);

		break;	
		case BossState.Dying :
			if(generator.enabled){
				generator.enabled = false;
			}

			graphic.DOFade(0f, 5f).OnComplete(delegate{
				gameManager.DisplayResult();
			});
		break;	
		case BossState.Idling :
			break;
		}
	}

	public void ReduceHealth(int val=1){
		if(curState != BossState.Attacking || curhealth <= 0){
			return;
		}

		// se
		myAudio.PlayOneShot(se_crash);

		// reaction
		Vector3 dir = punchDir[Random.Range(0, 4)];
		float force = Random.Range(0.5f, 2.5f);
		this.transform.DOPunchPosition(dir * force, 0.1f, 100, 10f).OnComplete(delegate{
			this.transform.position = defaultPos;
		});

		// effect
		/*
		Vector3 offset = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), -1f); 
		Quaternion rot = Quaternion.identity;
		Instantiate(spark, graphic.transform.position + offset, rot);
		*/

		curhealth -= val;

		if(curhealth <= 0){
			gameManager.done = true;
			curState = BossState.Dying;
			timer = wait;
		}
	}

	public void Escape(){
		curState = BossState.Escaping;
		timer = wait;
	}


}
