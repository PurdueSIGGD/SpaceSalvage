using UnityEngine;
using System.Collections;

public class CraneController : MonoBehaviour {
	public GameObject Player;
	private GameObject focus;
	public Vector3 current;
	private Vector3 pz;
	public int movespeed = 30;
	public float cranelength = 2;
	private bool grabbed = false;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		focus = GameObject.Find ("Pickup");
		current = Player.transform.position;
	}
	void OnMouseDown() {

	}
	void OnMouseUp() {

	}
	// Update is called once per frame
	void Update () {

		((CircleCollider2D)this.GetComponent("CircleCollider2D")).center = pz;
		if (Input.GetMouseButtonDown(0)) {
			print ("FUCK");
			Collider[] hitColliders = Physics.OverlapSphere(pz, .3f);
			int i = 0;
			while (i < hitColliders.Length) {
				if (focus.collider.GetComponent("ItemPickup") != null) {
					focus = hitColliders[i].gameObject;
					grabbed = true;
				}
				i++;
			}
		}
		LineRenderer l = (LineRenderer)GetComponent<LineRenderer> ();
		pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pz.z = 0;
		//focus.transform.position = pz;
		/*if (Vector3.Magnitude(Player.transform.position - pz) < cranelength) {
			pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pz.z = 0;
		} else {
			float theta = Mathf.Atan(pz.y/pz.x);
			pz = new Vector3((cranelength - .3f) * Mathf.Sin(theta), (cranelength - .3f) * Mathf.Cos (theta));
		}*/
		current += ((pz - current) / movespeed);
		if (grabbed) {
			focus.SendMessage("followSuit");
		}

		l.SetPosition(0, Player.transform.position);
		l.SetPosition(1, pz);
	}
	void FixedUpdate () {
		/*if (mouse0 != lastmouse0) { //toggle
			if (mouse0) {
				grabattempt = !grabattempt;
			}
			lastmouse0 = mouse0;
		}*/
		


	}
}
