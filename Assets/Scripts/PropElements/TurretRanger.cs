using UnityEngine;
using System.Collections;

public class TurretRanger : MonoBehaviour {
	GameObject Player;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.Equals(Player)) {
			this.SendMessageUpwards("Focus",true);
		}
	}
	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.Equals(Player)) {
			this.SendMessageUpwards("Focus",false);
		}
	}
}
