using UnityEngine;
using System.Collections;
using System;

public class ItemHolder : MonoBehaviour {
	public int numpackages;
	public int maxnumpackages = 3;
	public Vector2 packagearea;
	public GameObject[] items;
	// Use this for initialization
	void Start () {
		numpackages = 0;
		items = new GameObject[maxnumpackages];
	}
	void OnCollisionEnter2D(Collision2D col) {
		//go.guiText.text = "Whatever";
		bool meh = false;
		for (int i = 0; i < 3; i++) {
			try {
				if (items[i].Equals(col.gameObject)) {
					print("OPOIUHVILGOEWIT");
					meh = true; 
					break;
				}
			} catch (Exception e) {
				break;
			}
		}
		if (numpackages < maxnumpackages && col.collider.GetComponent<Loot>() && !meh) {
			items[numpackages] = col.gameObject;
			col.collider.rigidbody2D.velocity = new Vector2(0,0);
			col.collider.rigidbody2D.angularVelocity = 0;
			Physics2D.IgnoreCollision(this.collider2D, col.collider);
			print("enter");
			col.transform.position = new Vector3(packagearea.x, packagearea.y,-1);
			numpackages++;
			GameObject.Find("Player").GetComponentInChildren<CraneController>().SendMessage("I_am_letting_go_now");
			((CraneController)GameObject.Find("Player").GetComponentInChildren<CraneController>()).grabbed = false;


		}
	}
	void OnCollisionExit2D(Collision2D col) {
		if (col.collider.GetComponent<Loot>() && !((CraneController)GameObject.Find("Player").GetComponentInChildren<CraneController>()).grabbed) {
			items[numpackages - 1] = null;
			numpackages--;
			print("exit");
			Physics2D.IgnoreCollision(this.collider2D, col.collider, false);

		}


	}
	// Update is called once per frame
	void Update () {
	
	}
}
