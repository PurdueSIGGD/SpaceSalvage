﻿using UnityEngine;
using System.Collections;
using System;

public class ItemHolder : MonoBehaviour {
	//to hold items in the ship
	private int[] placements;
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
		placements = new int[maxnumpackages];
		numpackages = 0;
	}
	void OnTriggerEnter2D(Collider2D col) {

		if (col.GetComponent<Rigidbody2D>() != null) { //attach the item to the ship
			if (numpackages < maxnumpackages && !col.GetComponent<Rigidbody2D>().isKinematic && col.GetComponent<Loot>() && col.GetComponent<Loot>().timesincekinematic > 8) {
				int i;
				for (i = 0; i < maxnumpackages; i++) {
					if (placements[i] == 0) {
						col.GetComponent<Loot>().index = i;
						placements[i] = 1;
						break;
					}
				}
				col.transform.position = new Vector3(packagearea.x + this.transform.position.x - (1 * i), this.transform.position.y + packagearea.y ,-1 + (.01f * i));
				this.GetComponent<AudioSource>().Play();

				col.GetComponent<Rigidbody2D>().isKinematic = true;
				//if (col.GetComponent<RopeScript2D>() != null) col.gameObject.BroadcastMessage("DestroyRope");
				col.GetComponent<Loot>().isbelonging = true;
				col.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
				col.GetComponent<Rigidbody2D>().angularVelocity = 0;
				col.transform.rotation = Quaternion.Euler(0,0,90);
				col.GetComponent<Rigidbody2D>().Sleep();

				numpackages++;

				((CraneController)GameObject.Find("Player").GetComponentInChildren<CraneController>()).SendMessage("ObjectTakenByShip", col.gameObject);


			}
		}
	}
	void OnCollisionExit2D(Collision2D col) {



	}
	void OnTriggerExit2D(Collider2D col) { //reset the item to work
		if (col.GetComponent<Loot>() != null) {
			if (col.GetComponent<Loot>().isbelonging) {
				col.GetComponent<Loot>().isbelonging = false;
				placements[col.GetComponent<Loot>().index] = 0;
				if (numpackages > 0) {
						numpackages--;
					}
			}

		}

	}
	// Update is called once per frame
	void Update () {

	}
	void Im_Leaving() { //leave the map, save items
		Collider2D[] hitColliders = Physics2D.OverlapAreaAll(this.GetComponent<BoxCollider2D>().bounds.max, this.GetComponent<BoxCollider2D>().bounds.min);
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
		//save the items in a PlayerPrefs item, an external script similar to PlayerPrefs
		PlayerPrefsX.SetStringArray("Items", returnarray);

	}

}
