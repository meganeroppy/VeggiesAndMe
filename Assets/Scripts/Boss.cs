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
		Gone,
		Idling,
	};
	private  BossState curState;

	// reaction
	Dictionary<int, Vector3> punchDir = new Dictionary<int, Vector3>(){
		{0, Vector3.up},
		{1, Vector3.right},
		{2, Vector3.down},
		{3, Vector3.left},
	};

	// face
	private enum FaceStatus{
		Default,
		Damage,
		Wounded,
		Damage_Wounded,
		Lose,
	};
	Dictionary<FaceStatus, int> faces = new Dictionary<FaceStatus, int>(){
		{FaceStatus.Default, 0},
		{FaceStatus.Damage, 1},
		{FaceStatus.Wounded, 2},
		{FaceStatus.Damage_Wounded, 3},
		{FaceStatus.Lose, 4},
	};
	[SerializeField]
	private float damageFace_duration = 0.5f;
	private float faceChangeTimer = 0f;


	[SerializeField]
	private int MaxHealth = 10;//30;
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
	private GameObject[] sayingPrefab;
	private GameObject saying;
	[SerializeField]
	private Vector3 sayingOffset = new Vector3(-2f, 3f, -1f); 
	[SerializeField]
	private float greetDuration = 1.2f;
	private float greetTimer = 0f;


	// about emerging
	[SerializeField] 
	private Vector3 emergePosOffset = new Vector3(0f, 10f, 0f);
	private Vector3	defaultPos;
	[SerializeField]
	private float emergeDuration = 3f;

	// about attacking
	private Generator generator;

	//about visual
	private SpriteRenderer graphic;
	private SpriteRenderer face;

	[SerializeField]
	private Sprite[] facePattern;
	[SerializeField]
	private GameObject[] effect;
	
	private float effectTimer = 0f;
	
	// about escaping
	private bool startFadeOut;
	
	[SerializeField]
	private ScorePop scorePop;
	
	// se
	private  AudioSource myAudio;
	private  AudioSource myAudio_voice;
	[SerializeField]
	private AudioClip se_crash;
	[SerializeField]
	private AudioClip voice01;
	[SerializeField]
	private AudioClip voice02;
	[SerializeField]
	private AudioClip voice03;
	[SerializeField]
	private AudioClip se_escape;

	private void Awake(){
	
		// assign to variables
		generator = this.transform.GetChild(0).GetComponent<Generator>();
		graphic = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
		face = graphic.transform.GetChild(0).GetComponent<SpriteRenderer>();
		myAudio = this.GetComponent<AudioSource>();
		myAudio_voice = graphic.GetComponent<AudioSource>();

		// about emerging
		defaultPos = this.transform.position;
	}

	public void Init(){
	
		startFadeOut = false;
		curState = BossState.Idling;
		
		generator.enabled = false;
		face.sprite = facePattern[faces[ FaceStatus.Default]];
		
		if(saying != null){
			Destroy(saying);
		}

		// set heatlh 
		curhealth = MaxHealth;
		
		// about emerging : position
		this.transform.position = defaultPos + emergePosOffset;
		this.transform.DOMove(defaultPos, emergeDuration, false).SetEase(Ease.OutCirc)
			.OnComplete(delegate{
				curState = BossState.Greeting;
				timer = wait;
			});
			
				
		// about emerging : alpha
		face.material.DOFade(0f, 0f);
		graphic.material.DOFade(0f, 0f).OnComplete(delegate{
			graphic.material.DOFade(1f, emergeDuration).SetEase(Ease.InQuint).OnComplete(delegate{
				face.material.DOFade(1f, emergeDuration).SetEase(Ease.InOutBack);
			});
		});
		
	}


	private void Update(){
		if(timer > 0f){
			timer -= Time.deltaTime;
			return;
		}
		
		if( (curhealth <= MaxHealth / 2) && (curState == BossState.Attacking || curState == BossState.Dying) ){
			effectTimer += Time.deltaTime;
			if(effectTimer >= 0.25f){
				effectTimer = 0f;
			
				if(curState == BossState.Dying || (curState == BossState.Attacking && Random.Range(0, 2)==0)){
					MakeEffect();
				}
			}
		}

		switch(curState){
		case BossState.Emerging :
		break;
		case BossState.Greeting :

			if(saying == null){
				saying = Instantiate(sayingPrefab[0], graphic.transform.position + sayingOffset, graphic.transform.rotation) as GameObject;
				saying.transform.SetParent(this.transform);
				greetTimer = greetDuration;
				myAudio_voice.PlayOneShot(voice01);
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

			if(faceChangeTimer > 0f){
				faceChangeTimer -= Time.deltaTime;
				if(faceChangeTimer <= 0f){
					if(curhealth <= MaxHealth / 2){
						face.sprite = facePattern[faces[FaceStatus.Wounded]];
					}else{
						face.sprite = facePattern[faces[FaceStatus.Default]];
					}
				}
			}

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
		break;	
		case BossState.Dying :
			if(generator.enabled){
				generator.enabled = false;
			}
			
			if(startFadeOut){
				return;
			}
			
			startFadeOut = true;
			graphic.material.DOFade(0f, 5f).SetEase(Ease.InExpo).OnComplete(delegate{
				gameManager.DisplayResult(true, 1f);
				Destroy(saying.gameObject);
				curState = BossState.Gone;
			});
			face.material.DOFade(0f, 5f).SetEase(Ease.InExpo);
		break;	
		
		case BossState.Idling :
			break;
		}
	}

	public void ReduceHealth(int val=1){
		if(curState != BossState.Attacking || curhealth <= 0){
			return;
		}

		curhealth -= val;
		
		if(curhealth <= 0){
			GameManager.done = true;
			
			gameManager.AddScore(1500);
			curState = BossState.Dying;

			// voice
			myAudio_voice.PlayOneShot(voice03);

			//face
			face.sprite = facePattern[faces[FaceStatus.Lose]];
			
			// score pop
			ScorePop obj = Instantiate(scorePop).GetComponent<ScorePop>();
			obj.transform.position = transform.position + Vector3.up * 3;
			obj.SetText("1500", 5f);	
			
			if(saying == null){
				saying = Instantiate(sayingPrefab[2], graphic.transform.position + sayingOffset, graphic.transform.rotation) as GameObject;
				saying.transform.SetParent(this.transform);
			}

			timer = wait;
		}else{


			if(curhealth <= MaxHealth / 2){
				face.sprite = facePattern[faces[FaceStatus.Damage_Wounded]];
			}else{
				face.sprite = facePattern[faces[FaceStatus.Damage]];
			}
			faceChangeTimer = damageFace_duration;

			// reaction
			Vector3 dir = punchDir[Random.Range(0, 4)];
			float force = Random.Range(0.5f, 2.5f);
			this.transform.DOPunchPosition(dir * force, 0.1f, 100, 10f).OnComplete(delegate{
				this.transform.position = defaultPos;
			});
			
			// effect
			MakeEffect();
		}

		// se
		myAudio.PlayOneShot(se_crash);

	}

	private void MakeEffect(int num=1){
		for(int i = 0 ; i < num ; i++){
			Vector3 offset = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), -1f); 
			Instantiate(effect[0], graphic.transform.position + offset, Quaternion.identity);
			if(curState == BossState.Dying){
				offset = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), -1f); 
				int seed = Random.Range(1, effect.Length);
				Instantiate(effect[seed], graphic.transform.position + offset, Quaternion.identity);
				myAudio.PlayOneShot(se_crash);
				
			}
		}
	}

	public void Escape(){
		curState = BossState.Escaping;
		face.sprite = facePattern[faces[FaceStatus.Default]];
		timer = wait;
		
		if(saying == null){
			saying = Instantiate(sayingPrefab[1], graphic.transform.position + sayingOffset, graphic.transform.rotation) as GameObject;
			saying.transform.SetParent(this.transform);
		}
		
		// voice
		myAudio_voice.PlayOneShot(voice02);
		
		float waitTmp = wait * 2;
		
		graphic.material.DOFade(0f, emergeDuration).SetDelay(waitTmp);
		face.material.DOFade(0f, emergeDuration).SetDelay(waitTmp);
		
		this.transform.DOMove(defaultPos + emergePosOffset, emergeDuration).SetDelay(waitTmp).OnComplete(delegate{
			gameManager.DisplayResult(false, 1f);
			myAudio.PlayOneShot(se_escape);
			Destroy(saying.gameObject);
		});
	}
}
