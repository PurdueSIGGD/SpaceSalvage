using UnityEngine;
using System.Collections;
using System;

public class DebrisStart : MonoBehaviour {
	//For floaters and stuff
	public float startVal = 1;
	private Vector2 startF;
	private float startT;
	// Use this for initialization
	void Start () {
		//this.transform.ttag = "Floater";
		startF = new Vector2(0, this.transform.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25 * startVal,25 * startVal));
		startT = this.transform.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25 * startVal,25 * startVal);
		ReStart();
	}
	
	void ReStart() {
		this.transform.GetComponent<Rigidbody2D>().AddForce(startF);
		this.transform.GetComponent<Rigidbody2D>().AddTorque(startT);
	}
	void Stop() { //so you don't come back to objects and they are still
		startF = this.GetComponent<Rigidbody2D>().velocity;
		startT = this.GetComponent<Rigidbody2D>().angularVelocity;
	}
}
