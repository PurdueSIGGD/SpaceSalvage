using UnityEngine;
using System.Collections;

public class ItemDissolve : MonoBehaviour {
	// AKA coin
	private GameObject Player;
    private GameObject CoinSound;
    
	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
        CoinSound = GameObject.Find("CoinPickup");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		
		if(col.gameObject == Player ) {
            CoinSound.GetComponent<AudioSource>().Play();
			col.SendMessage("changeWallet", 1);
			DestroyObject(this.gameObject);
		} else if(col.transform.GetComponentInParent<CraneController>() != null){
			CoinSound.GetComponent<AudioSource>().Play();
			col.transform.parent.transform.parent.SendMessage("changeWallet", 1);
			DestroyObject(this.gameObject);

		}
		
		
	}

}
