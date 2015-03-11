using UnityEngine;
using System.Collections;

public class RigidIgnorer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Rigidbody2D[] rigids = GameObject.FindObjectsOfType<Rigidbody2D>();
		foreach (Rigidbody2D d in rigids) {
			if (d.GetComponent<RopeCrusher>() == null) { //add the exceptions here
				Physics2D.IgnoreCollision(d.collider2D, this.rigidbody2D.collider2D);
			}
		}


	}
	void OnCollisionEnter2D(Collision2D col) {

	}
	// Update is called once per frame
	void Update () {
	
	}
	void IgnoreMe(Collider2D col) {
		Physics2D.IgnoreCollision(col, this.collider2D);
	}
	void RememberMe(Collider2D col) {
		Physics2D.IgnoreCollision(col, this.collider2D, false);
	}
}
