using UnityEngine;
using System.Collections;

public class BehindWall : MonoBehaviour {
	private bool exiting;
	// Use this for initialization
	void Start () {
		exiting = false;
	}
	void OnTriggerEnter2D(Collider2D col) {
		exiting = false;

		//((SpriteRenderer)this.GetComponent("SpriteRenderer")).color = new Color(((SpriteRenderer)this.GetComponent("SpriteRenderer")).color.r, ((SpriteRenderer)this.GetComponent("SpriteRenderer")).color.g, ((SpriteRenderer)this.GetComponent("SpriteRenderer")).color.b, )
	}
	void OnTriggerStay2D(Collider2D col) {

		if (col.name.Equals("Player")) {
			exiting = false;
			SpriteRenderer focus = (SpriteRenderer)this.GetComponent("SpriteRenderer");
			if (focus.color.a >= 0) {
				focus.color = new Color(focus.color.r, focus.color.g, focus.color.b, focus.color.a - Time.deltaTime);
			}
		}
	}
	void OnTriggerExit2D(Collider2D col) {
		if (col.name.Equals ("Player")) {
			exiting = true;
		}
	}
	// Update is called once per frame
	void Update () {
		if (exiting) {
			SpriteRenderer focus = (SpriteRenderer)this.GetComponent("SpriteRenderer");
			focus.color = new Color(focus.color.r, focus.color.g, focus.color.b, focus.color.a + Time.deltaTime);

		}
	}
}
