using UnityEngine;
using System.Collections;

public class TextMessageScript : MonoBehaviour {
	public string usestring1 = "Example Text";
	public string usestring2 = "";
	public string usestring3 = "";
	public string usestring4 = "";
	//uses the interact script to show items, but cannot be used for anything

	// Use this for initialization
	void Use() {
		// can't use this
	}
	void Update() {

	}
	void OnTriggerExit2D (Collider2D col) {

		if (col.GetComponent<InteractController>() != null && col.GetComponent<InteractController>().message == this.usestring1 + "\n" + this.usestring2 + "\n" + this.usestring3 + "\n" + this.usestring4) {
			col.SendMessage("GetMessage", "");
		}
	}
	void OnTriggerStay2D(Collider2D col) {
		if (col.GetComponent<InteractController>() != null && col.GetComponent<InteractController>().message == "") {
			col.SendMessage("GetMessage", this.usestring1 + "\n" + this.usestring2 + "\n" + this.usestring3 + "\n" + this.usestring4);
			col.SendMessage("GetGO", this.gameObject);
		}
	}
}
