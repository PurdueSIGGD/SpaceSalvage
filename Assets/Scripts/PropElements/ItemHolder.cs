using UnityEngine;
using System.Collections;
using System;

public class ItemHolder : MonoBehaviour {
	public int numpackages;
	public int maxnumpackages = 3;
	public Vector2 packagearea;
	public GameObject[] items;
	private String[] itemnames; 
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey("capacity")) {
			maxnumpackages = PlayerPrefs.GetInt("capacity");
		} else {
			PlayerPrefs.SetInt("capacity",3);
		}
		itemnames = new string[maxnumpackages];
		numpackages = 0;
		items = new GameObject[maxnumpackages];
	}
	void OnTriggerEnter2D(Collider2D col) {
		//go.guiText.text = "Whatever";
		print("oh heloooo");
		bool meh = false;
		for (int i = 0; i < maxnumpackages; i++) {
			itemnames[i] = "";
		}
		if (numpackages < maxnumpackages && !col.rigidbody2D.isKinematic && col.GetComponent<Loot>() && !meh && col.GetComponent<Loot>().timesincekinematic > 8) {
			col.transform.position = new Vector3(packagearea.x + this.transform.position.x, this.transform.position.y + packagearea.y + (1 * numpackages),-1);
			col.rigidbody2D.isKinematic = true;
			if (col.GetComponent<RopeScript2D>() != null) col.gameObject.BroadcastMessage("DestroyRope");
			//col.isTrigger = true;
			items[numpackages] = col.gameObject;
			//print (numpackages + "   " + col.GetComponent<Loot>().itemtype);
			itemnames[numpackages] = col.GetComponent<Loot>().itemtype;
			col.rigidbody2D.velocity = new Vector2(0,0);
			col.rigidbody2D.angularVelocity = 0;
			col.transform.rotation = Quaternion.Euler(0,0,0);
			//Physics2D.IgnoreCollision(this.collider2D, col);





			numpackages++;
			print(new Vector3(packagearea.x + this.transform.position.x, this.transform.position.y + packagearea.y + (1 * numpackages),-1));

			((CraneController)GameObject.Find("Player").GetComponentInChildren<CraneController>()).grabbed = false;


		}
	}
	void OnCollisionExit2D(Collision2D col) {



	}
	void OnTriggerExit2D(Collider2D col) {
		if (col.GetComponent<Loot>()) {// && !((CraneController)GameObject.Find("Player").GetComponentInChildren<CraneController>()).grabbed) {
			if (items[numpackages - 1] != null) {
				(items[numpackages - 1]).collider2D.isTrigger = false;
				items[numpackages - 1] = null;
				numpackages--;

			}
			//print(numpackages);
			//Physics2D.IgnoreCollision(this.collider2D, col.collider, false);
			
		}

	}
	// Update is called once per frame
	void Update () {
		/*for (int i = 0; i < itemnames.Length; i++) {
			print (itemnames[i] + "  " + i);
		}*/
	}
	void Im_Leaving() {
		//print ("Oh he leaving");

			//print ("Sending and stuff");
		for (int i = 0; i < numpackages; i++) {
			itemnames[i] = items[i].GetComponent<Loot>().itemtype;
		}
		PlayerPrefsX.SetStringArray("Items", itemnames);

	}

}
