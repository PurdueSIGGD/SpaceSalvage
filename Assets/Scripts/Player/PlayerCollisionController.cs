using UnityEngine;
using System.Collections;

public class PlayerCollisionController : MonoBehaviour {
	public float health = 100;
	public bool abletopickup = false;
	public bool pickup = false;
	private bool pickupkey = false;
	private bool lastkey = false;
		
	void OnCollisionStay2D(Collision2D col) {

	}
	void OnCollisionEnter2D(Collision2D col) {
		if (Vector3.Magnitude(col.relativeVelocity) > 4) {
			health -= Vector3.Magnitude(col.relativeVelocity);
			print ("Ow don't do that, I have " + health + " health left");
		}
		abletopickup = true;
		
	}
	void OnCollisionExit2D(Collision2D col) {
		abletopickup = false;
	}
	void OnTriggerEnter2D(Collider2D col) {
		//abletopickup = true;
	}
	void OnTriggerExit2D(Collider2D col) {
		//abletopickup = false;
	}
	// Use this for initialization
	void Start () {
		Physics2D.IgnoreLayerCollision(0,8);
	}
	
	// Update is called once per frame

	void Update () {
		pickupkey = Input.GetKey (KeyCode.F);

	}
	void FixedUpdate() {

		if (pickupkey != lastkey && abletopickup) { //toggle
			if (pickupkey) {
				//print (pickup);
				pickup = !pickup;

			} else {

			}
			lastkey = pickupkey;
		}

	}
}
