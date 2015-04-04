using UnityEngine;
using System.Collections;

public class BehindWall : MonoBehaviour {
	private bool exiting;
	// Use this for initialization
	void Start () {
		SpriteRenderer focus = (SpriteRenderer)this.GetComponent("SpriteRenderer");
		if (focus != null) {
			focus.color = new Color(focus.color.r, focus.color.g, focus.color.b, 1);
		} else {
			foreach (SpriteRenderer t in this.transform.GetComponentsInChildren<SpriteRenderer>()) {
				t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - Time.deltaTime);
				
			}
		}		
		exiting = true;
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.name.Equals("Player")) exiting = false;

	}
	void OnTriggerStay2D(Collider2D col) {

		if (col.name.Equals("Player")) {
			exiting = false;
			SpriteRenderer focus = (SpriteRenderer)this.GetComponent("SpriteRenderer");
			if (focus != null) {
				if (focus.color.a >= 0) {
					focus.color = new Color(focus.color.r, focus.color.g, focus.color.b, focus.color.a - Time.deltaTime);
				}
			} else {
				foreach (SpriteRenderer t in this.transform.GetComponentsInChildren<SpriteRenderer>()) {
					t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - Time.deltaTime);

				}
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
			if (focus != null) {
				if (focus.color.a <= 1) {
					focus.color = new Color(focus.color.r, focus.color.g, focus.color.b, focus.color.a + Time.deltaTime);
				}
			} else {
				foreach (SpriteRenderer t in this.transform.GetComponentsInChildren<SpriteRenderer>()) {
					t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a + Time.deltaTime);
					
				}
			}

		}
	}
}
