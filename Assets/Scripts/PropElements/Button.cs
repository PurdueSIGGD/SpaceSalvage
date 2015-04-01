using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	string usestring = "Press 'f' to open door";
	private bool on;
	// Use this for initialization
	void Start () {
		on = false;
		if (this.transform.parent != null) 
			if (this.transform.parent.name.Equals("Airlock")) usestring = "Press 'f' to use airlock";

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Use() {
		on = !on;
		if (on) {
			if (this.transform.parent != null) this.transform.parent.SendMessage("Open");
			else BroadcastMessage("Open");
		} else {
			if (this.transform.parent != null) this.transform.parent.SendMessage("Close");
			else BroadcastMessage("Close");
		}
		//do whatever you want your code to do if the player "uses" it
	}
	
	void OnTriggerExit2D (Collider2D col) {
		if (col.GetComponent<InteractController>() != null) {
			col.SendMessage("GetMessage", "");
		}
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.GetComponent<InteractController>() != null) {
			col.SendMessage("GetMessage", this.usestring);
			col.SendMessage("GetGO", this.gameObject);
		}
	}
	void OnTriggerStay2D(Collider2D col) {
		if (col.GetComponent<InteractController>() != null) {
			col.SendMessage("GetMessage", this.usestring);
		}
	}
	void ChangeWord(string s) {
		usestring = s;
	}

}
