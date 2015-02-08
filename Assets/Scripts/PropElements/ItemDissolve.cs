using UnityEngine;
using System.Collections;

public class ItemDissolve : MonoBehaviour {

	private GameObject Player;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		
		if(col.gameObject == Player){

			((PlayerCollisionController)Player.GetComponent ("PlayerCollisionController")).wallet++;
			DestroyObject(this.gameObject);

		}
		
		
	}

}
