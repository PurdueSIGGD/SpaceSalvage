using UnityEngine;
using System.Collections;

public class JointScript : MonoBehaviour {
	private LineRenderer lr;
	private SpringJoint2D sp;
	private bool broken;
	public bool severed;
	private GameObject focus;
	private GameObject connected;
	// Use this for initialization
	void Start () {
		severed = false;
		lr = this.GetComponent<LineRenderer>();
		sp = this.GetComponent<SpringJoint2D>();
		lr.SetVertexCount(2);

	}
	
	// Update is called once per frame
	void Update () {
		if (!broken) {
			lr = this.GetComponent<LineRenderer>();
			sp = this.GetComponent<SpringJoint2D>();
		//	EdgeCollider2D col = this.GetComponent<EdgeCollider2D>(); //remove if rolling back
			//if (connected != null) {
		//		col.points = new Vector2[2] {new Vector2(0,0), ((Vector2)(this.transform.position - connected.transform.position))}; //remove if rolling back
		//	}
			lr.SetVertexCount(2);
			lr.SetPosition(0,this.transform.position);
			if (sp.connectedBody != null) {
				lr.SetPosition(1,new Vector3(sp.connectedBody.transform.position.x, sp.connectedBody.transform.position.y, this.transform.position.z));
				SpringJoint2D attempt;
				if ((attempt = sp.connectedBody.GetComponent<SpringJoint2D>()) != null) {
					if (attempt.connectedBody != null && attempt.connectedBody.gameObject != focus) {
						lr.SetVertexCount(3);
						lr.SetPosition(2, attempt.connectedBody.transform.position);
					}
				}
			}
		}

	}
	void BrokenJoint() {

		Destroy(this.GetComponent<SpringJoint2D>());
		lr.SetVertexCount(0);
		broken = true;
		if (!severed && focus != null && focus.GetComponent<RopeScript2D>() != null) {
			focus.BroadcastMessage ("BrokenRope");
		}

	}
	void GiveFocus(GameObject g) {
		focus = g;
	}
	void GiveConnected(GameObject g) {
		connected = g;
	}
	void ReconnectJoint() {
		broken = false;
	}
}
