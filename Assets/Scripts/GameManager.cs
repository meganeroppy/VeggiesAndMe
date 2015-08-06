using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private const float MaxTime = 35f;

	private static int _score = 0;
	private static float _time = 0f;

	private static bool endOfGame = false;
	
	[SerializeField]
	private Generator generator;

	private void Awake(){
		Init();
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

		if(endOfGame){ 
			if(Input.anyKeyDown){
				Restart();
			}
		}

		// reduce limit time

		if(time > 0 ){
			time -= Time.deltaTime;
		}else{
			if(!endOfGame){
				endOfGame = true;
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
}
