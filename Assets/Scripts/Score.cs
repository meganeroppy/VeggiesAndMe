using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

	[SerializeField]
	protected Text text;
	
	protected void Update(){
		if(text != null){
		text.text = "Score:" + GameManager.score.ToString();
		}
	
	}

}
