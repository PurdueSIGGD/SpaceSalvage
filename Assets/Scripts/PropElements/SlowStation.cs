using UnityEngine;
using System.Collections;

public class SlowStation : MonoBehaviour {
	//to slow the player down, like a thick goo

	private Rigidbody2D rigid;
	private GameObject Player;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find ("Player");
		rigid = Player.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){

		if(col.gameObject == Player){
			rigid.drag = 1;
		}

	}

	void OnTriggerExit2D(Collider2D col){
		
		if(col.gameObject == Player){
			rigid.drag = 0;
		}
		
	}

}
