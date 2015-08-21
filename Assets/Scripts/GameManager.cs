using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

	[SerializeField]
	private float	MaxTime = 35f;

	private static int _score = 0;
	private static float _time = 0f;
	
	public bool started{ get; set; } // game is ongoing or not
	public static bool done{ get; set; } // game is done or not
	public bool readyToRestart{ get; set; } // displaying result is finished
	private bool hurryup = false;

	[SerializeField]
	private Generator
		generator;
	
	[SerializeField]
	private Boss
		boss;
	
	[SerializeField]
	private Score
		scoreTextPrefab;
	private Score scoreText;

	private AudioSource myAudio;
	
	private void Awake ()
	{
		myAudio = GetComponent<AudioSource>();
		Init ();
	}


	private void Init ()
	{
		score = 0;
		
		time = MaxTime;
		started = false;
		done = false;
		readyToRestart = false;
		hurryup = false;

		if (scoreText != null) {
			Destroy (scoreText.gameObject);
		}

		generator.Init ();
		boss.Init ();

		myAudio.pitch = 1.0f;

	}
	
	public static int score {
		get {
			return _score;
		}
		
		set {
			_score = value;
		}
	}
	
	public static float time {
		get {
			return _time;
		}
		
		set {
			_time = value;
		}
	}
	
	private void Update ()
	{

		if (readyToRestart) { 
			if (Input.anyKeyDown) {
				Restart ();
			}
		}

		if (!started || done) {
			return;
		}

		// reduce limit time
		if (time > 0) {
			time -= Time.deltaTime;

			if(time < 10f){
				myAudio.pitch = 1.15f;
			}
		} else {
			if (!done) {
				// game over 
				GameObject.Find ("Boss").GetComponent<Boss> ().Escape ();
				done = true;
			}
		}
	}
	
	public void AddScore (int val=1)
	{
		score += val;
		
		if (generator != null) {
			generator.Quicken ();
//			myAudio.pitch *= 1.0005f;
		}
	}

	public void ReduceScore (int val=1)
	{
		score -= val;
	}
	
	public void AddTime (float val=1f)
	{
		time += val;
	}


	private void Restart ()
	{
		Init ();
	}

	public void DisplayResult (bool clear=false)
	{
	
		Debug.Log ("DisplayResult");
		
		Vector3 offset = new Vector3 (0f, 5f, 20f);

		int clearTime = clear ? Mathf.FloorToInt (MaxTime - time) : 0;
		int restTime = clear ? Mathf.FloorToInt (time) : 0;
		int totalScore = clear ? score + restTime * 10 : score;

		scoreText = Instantiate (scoreTextPrefab).GetComponent<Score> ();
		scoreText.transform.position = this.transform.position = offset;
		scoreText.text =
			(clear ? "clear time\n" + clearTime.ToString () + " = " + (restTime * 10).ToString () + "pt\n" : "") 
			+ "your score\n" + totalScore + "pt";
		scoreText.transform.DOMove (new Vector3 (0f, 0f, 20f), 5f).OnComplete (delegate {
			readyToRestart = true;
		});
		
	}
}
