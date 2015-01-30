using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerStay2D(Collider2D col) {
		if (col.GetComponent("PlayerCollisionController") != null) {
			if (((PlayerCollisionController)col.GetComponent("PlayerCollisionController")).pickup) {

				this.transform.rigidbody2D.velocity = ((Rigidbody2D)col.GetComponent("Rigidbody2D")).velocity;
				((PlayerMovement)col.GetComponent("PlayerMovement")).moverate = 2f;
			}
			else {
				((PlayerMovement)col.GetComponent("PlayerMovement")).moverate = 1f;
			}
		}

	}

	// Update is called once per frame
	void Update () {

	}
}
