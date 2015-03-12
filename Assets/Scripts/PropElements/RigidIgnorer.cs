using UnityEngine;
using System.Collections;

public class RigidIgnorer : MonoBehaviour {
	private float time;
	// Use this for initialization
	void Start () {
		refresh_rigids();


	}
	void OnCollisionEnter2D(Collision2D col) {

	}
	void refresh_rigids() {
		Rigidbody2D[] rigids = GameObject.FindObjectsOfType<Rigidbody2D>();
		foreach (Rigidbody2D d in rigids) {
			if (d.GetComponent<RopeCrusher>() == null) { //add the exceptions here
				Physics2D.IgnoreCollision(d.collider2D, this.rigidbody2D.collider2D);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		time+= Time.deltaTime;

		if (time > 1) {
			refresh_rigids();
			time = 0;
		}
	}
	void IgnoreMe(Collider2D col) {
		Physics2D.IgnoreCollision(col, this.collider2D);
	}
	void RememberMe(Collider2D col) {
		Physics2D.IgnoreCollision(col, this.collider2D, false);
	}
}
