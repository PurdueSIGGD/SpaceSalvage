using UnityEngine;
using System.Collections;
using System;

public class ItemHolder : MonoBehaviour {
	public int numpackages;
	public int maxnumpackages = 3;
	public Vector2 packagearea;
	// Use this for initialization
	void Start () {

		if (PlayerPrefs.HasKey("capacity")) {
			maxnumpackages = PlayerPrefs.GetInt("capacity");
		} else {
			PlayerPrefs.SetInt("capacity",3);
		}
		numpackages = 0;
		//items = new GameObject[maxnumpackages];
	}
	void OnTriggerEnter2D(Collider2D col) {
		//go.guiText.text = "Whatever";
		//print("oh heloooo");


		if (col.rigidbody2D != null) {
			//print("Has rigidbody" + col.name);
			if (numpackages < maxnumpackages && !col.rigidbody2D.isKinematic && col.GetComponent<Loot>() && col.GetComponent<Loot>().timesincekinematic > 8) {
				//print("get in");
				col.transform.position = new Vector3(packagearea.x + this.transform.position.x, this.transform.position.y + packagearea.y + (1 * numpackages),-1);
				col.rigidbody2D.isKinematic = true;
				if (col.GetComponent<RopeScript2D>() != null) col.gameObject.BroadcastMessage("DestroyRope");
				//col.isTrigger = true;
				//items[numpackages] = col.gameObject;
				//print (numpackages + "   " + col.GetComponent<Loot>().itemtype);
				//itemnames[numpackages] = col.GetComponent<Loot>().itemtype;
				col.GetComponent<Loot>().isbelonging = true;
				col.rigidbody2D.velocity = new Vector2(0,0);
				col.rigidbody2D.angularVelocity = 0;
				col.transform.rotation = Quaternion.Euler(0,0,0);
				col.rigidbody2D.Sleep();
				//Physics2D.IgnoreCollision(this.collider2D, col);





				numpackages++;
				//print(new Vector3(packagearea.x + this.transform.position.x, this.transform.position.y + packagearea.y + (1 * numpackages),-1));

				((CraneController)GameObject.Find("Player").GetComponentInChildren<CraneController>()).grabbed = false;


			}
		}
	}
	void OnCollisionExit2D(Collision2D col) {



	}
	void OnTriggerExit2D(Collider2D col) {
		if (col.GetComponent<Loot>() != null) {// && !((CraneController)GameObject.Find("Player").GetComponentInChildren<CraneController>()).grabbed) {
			if (col.GetComponent<Loot>().isbelonging) {
				col.GetComponent<Loot>().isbelonging = false;
				if (numpackages > 0) {
						numpackages--;
					}
			}
			//print(numpackages);
			//Physics2D.IgnoreCollision(this.collider2D, col.collider, false);
			
		}

	}
	// Update is called once per frame
	void Update () {

	}
	void Im_Leaving() {
		//print ("Oh he leaving");
		Collider2D[] hitColliders = Physics2D.OverlapAreaAll(this.GetComponent<BoxCollider2D>().bounds.max, this.GetComponent<BoxCollider2D>().bounds.min);
			//print ("Sending and stuff");
		String[] itemnames = new String[maxnumpackages];
		int i = 0;
		foreach (Collider2D c in hitColliders) {
			if (c.GetComponent<Loot>() != null && i < this.maxnumpackages) {
				itemnames[i] = c.GetComponent<Loot>().itemtype;
				i++;
			}
		}
		String[] returnarray = new String[i];
		i = 0;
		foreach (string s in itemnames) {
			if (s != null) {
				returnarray[i] = s;
				i++;
			}
		}
		PlayerPrefsX.SetStringArray("Items", returnarray);

	}

}
