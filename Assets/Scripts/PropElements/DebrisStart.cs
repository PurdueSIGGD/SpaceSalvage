using UnityEngine;
using System.Collections;
using System;

public class DebrisStart : MonoBehaviour {

	// Use this for initialization
	void Start () {

		this.transform.rigidbody2D.AddForce(new Vector2(0, UnityEngine.Random.Range(-25,25)));
		this.transform.rigidbody2D.AddTorque(UnityEngine.Random.Range(-25,25));
	}
	
	// Update is called once per frame
	void Update () {
		//destroy actor if out of range
	}
}
