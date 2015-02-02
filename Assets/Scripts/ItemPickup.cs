using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {
	public Transform bulletPrefab;
	public GameObject Player;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		//Physics2D.IgnoreCollision(Player.collider2D, collider2D);	
	}
	void followSuit() {
		transform.position = ((CraneController)Player.GetComponentInChildren<CraneController>()).current;

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
		transform.position = ((CraneController)Player.GetComponentInChildren<CraneController>()).current;

	}
}
