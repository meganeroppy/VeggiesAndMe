using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ScorePop : MonoBehaviour {
	[SerializeField]
	private float defLifeTime = 1.0f;
	private float timer = 0;
	[SerializeField]
	private float endOffset = 5f;
	
	private TextMesh text;
	
	private void Awake(){
		text = GetComponent<TextMesh>();
		timer = defLifeTime;
	}

	public void SetText(string str, float duration=1f){
		timer = duration;
		text.text = str;
	}

	// Use this for initialization
	void Start () {
		this.transform.DOMove(this.transform.position + Vector3.up * endOffset, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer <= 0 ){
		Destroy(this.gameObject);
		}
	}
}
