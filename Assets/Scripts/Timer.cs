using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {
	
	[SerializeField]
	protected Text text;
	
	protected void Update(){
		if(text != null){
			float val = Mathf.Floor( GameManager.time );
			if(val < 0){
				val = 0;
			}
			text.text = "Time:" +  val.ToString();
		}
		
	}
	
}
