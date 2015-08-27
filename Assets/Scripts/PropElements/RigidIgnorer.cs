using UnityEngine;
using System.Collections;
/* The purpose for this script is to 
 * ignore all rigidbody2D containing items
 * within the game. This is primarily intended for
 * ropescript joints, since they have been making
 * strange, large jumps while hitting any
 * free rigidbody2D objects. 
 * 
 * Not necessarily gameplay breaking, but may be bugfixed in the future.
 * */
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
				Physics2D.IgnoreCollision(d.GetComponent<Collider2D>(), this.GetComponent<Rigidbody2D>().GetComponent<Collider2D>());
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
		Physics2D.IgnoreCollision(col, this.GetComponent<Collider2D>());
	}
	void RememberMe(Collider2D col) {
		Physics2D.IgnoreCollision(col, this.GetComponent<Collider2D>(), false);
	}
}
