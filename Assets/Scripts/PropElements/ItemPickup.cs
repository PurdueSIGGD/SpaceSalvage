using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {
	//Items able to be grabbed

	public Transform bulletPrefab;
	public GameObject Player;
	private Vector3 delta;
	private Vector3 difference;
	public bool colliding;
	public bool YELLING;
	// Use this for initialization
	void Start () {
		colliding = false;
		Player = GameObject.Find("Player");

	}
	void OnCollisionEnter2D(Collision2D col) {
		colliding = true;
		if (Player != null && ((CraneController)Player.GetComponentInChildren<CraneController>()).grabbed) {
			if (col.collider.GetComponent("Rigidbody2D") != null) {
				((Rigidbody2D)col.collider.GetComponent("Rigidbody2D")).AddForce(40 * (col.transform.position - this.transform.position));
			}
			
			
		}

	}
	void OnCollisionExit2D(Collision2D col) {
		colliding = false;

	}

	// Update is called once per frame
	void Update () {
		delta = transform.position;
		if (YELLING) {
			print(delta);
		}


	}
}
