using UnityEngine;
using System.Collections;

public class TurretRanger : MonoBehaviour {
	// To find the player, used with the chaser and turrets
	float time;
	GameObject Player;
	bool tenabled;
	// Use this for initialization 
	void Start () {
		Player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {

		if (time > 3) {
			tenabled = true;
		} else {
			time+= Time.deltaTime;
			this.SendMessageUpwards("Focus",false);
		}
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (tenabled && Player != null && col.name == "Player") {
			//print(col.name);
			this.SendMessageUpwards("Focus",true);
		}
	}
	void OnTriggerExit2D(Collider2D col) {
		if (tenabled && Player != null && col.name == "Player") {
			this.SendMessageUpwards("Focus",false);

		}
	}
}
