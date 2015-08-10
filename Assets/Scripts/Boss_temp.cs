using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Boss_temp : MonoBehaviour {

	// status
	private enum BossState{
		Emerging,
		Greeting,
		Attacking,
		Escaping,
		Dying,
		Idling,
	};
	private BossState curState = BossState.Idling;

	// game manager
	[SerializeField]
	private GameManager gameManager;

	// wait shwn switch state
	[SerializeField]
	private float wait = 0.7f;
	private float timer = 0f;


	// about greeting
	[SerializeField]
	private GameObject sayingPrefab;
	private GameObject saying;
	[SerializeField]
	private Vector3 sayingOffset = new Vector3(-2f, 3f, -1f); 
	[SerializeField]
	private float greetDuration = 2f;
	private float greetTimer = 0f;


	// about emerging
	[SerializeField] 
	private Vector3 emergePosOffset = new Vector3(0f, 10f, 0f);
	private Vector3	defaultPos;
	[SerializeField]
	private float emergeDuration = 3f;

	// about attacking
	private Generator generator;
	private SpriteRenderer graphic;

	private void Awake(){
		// assign to variables
		generator = this.transform.GetChild(0).GetComponent<Generator>();
		generator.enabled = false;
		graphic = this.transform.GetChild(1).GetComponent<SpriteRenderer>();

		// about emerging
		defaultPos = this.transform.position;
	}

	private void Start(){
		// about emerging : position
		this.transform.position = defaultPos + emergePosOffset;
		this.transform.DOMove(defaultPos, emergeDuration, false)
			.OnComplete(delegate{
				curState = BossState.Greeting;
				timer = wait;
			});

		// about emerging : alpha
		graphic.material.DOFade(0f, 0f).OnComplete(delegate{
			graphic.material.DOFade(1f, emergeDuration).SetEase(Ease.InExpo);

		});


	}

	private void Update(){
		switch(curState){
		case BossState.Emerging :
		break;
		case BossState.Greeting :

			if(timer > 0f){
				timer -= Time.deltaTime;
				return;
			}

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

			if(timer > 0f){
				timer -= Time.deltaTime;
				return;
			}

			if(!gameManager.started){
				gameManager.started = true;
			}

			if(!generator.enabled){
				generator.enabled = true;
			}
		break;	
		case BossState.Escaping :
		break;	
		case BossState.Dying :
		break;	
		case BossState.Idling :
			break;
		}
	}



}
