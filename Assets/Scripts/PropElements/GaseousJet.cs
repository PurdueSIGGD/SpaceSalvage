using UnityEngine;
using System.Collections;

public class GaseousJet : MonoBehaviour {
	public float maxspeed = 20;
	public float force = 1;
	private Vector2 top;
	private Vector2 bottom;
	// Use this for initialization
	void Start () {
		top = transform.FindChild("Top").position;
		bottom = transform.FindChild("Bottom").position;
	}
	void OnTriggerStay2D(Collider2D col) {
		if (col.GetComponent<Rigidbody2D>() != null) {
			if (Vector2.SqrMagnitude(col.GetComponent<Rigidbody2D>().velocity) <= maxspeed) {
				col.GetComponent<Rigidbody2D>().AddForce(force * (top - bottom));
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
