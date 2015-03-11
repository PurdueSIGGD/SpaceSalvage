using UnityEngine;
using System.Collections;

public class JointScript : MonoBehaviour {
	private LineRenderer lr;
	private SpringJoint2D sp;
	private bool broken;
	public bool severed;
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
			lr.SetPosition(0,this.transform.position);
			lr.SetPosition(1,sp.connectedBody.transform.position);
		}
	}
	void BrokenJoint() {

		Destroy(this.GetComponent<SpringJoint2D>());
		lr.SetVertexCount(0);
		broken = true;
		if (!severed) {
			//GameObject.Find ("Ship").BroadcastMessage ("BrokenRope");
		}

	}
	void ReconnectJoint(Rigidbody2D r) {
		//SpringJoint2D end = this.gameObject.AddComponent<SpringJoint2D>();

	}
}
