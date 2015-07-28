using UnityEngine;
using System.Collections;

public class PostPone : FlyingObject {

	private GameObject aura;
	
	protected override void Awake ()
	{
		base.Awake ();
		aura = transform.GetChild(1).gameObject;
	}
	
	public override void Die ()
	{
		aura.SetActive(false);
		base.Die ();
	}
}
