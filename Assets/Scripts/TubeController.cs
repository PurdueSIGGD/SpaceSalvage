using UnityEngine;
using System.Collections;
using System;

public class TubeController : MonoBehaviour {
	public int tubesleft = 50;
	private float ejectcooldown;
	private bool ejectkey;
	private bool ejected = false;
	private int tubecount = 0; //because I am stupid and +- 1 errors are the bane of my existence
	private float tubelength = 3;
	private Vector3[] tubes; //There is no function to get a value at the index for LineRenderer
	private float dist = 0;
	private float timesincelast;
	// Use this for initialization
	void Start () {
		tubes = new Vector3[tubesleft];
		LineRenderer l = (LineRenderer) GetComponent<LineRenderer>();
		l.SetVertexCount (2);
		l.SetPosition (0, new Vector3(0,0,0));
		l.SetPosition (1, this.transform.position);
		tubes[0] = (new Vector3 (0, 0, 0)); //Start with two values, one being in ship, one on you (free to change);
		tubecount = 2;
		ejectkey = false;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (!ejectkey) { //Stop everything if ejected
						ejectkey = Input.GetKey (KeyCode.G);
						tubes [tubecount - 1] = this.transform.position; //Update position
						LineRenderer l = (LineRenderer)GetComponent<LineRenderer> ();
						if (tubecount > 1) { //In order to pick up tubes
								if (timesincelast > 2 && (Math.Abs (this.transform.position.x - ((Vector3)tubes [tubecount - 2]).x) < .6) && (Math.Abs (this.transform.position.y - ((Vector3)tubes [tubecount - 2]).y) < .6)) {
										
										tubecount--;
										dist = 0;
										tubesleft++;
										l.SetVertexCount (tubecount);
										timesincelast = 0;
										tubes [tubecount] = new Vector3 (0, 0, 0); //clear value
								}
						}
						dist = Vector3.Magnitude ((tubes [tubecount - 2] - transform.position)); //far away enough?
						l.SetPosition (tubecount - 1, this.transform.position);
						timesincelast += Time.deltaTime;
						if (dist > tubelength && timesincelast > 2) { //can we drop it like it is hot?
								timesincelast = 0;
								tubesleft--;
								tubecount++;
								tubes [tubecount - 1] = this.transform.position;
								l.SetVertexCount (tubecount);
								l.SetPosition (tubecount - 1, this.transform.position);
								dist = 0;
						}
				}
	}
	void FixedUpdate() {
		if (ejected) {
			ejectcooldown += Time.fixedDeltaTime;
			if (ejectcooldown >= 4 && (Math.Abs(this.transform.position.x - ((Vector3)tubes [tubecount - 1]).x) < .6) && (Math.Abs (this.transform.position.y - ((Vector3)tubes [tubecount - 1]).y) < .6 )) {
				ejectkey = false; //come back from ejecting
				ejected = false;
				ejectcooldown = 0;
			}
		}

		if (ejectkey) {
			ejected = true;
		}
	}
}
