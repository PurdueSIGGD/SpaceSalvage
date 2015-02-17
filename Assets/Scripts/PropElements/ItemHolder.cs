﻿using UnityEngine;
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
		itemnames = new string[maxnumpackages];
		numpackages = 0;
		items = new GameObject[maxnumpackages];
	}
	void OnCollisionEnter2D(Collision2D col) {
		//go.guiText.text = "Whatever";
		bool meh = false;
		for (int i = 0; i < maxnumpackages; i++) {
			itemnames[i] = "";
		}
		if (numpackages < maxnumpackages && col.collider.GetComponent<Loot>() && !meh) {
			col.collider.isTrigger = true;
			items[numpackages] = col.gameObject;
			//print (numpackages + "   " + col.collider.GetComponent<Loot>().itemtype);
			itemnames[numpackages] = col.collider.GetComponent<Loot>().itemtype;
			col.collider.rigidbody2D.velocity = new Vector2(0,0);
			col.collider.rigidbody2D.angularVelocity = 0;
			col.transform.rotation = Quaternion.Euler(0,0,0);
			//Physics2D.IgnoreCollision(this.collider2D, col.collider);





			numpackages++;
			//print(numpackages);
			col.transform.position = new Vector3(packagearea.x, packagearea.y + 1 * numpackages,-1);

			GameObject.Find("Player").GetComponentInChildren<CraneController>().SendMessage("I_am_letting_go_now");
			((CraneController)GameObject.Find("Player").GetComponentInChildren<CraneController>()).grabbed = false;


		}
	}
	void OnCollisionExit2D(Collision2D col) {



	}
	void OnTriggerExit2D(Collider2D col) {
		if (col.GetComponent<Loot>()) {// && !((CraneController)GameObject.Find("Player").GetComponentInChildren<CraneController>()).grabbed) {
			(items[numpackages - 1]).collider2D.isTrigger = false;
			items[numpackages - 1] = null;
			numpackages--;
			print(numpackages);
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
