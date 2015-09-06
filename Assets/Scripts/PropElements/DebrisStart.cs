using UnityEngine;
using System.Collections;
using System;

public class DebrisStart : MonoBehaviour {

	// Use this for initialization
	void Start () {

		this.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, this.transform.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25,25)));
		this.transform.GetComponent<Rigidbody2D>().AddTorque( this.transform.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25,25));
	}
	
	// Update is called once per frame
	void Update () {
		//destroy actor if out of range
	}
}
