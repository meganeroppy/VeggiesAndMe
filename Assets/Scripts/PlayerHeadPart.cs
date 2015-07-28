using UnityEngine;
using System.Collections;

public class PlayerHeadPart : MonoBehaviour {

	[SerializeField]
	private GameManager game;
	private PlayerHead head;

	private void Awake(){
		head = transform.parent.GetComponent<PlayerHead>();
	}

	protected virtual void OnTriggerEnter(Collider col){
		head.Hit(col);
	}
}
