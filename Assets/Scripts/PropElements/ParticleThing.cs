using UnityEngine;
using System.Collections;

public class ParticleThing : MonoBehaviour {
	private float time;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime;
		this.transform.localScale = new Vector3(this.transform.localScale.x - Time.deltaTime/5, this.transform.localScale.y - Time.deltaTime/5, 1);
		if (time > 2) {
			Destroy(this.gameObject);
		}
	}
}
