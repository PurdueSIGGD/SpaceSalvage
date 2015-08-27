using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JointScript : MonoBehaviour {
	private LineRenderer lr;
	private SpringJoint2D sp;
	private EdgeCollider2D eg;
	private bool broken;
	public bool shiprope;
	public bool severed;
	public string connector;
	public float linewidth = .02f;
	public Material material;
	private GameObject focus, subline;
	//private GameObject connected;
	// Use this for initialization
	void Start () {
		severed = false;
		if (this.name == "Player" && !shiprope && this.transform.FindChild("SubLine") == null) {
			subline = new GameObject("SubLine");
			subline.transform.parent = this.transform;
			subline.transform.position = this.transform.position;
			lr = subline.AddComponent<LineRenderer>();
			lr.material = material;
			lr.SetWidth(linewidth, linewidth);


		} else {
			lr = this.GetComponent<LineRenderer>();
		}
		//if (this.name != "Player") {
			//eg = this.GetComponent<EdgeCollider2D>();
		//}
			sp = this.GetComponent<SpringJoint2D>();
		lr.SetVertexCount(2);

	}
	
	// Update is called once per frame
	void Update () {
		if (this.name == "Player" && !shiprope && subline != null	) subline.transform.position = this.transform.position;

		//if (focus != null) print(focus.name);
		if (!broken) {

			if (this.name == "Player" && !shiprope && subline != null) {
				lr = subline.GetComponent<LineRenderer>();
			} else {

				lr = this.GetComponent<LineRenderer>();
			}
			SpringJoint2D[] sps = this.GetComponents<SpringJoint2D>();
			foreach (SpringJoint2D spee in sps) {
				//	print(spee.name + "   " + spee.GetComponent<JointScript>().shiprope);
				if (spee.connectedBody != null && spee.connectedBody.GetComponent<JointScript>() != null) {
					if (spee.connectedBody.GetComponent<JointScript>().shiprope == this.shiprope) { //make sure we get the correct springjoint2D from the player
						sp = spee;
					}
				}
			}


		//	EdgeCollider2D col = this.GetComponent<EdgeCollider2D>(); //remove if rolling back
			//if (connected != null) {
		//		col.points = new Vector2[2] {new Vector2(0,0), ((Vector2)(this.transform.position - connected.transform.position))}; //remove if rolling back
		//	}

			if (this.name != "Player" || shiprope) {
		//		int i = 0;
		//		Vector2[] thepoints = new Vector2[4];
				lr.enabled = true;
				lr.SetWidth(this.linewidth,this.linewidth);
				lr.SetVertexCount(2);
				lr.SetPosition(0,this.transform.position);
		/*		if (this.name != "Player") {

					thepoints[i] = Vector2.zero;
					i++;
					print("setting the first");
				}*/
				if (sp != null && sp.connectedBody != null) {
					lr.SetPosition(1,new Vector3(sp.connectedBody.transform.position.x, sp.connectedBody.transform.position.y, this.transform.position.z));
					/*if (this.name != "Player") {
						thepoints[i] = (new Vector3(sp.connectedBody.transform.position.x, sp.connectedBody.transform.position.y)- this.transform.position);

						i++;
						
						print("setting the second");
					}*/
					SpringJoint2D attempt;
					if ((attempt = sp.connectedBody.GetComponent<SpringJoint2D>()) != null) {
						if (attempt.connectedBody != null ){//&& attempt.connectedBody.gameObject != focus) {


							/*if (this.name != "Player" && this.name != "Joint_1") thepoints[i] = (attempt.connectedBody.transform.position - this.transform.position);
							i++;*/
							lr.SetVertexCount(3);
							lr.SetPosition(2, attempt.connectedBody.transform.position);
						}
					}
				}
				/*if (this.name != "Player") {
					thepoints[i] = Vector2.zero;
					i++;

					//eg.points = thepoints;
					//((Vector2[])newVerticies).CopyTo(eg.points);

				}*/
			} else {
				LineRenderer[] ls = this.GetComponentsInChildren<LineRenderer>();
				foreach (LineRenderer ell in ls) {
					if (ell.name == "SubLine" && !this.shiprope) {
						print(this.name + "  " + this.shiprope + "" + this.linewidth);
						ell.SetWidth(this.linewidth,this.linewidth);
						ell.SetVertexCount(2);
						ell.SetPosition(0,this.transform.position);
						if (sp != null && sp.connectedBody != null) {
							ell.SetPosition(1,new Vector3(sp.connectedBody.transform.position.x, sp.connectedBody.transform.position.y, this.transform.position.z));
							SpringJoint2D attempt;
							if ((attempt = sp.connectedBody.GetComponent<SpringJoint2D>()) != null) {
								if (attempt.connectedBody != null ){//&& attempt.connectedBody.gameObject != focus) {
									ell.SetVertexCount(3);
									ell.SetPosition(2, attempt.connectedBody.transform.position);
								}
							}
						}
						ell.enabled = (GameObject.Find("Player").GetComponentInChildren<CraneController>().grabbed && !GameObject.Find("Player").GetComponentInChildren<CraneController>().broken);
					} else {

					}
				}
			}

		}

	}
	void BrokenJoint() {

		Destroy(sp);
		lr.SetVertexCount(0);
		broken = true;

		if (!severed && focus != null && focus.GetComponent<RopeScript2D>() != null) {
			focus.BroadcastMessage ("BrokenRope");
		}

	}
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "Player") Physics2D.IgnoreCollision(col.collider.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());

		connector = col.gameObject.name;
		//print(col.gameObject.name);

	}
	void GiveFocus(GameObject g) {
		focus = g;
	}
	/*void GiveConnected(GameObject g) {
		connected = g;
	}*/
	void ReconnectJoint() {
		broken = false;
	}
}
