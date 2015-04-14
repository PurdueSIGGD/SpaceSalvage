using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	string usestring = "Press 'f' to open door";
	public bool open;
	// Use this for initialization
	void Start () {
		if (open)  {
			BroadcastMessage("Open");
			open = !open;
		}
		if (this.transform.parent != null) 
			if (this.transform.parent.name.Equals("Airlock")) usestring = "Press 'f' to use airlock";

	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.parent != null) {
			if (this.transform.parent.name.Equals("Airlock")) {
				if (this.transform.parent.GetComponent<Airlock>().closed || this.transform.parent.GetComponent<Airlock>().cooldowntime < 10) {
					usestring = "";
				} else {
					usestring = "Press 'f' to use airlock";
				}
			}
		}
	}
	void Use() {
		this.GetComponent<AudioSource>().Play();

		open = !open;
		if (!open) {
			//BroadcastMessage("Open");
			if (this.transform.parent != null) this.transform.parent.SendMessage("Open");
			else BroadcastMessage("Open");
		} else {
			//BroadcastMessage("Close");
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
		if (this.transform.parent != null && this.transform.parent.name != "Airlock") usestring = s;
	}

}
