using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private const float MaxTime = 35f;

	private static int _score = 0;
	private static float _time = 0f;
	
	public bool started{get; set;} // game is ongoing or not
	public bool done{get; set;} // game is done or not

	[SerializeField]
	private Generator generator;

	private void Awake(){
		Init();
		started = false;
		done = false;
	}


	private void Init(){
		score = 0;
		time = MaxTime;

		generator.Init();
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

		if(done){ 
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
				GameObject.Find("Boss").GetComponent<Boss_temp>().Escape();
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
	}
}
