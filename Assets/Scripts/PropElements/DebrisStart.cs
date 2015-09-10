using UnityEngine;
using System.Collections;
using System;

public class DebrisStart : MonoBehaviour {
	public float startVal = 1;
	// Use this for initialization
	void Start () {

		this.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, this.transform.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25 * startVal,25 * startVal)));
		this.transform.GetComponent<Rigidbody2D>().AddTorque( this.transform.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25 * startVal,25 * startVal));
	}
	
	// Update is called once per frame
	void Update () {
		//destroy actor if out of range
	}
}
