using UnityEngine;
using System.Collections;

public class ItemDissolve : MonoBehaviour {
	// AKA coin
	private GameObject Player;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		
		if(col.gameObject == Player ) {
			col.SendMessage("changeWallet", 1);
			DestroyObject(this.gameObject);
		} else if(col.transform.GetComponentInParent<CraneController>() != null){
			col.transform.parent.transform.parent.SendMessage("changeWallet", 1);
			DestroyObject(this.gameObject);

		}
		
		
	}

}
