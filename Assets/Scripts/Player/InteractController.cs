using UnityEngine;
using System.Collections;
/* This is a class used to interact with the world.
 * 
 * Sample of code in your target, with a collider2D where istrigger = true.
 * 
 * void Use() {
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
 * usestring being the description of what you want the item to do, i.e. "exit map"
 * 
 */
public class InteractController : MonoBehaviour {
	private GameObject go, target;
	private string message;
	// Use this for initialization
	void Start () {
		go = new GameObject("ExitText");
		GUIText gu = go.AddComponent<GUIText>();
		gu.text = "";
		message = "";
		//gu.font = new Font("Arial");
		//gu.fontSize = 15;
		gu.color = new Color(1,1,1);
		gu.pixelOffset = new Vector2(Screen.width / 2, Screen.height/2);
	}


	// Update is called once per frame
	void GetMessage(string s) {
		message = s;
	}
	void GetGO(GameObject g) {
		target = g;
	}
	void Update () {
		if (target != null && Input.GetKeyDown(KeyCode.F)) {
			target.SendMessage("Use");
		}

		if (message != "") {
			go.GetComponent<GUIText>().text = "Press 'f' to " + message;
		} else {
			go.GetComponent<GUIText>().text = "";
		}
	}
}
