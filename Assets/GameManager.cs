using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private static int _score = 0;
	private static float _time = 30f;
	
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
		if(time > 0 ){
			time -= Time.deltaTime;
		}
	}
}
