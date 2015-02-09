using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour {
	//private GameObject display;
	// Use this for initialization
	GameObject go;
	void Start () {
		//go = new GameObject("Return");
		//display = GameObject.Find ("display");
		//go.AddComponent("GUIText");

	}
	void OnTriggerEnter2D(Collider2D col) {
		//go.guiText.text = "Whatever";
	}
	void OnTriggerExit2D (Collider2D col) {
		//go.guiText.text = "";
	}
	void OnTriggerStay2D(Collider2D col) {
		//go.transform.position = new Vector3 (col.transform.position.x, col.transform.position.y + 40, col.transform.position.z);
		//print ("hi");


		if (Input.GetKey (KeyCode.F) && col.name.Equals("Player")) {
			//Application.LoadLevel("Testing Scene 1");
			// this will be our scene where you start to purchase items and change your stats
			print ("Loading new level");
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
