using UnityEngine;
using System.Collections;

public class ParticleThing : MonoBehaviour {
	private float time;
	public float distanceLasting = 2.5f;
	private Vector3 startScale; 
	//particle that will move away and get smaller
	void Start() {
		startScale = this.transform.localScale;
	}
	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime;
		float foo;
		if (time < (1/distanceLasting)) foo = startScale.x * distanceLasting;
		else foo = startScale.x/time;
		this.transform.localScale = new Vector3(foo, foo, 1);
		if (foo <= .1f) {
			Destroy(this.gameObject);
		}
	}
}
