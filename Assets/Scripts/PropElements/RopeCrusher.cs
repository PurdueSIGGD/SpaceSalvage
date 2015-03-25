using UnityEngine;
using System.Collections;

public class RopeCrusher : MonoBehaviour {

	private bool isclosing;
	public GameObject partner;
	private bool isclosed;
	// Use this for initialization
	void Start () {
	}
	void OnCollisionEnter2D(Collision2D col) {
		if (this.partner.Equals(col.gameObject)) {
			isclosed = true;
		}
		if (col.gameObject.GetComponent<RigidIgnorer>() != null && isclosing) {
			col.collider.rigidbody2D.isKinematic = true;
			col.gameObject.SendMessage("IgnoreMe", this.collider2D);
		}



	}
	void OnCollisionExit2D(Collision2D col) {
		if (col.gameObject.GetComponent<RigidIgnorer>() != null) {
			col.collider.rigidbody2D.isKinematic = false;
			col.gameObject.SendMessage("RememberMe", this.collider2D);
		}
	}
	// Update is called once per frame
	void Update () {
		/*if (isclosed) {
			this.rigidbody2D.isKinematic = true;
		} else {
			this.rigidbody2D.isKinematic = false;
		}*/
	}
	void Opening() {
		isclosing = false;
		isclosed = false;
	}
	void Closing() {
		isclosing = true;
	}
}
