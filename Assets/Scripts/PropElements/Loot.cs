using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour {
	//Just to show that this item is loot
	public string itemtype;
	public float timesincekinematic;


	// Use this for initialization
	void Start () {
		if (itemtype == null || itemtype.Equals("")) {
			itemtype = "Medical Supplies";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.rigidbody2D.isKinematic) {
			timesincekinematic += Time.deltaTime;
		} else {
			timesincekinematic = 0;
		}
		//print (this.transform.position);
	}
}
