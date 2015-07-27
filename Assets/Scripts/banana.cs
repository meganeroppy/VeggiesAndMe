using UnityEngine;
using System.Collections;

public class banana : MonoBehaviour {
	
	Transform playerCam;
	// Use this for initialization
	void Start () {
		playerCam = GameObject.Find("Player").transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerCamRot =   playerCam.rotation.eulerAngles;
		Vector3 newRot = new Vector3(this.transform.rotation.x, playerCamRot.y, this.transform.rotation.z);
		this.transform.rotation = Quaternion.Euler( newRot );
		//this.transform.rotation = playerCam.rotation;


//		Debug.Log("banana rot:" + this.transform.rotation.eulerAngles + "playerCam rot:" + playerCam.rotation.eulerAngles);

	}
}
