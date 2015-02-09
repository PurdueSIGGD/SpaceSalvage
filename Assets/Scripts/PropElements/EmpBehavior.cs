using UnityEngine;
using System.Collections;

public class EmpBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerStay2D(Collider2D col) {
		if (col.name.Equals("Player")) {
			col.SendMessage("EMP");
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
