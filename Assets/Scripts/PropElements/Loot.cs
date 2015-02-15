using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour {
	//Just to show that this item is loot
	public string itemtype;


	// Use this for initialization
	void Start () {
		if (itemtype == null) {
			itemtype = "Medical Supplies";
		}
	}
	
	// Update is called once per frame
	void Update () {
		//print (this.transform.position);
	}
}
