using UnityEngine;
using System.Collections;

public class CraneController : MonoBehaviour {
	public GameObject Player;
	private GameObject focus;
	public Vector3 current;
	private Vector3 pz;
	public int movespeed = 1;
	public float cranelength = 2;
	public bool grabbed = false;
	private bool rot = false;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		current = Player.transform.position;
	}
	void OnMouseDown() {

	}
	void OnMouseUp() {

	}
	// Update is called once per frame
	void Update () {
		this.transform.position = Player.transform.position;
		pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pz.z = 0;
		if (Input.GetMouseButtonDown(0)) {
			if (!grabbed) {
				Collider2D hitCollider = Physics2D.OverlapCircle(current, .3f);
				if (hitCollider.GetComponent("ItemPickup") != null) {
					print ("Got one");
					focus = hitCollider.gameObject;
					grabbed = true;
					((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate = 2;
				}
			} else {
				focus = null;
				grabbed = false;
				((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate = 1;
				//((Rigidbody2D)focus.GetComponent("Rigidbody2D")).velocity = Player.rigidbody2D.velocity;
			}
		}
		rot = Input.GetMouseButton(1);

		LineRenderer l = (LineRenderer)GetComponent<LineRenderer> ();

		//focus.transform.position = pz;
		if (Vector3.Magnitude(Player.transform.position - pz) > cranelength) {
			float theta = Mathf.Atan(Mathf.Abs(pz.y - Player.transform.position.y)/Mathf.Abs (pz.x - Player.transform.position.x));
			//print (this.transform.position);
			//pz = new Vector3((cranelength) * Mathf.Sin(theta), (cranelength) * Mathf.Cos (theta));
		} else {


		}

		current += ((pz - current) / movespeed);


		l.SetPosition(0, Player.transform.position);
		l.SetPosition(1, current);
	}
	void FixedUpdate () {
		if (rot && grabbed) {
			print(focus.transform.rotation);
			focus.transform.Rotate(Vector3.back);
		}
		/*if (mouse0 != lastmouse0) { //toggle
			if (mouse0) {
				grabattempt = !grabattempt;
			}
			lastmouse0 = mouse0;
		}*/
		


	}
}
