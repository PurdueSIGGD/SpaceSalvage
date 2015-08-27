using UnityEngine;
using System.Collections;

public class RopeCrusher : MonoBehaviour {

	private bool isclosing;
	public GameObject partner;
	// Use this for initialization
	void Start () {
	}
	void OnCollisionEnter2D(Collision2D col) {

		if (col.gameObject.GetComponent<RigidIgnorer>() != null && isclosing) {
			col.collider.GetComponent<Rigidbody2D>().isKinematic = true;
			col.gameObject.SendMessage("IgnoreMe", this.GetComponent<Collider2D>());
		}



	}
	void OnCollisionExit2D(Collision2D col) {
		if (col.gameObject.GetComponent<RigidIgnorer>() != null) {
			col.collider.GetComponent<Rigidbody2D>().isKinematic = false;
			col.gameObject.SendMessage("RememberMe", this.GetComponent<Collider2D>());
		}
	}
	// Update is called once per frame
	void Update () {

	}
	void Opening() {
		isclosing = false;
	}
	void Closing() {
		isclosing = true;
	}
}
