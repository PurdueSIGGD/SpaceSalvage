using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JointScript : MonoBehaviour {
	/* Code meant for each joint we would have in any rope
	 * Can be cut or changed
	 * Similar to a linkedlist
	 */

	private LineRenderer lr;
	private SpringJoint2D sp;
	private EdgeCollider2D eg;
	private bool broken;
	private float oxyTime, timeSinceOxy, stopOxy = 5;
	public int shipRopeIndex;
	public bool shiprope;
	public bool severed;
	public bool sleeping;
	public bool spraying;
	public float linewidth = .02f;
	public Material material;
	public GameObject focus, subline;

	//private GameObject connected;
	// Use this for initialization
	void Start () {
		severed = false;
		if (!shiprope) shipRopeIndex = -1;
		if (this.name == "Player" && !shiprope && this.transform.FindChild("SubLine") == null) {
			subline = new GameObject("SubLine"); //we have to create another gameobject to connect our line to if the player grabs an object.
			//I was unable to find a way to make the line renderer have two dfferent materials, however I know it can.
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
		oxyTime += Time.deltaTime;

		if (broken || severed || spraying) {
			timeSinceOxy +=Time.deltaTime;
			if (oxyTime > .1f && ( !spraying || (timeSinceOxy < stopOxy && spraying))) {
				GameObject thingy = (GameObject)Instantiate(GameObject.Find("Player").GetComponent<HealthController>().particle, this.transform.position, Quaternion.identity); //spawning particles
				float r = Random.value;
				//thingy.GetComponent<SpriteRenderer>().sprite = ;
				thingy.transform.localScale = new Vector3(1.5f/(timeSinceOxy+1),1.5f/(timeSinceOxy+1),1.5f/(timeSinceOxy+1)); //typical scale is 5, dont want parts too big or small
				thingy.GetComponent<SpriteRenderer>().color = new Color(1,1,1); //make it redder if necessary
				thingy.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-50,50)));
				//thingy.GetComponent<Rigidbody2D>().AddTorque(thingy.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25,25));
				oxyTime = 0;


			}
		}


		sleeping = this.GetComponent<Rigidbody2D>().IsSleeping();
		if (this.name == "Player" && !shiprope && subline != null	) subline.transform.position = this.transform.position; //keep subline close

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
				if (sp != null && sp.connectedBody != null) { //update the line renderer from our point to points around it, so the lines don't look bulky
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
							lr.SetPosition(2, new Vector3(attempt.connectedBody.transform.position.x, attempt.connectedBody.transform.position.y, -0.29f));
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
						//print(this.name + "  " + this.shiprope + "" + this.linewidth);
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
	void BrokenJoint() { //get rid of the joint
		if (this.GetComponent<SpringJoint2D>()) this.GetComponent<SpringJoint2D>().connectedBody.GetComponent<JointScript>().spraying = true;
		Destroy(sp);
		lr.SetVertexCount(0);
		broken = true;

		if (!severed && focus != null && focus.GetComponent<RopeScript2D>() != null) {
			focus.BroadcastMessage ("BrokenRope");
		}

	}
	void OnCollisionEnter2D(Collision2D col) { //just to confirm we ignore the collision with the player
		if (col.gameObject.name == "Player") Physics2D.IgnoreCollision(col.collider.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
		//print(col.gameObject.name);

	}
	void GiveFocus(GameObject g) { //to understand where the rope is headed
		focus = g;
	}
	/*void GiveConnected(GameObject g) {
		connected = g;
	}*/
	void ReconnectJoint() {
		broken = false;
	}
}
