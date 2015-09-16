using UnityEngine;
using System.Collections;

public class ParticleThing : MonoBehaviour {
	private float time;
	//particle that will move away and get smaller

	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime;
		float foo;
		if (time < (1/2.5f)) foo = 2.5f;
		else foo = 1/time;
		this.transform.localScale = new Vector3(foo, foo, 1);
		if (time > 2) {
			Destroy(this.gameObject);
		}
	}
}
