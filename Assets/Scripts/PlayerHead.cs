﻿using UnityEngine;
using System.Collections;

public class PlayerHead : MonoBehaviour {
	
	Transform playerCam;
	
	[SerializeField]
	//float maxAngleZ = 36f;
	
	// Use this for initialization
	void Start () {
		playerCam = GameObject.Find("Player").transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerCamRot = playerCam.rotation.eulerAngles;
		//Vector3 newRot = new Vector3(this.transform.rotation.x, playerCamRot.y, this.transform.rotation.z);
		
		Vector3 newRot = new Vector3(playerCamRot.x, playerCamRot.y, playerCamRot.z);
		
		//float z = Input.GetAxis("Horizontal") * -maxAngleZ;
		
		//this.transform.rotation = Quaternion.Euler(newRot.x, newRot.y, z);
		this.transform.rotation = Quaternion.Euler(0, 0, -newRot.y);
		//this.transform.rotation = playerCam.rotation;

//		Debug.Log("banana rot:" + this.transform.rotation.eulerAngles + "playerCam rot:" + playerCam.rotation.eulerAngles);

	}
}