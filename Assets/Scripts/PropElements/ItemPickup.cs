using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {
	public Transform bulletPrefab;
	public GameObject Player;
	private Vector3 delta;
	private Vector3 difference;
	public bool colliding;
	// Use this for initialization
	void Start () {
		colliding = false;
		Player = GameObject.Find("Player");
		//4i;	
	}
	void OnCollisionEnter2D(Collision2D col) {
		colliding = true;
		if (((CraneController)Player.GetComponentInChildren<CraneController>()).grabbed) {
			//print (40 * (this.transform.position - delta));
			if (col.collider.GetComponent("Rigidbody2D") != null) {
				((Rigidbody2D)col.collider.GetComponent("Rigidbody2D")).AddForce(40 * (col.transform.position - this.transform.position));
			}
			
			
		}

	}
	void OnCollisionExit2D(Collision2D col) {
		colliding = false;

	}
	/*void OnCollisionStay2D(Collision2D col) {
		if (col.collider.GetComponent("PlayerCollisionController") != null) {

			if (((PlayerCollisionController)col.collider.GetComponent("PlayerCollisionController")).pickup) {
				//this.transform.rigidbody2D.AddForce(2 * (Vector2)(new Vector3(col.transform.position.x, col.transform.position.y, col.transform.position.z) - this.transform.position)); //Float to a direct place
				this.transform.rigidbody2D.velocity = ((Rigidbody2D)col.collider.GetComponent("Rigidbody2D")).velocity; //Stick on
				((PlayerMovement)col.collider.GetComponent("PlayerMovement")).moverate = 2f;	
			}
			else {
				((PlayerMovement)col.collider.GetComponent("PlayerMovement")).moverate = 1f;

			}
		}

	}*/

	// Update is called once per frame
	void Update () {
		delta = transform.position;
		if (((CraneController)Player.GetComponentInChildren<CraneController>()).grabbed) {




		}/*


			if (!ended) {
				Physics2D.IgnoreCollision(Player.collider2D, collider2D);
				difference = transform.position - ((CraneController)Player.GetComponentInChildren<CraneController>()).current;
			}
			delta = transform.position;
			//print (((CraneController)Player.GetComponentInChildren<CraneController>()).current + difference + "        " + transform.position);
			transform.position = ((CraneController)Player.GetComponentInChildren<CraneController>()).current + difference;
			ended = true;

		} else {
			if (ended) {
				Physics2D.IgnoreCollision(Player.collider2D, collider2D, false);
				this.rigidbody2D.velocity = 60 * (transform.position - delta);
				ended = false;
			}
		} */

	}
}
