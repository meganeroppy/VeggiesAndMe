using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour {

	private const float MaxTime = 35f;

	private static int _score = 0;
	private static float _time = 0f;
	
	public bool started{get; set;} // game is ongoing or not
	public bool done{get; set;} // game is done or not
	public bool readyToRestart{get; set;} // displaying result is finished
	
	[SerializeField]
	private Generator generator;
	
	[SerializeField]
	private Boss boss;
	
	[SerializeField]
	private Score scoreTextPrefab;
	private Score scoreText;
	
	private void Awake(){
		Init();
	}


	private void Init(){
		score = 0;
		
		time = MaxTime;
		started = false;
		done = false;
		readyToRestart = false;
		
		if(scoreText != null){
			Destroy( scoreText.gameObject);
		}

		generator.Init();
		boss.Init();
	}
	
	public static int score{
		get{
			return _score;
		}
		
		set{
			_score = value;
		}
	}
	
	public static float time{
		get{
			return _time;
		}
		
		set{
			_time = value;
		}
	}
	
	private void Update(){

		if(readyToRestart){ 
			if(Input.anyKeyDown){
				Restart();
			}
		}

		if(!started || done){
			return;
		}

		// reduce limit time
		if(time > 0 ){
			time -= Time.deltaTime;
		}else{
			if(!done){
				// game over 
				GameObject.Find("Boss").GetComponent<Boss>().Escape();
				done = true;
			}
		}
	}
	
	public void AddScore(int val=1){
		score += val;
		
		if(generator != null){
			generator.Quicken();
		}
	}

	public void ReduceScore(int val=1){
		score -= val;
	}
	
	public void AddTime(float val=1f){
		time += val;
	}


	private void Restart(){
		Init();
	}

	public void DisplayResult(){
	
		Debug.Log("DisplayResult");
		
		
		Vector3 offset = new Vector3(0f, 5f, 5f);
		
		scoreText = Instantiate(scoreTextPrefab).GetComponent<Score>();
		scoreText.transform.position = this.transform.position = offset;
		scoreText.text = "your score\n" + score;
		scoreText.transform.DOMove(new Vector3(0f, 0f, 5f), 5f).OnComplete(delegate{
			readyToRestart = true;
		});
		
	}
}
