using UnityEngine;
using System.Collections;

public class PlayerCollisionController : MonoBehaviour {
	public float health = 100;
	public float startingoxy = 60;
	public float oxy;
	private GameObject text;
		
	public int wallet = 0;
	void OnCollisionStay2D(Collision2D col) {

	}
	void OnCollisionEnter2D(Collision2D col) {
		if (Vector3.Magnitude(col.relativeVelocity) > 4) {

			//print ("Ow don't do that, I have " + health + " health left");
			if (health < 0) {
				oxy -= Vector3.Magnitude(col.relativeVelocity);
				((GUIText)text.GetComponent("GUIText")).text = "Suit Integrity = " + 0;
				((TubeController)this.GetComponent("TubeController")).SendMessage("DeathIsSoon");
			} else {
				health -= Vector3.Magnitude(col.relativeVelocity);
				((GUIText)text.GetComponent("GUIText")).text = "Suit Integrity = " + health.ToString ("F2");
			}

		}
		
	}
	void OnCollisionExit2D(Collision2D col) {}
	void OnTriggerEnter2D(Collider2D col) {
		//abletopickup = true;
	}
	void OnTriggerExit2D(Collider2D col) {
		//abletopickup = false;
	}
	// Use this for initialization
	void Start () {
		oxy = startingoxy;
		text = GameObject.Find("GuiText");
		Physics2D.IgnoreLayerCollision(0,8);
		//((GUIText)text.GetComponent("GUIText")).text = "Suit Integrity = " + health;
	}
	
	// Update is called once per frame

	void Update () {
		//pickupkey = Input.GetKey (KeyCode.F);
		if (((TubeController)this.GetComponent("TubeController")).ejected) {
			((GUIText)text.GetComponent("GUIText")).text = "Oxygen Left = " + oxy.ToString ("F2");
			oxy -= Time.deltaTime;
			if (oxy <= 0) {
				GameObject.Destroy (this);
			}
		} else {
			((GUIText)text.GetComponent("GUIText")).text = "Suit Integrity = " + health.ToString ("F2");
			oxy = startingoxy;
		}

	}
	void FixedUpdate() {


	}
}
