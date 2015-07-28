using UnityEngine;
using System.Collections;

public class PlayerHeadPart : MonoBehaviour {

	[SerializeField]
	private GameManager game;
	
	protected virtual void OnTriggerEnter(Collider col){
		if(!col.tag.Equals("FlyingObject")){
			return;
		}
		
		if(game != null){
			game.AddScore();
		}
	}
}
