using UnityEngine;
using System.Collections;

public class OxygenStation : MonoBehaviour {
	public float oxygaugeoffset = 1.5f;
	public float oxygenAmt = 50;
	private float startingoxy;
	private GameObject Player;
	private GameObject child;


	public Sprite sprite;

	// Use this for initialization
	void Start () {

		startingoxy = oxygenAmt;
		child = new GameObject("Gauge");
		child.transform.position = this.transform.position;
		//child.transform.position = new Vector3);
		child.transform.parent = this.transform;
		SpriteRenderer sp = child.AddComponent<SpriteRenderer>();
		sp.sprite = sprite;
		//Recognize the player in the game
		Player = GameObject.Find("Player"); 
	}
	
	// Update is called once per frame
	void Update () {
		SpriteRenderer sp = child.GetComponent<SpriteRenderer>();
		sp.color = new Color(1 - oxygenAmt/startingoxy, oxygenAmt/startingoxy, 0 , sp.color.a);
		child.transform.position = new Vector3(this.transform.position.x + oxygaugeoffset, this.transform.position.y, this.transform.position.z);
		if (oxygenAmt > .2f) {
			child.transform.localScale = new Vector2(1/this.transform.localScale.x,10 *(1-(oxygenAmt))/(startingoxy * this.transform.localScale.y));

		} else {
			child.transform.localScale = new Vector3(.2f,.2f,.2f);
		}

	}


	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.Equals(Player)){
			if(Player.GetComponent<HealthController>().acceptingOxy && oxygenAmt > 0) {
				oxygenAmt -= 3 * Time.deltaTime;
				col.SendMessage("changeOxy", 10 * Time.deltaTime);
				col.SendMessage("GettingOxy", true);
			} 

		}

	}
	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.Equals(Player)){
			col.SendMessage("GettingOxy", false);	
		}


	}
}
