using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Score : MonoBehaviour {
	
	[SerializeField]
	protected TextMesh textMesh;
	
	public string text{
		set{
			textMesh.text = value;
		}
	}
	/*
	protected void Update(){
		
		if(textMesh != null){
			textMesh.text = "your score\n" + GameManager.score.ToString();
		}
	
	}
	*/
}
