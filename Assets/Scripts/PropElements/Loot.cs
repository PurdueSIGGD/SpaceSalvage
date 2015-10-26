﻿using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour {
	//Just to show that this item is loot, and can be taken by the ship
	public bool isbelonging = false;
	public string itemtype;
	public float timesincekinematic;
	public int index;


	// Use this for initialization
	void Start () {
		timesincekinematic = 8;
		if (itemtype == null || itemtype.Equals("")) {
			itemtype = "Medical Supplies";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.GetComponent<Rigidbody2D>().isKinematic) {
			timesincekinematic += Time.deltaTime;
		} else {
			timesincekinematic = 0;
		}
		//print (this.transform.position);
	}
}
