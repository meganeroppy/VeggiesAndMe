using UnityEngine;
using System.Collections;

public class Mush : FlyingObject {

	private GameObject aura;

	protected override void Awake ()
	{
		base.Awake ();
		aura = transform.GetChild(1).gameObject;
	}

	public override void Die (bool withEffect=true)
	{
		aura.SetActive(false);
		base.Die (withEffect);
	}
}
