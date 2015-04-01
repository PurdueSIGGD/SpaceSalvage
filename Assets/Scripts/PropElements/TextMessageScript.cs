using UnityEngine;
using System.Collections;

public class TextMessageScript : MonoBehaviour {
	public string usestring1 = "Example Text";
	public string usestring2 = "";
	public string usestring3 = "";
	public string usestring4 = "";


	// Use this for initialization
	void Use() {
		// can't use this
		//do whatever you want your code to do if the player "uses" it
	}
	
	void OnTriggerExit2D (Collider2D col) {
		if (col.GetComponent<InteractController>() != null) {
			col.SendMessage("GetMessage", "");
		}
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.GetComponent<InteractController>() != null) {
			col.SendMessage("GetMessage", this.usestring1 + "\n" + this.usestring2 + "\n" + this.usestring3 + "\n" + this.usestring4);
			col.SendMessage("GetGO", this.gameObject);
		}
	}
}
