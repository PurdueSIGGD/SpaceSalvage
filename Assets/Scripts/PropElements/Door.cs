using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	//used with button
	private float time;
	private Vector3 open, close;
	public bool opened;
	public float buffer = .1f;
	private bool moving;
	// Use this for initialization
	void Start () {
		buffer = .1f;
		open = transform.FindChild("DoorOpen").transform.localPosition;
		close = transform.FindChild("DoorClose").transform.localPosition;

		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime;
		//print("Moving: " + moving + " Opened: " + opened + "  " + this.name);

			if (!opened) { //make it relative, add each time Time.deltaTime *  moverate * (pos1.x - pos2.x)

				if (transform.parent.name == "Button") this.SendMessageUpwards("ChangeWord", "open door");



				if (Mathf.Abs(this.transform.localPosition.y - close.y) > buffer) this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + (Time.deltaTime * (close.y - open.y)), this.transform.localPosition.z);
				else this.transform.localPosition = new Vector3(this.transform.localPosition.x, close.y, this.transform.localPosition.z);
				if (Mathf.Abs(this.transform.localPosition.x - close.x) > buffer) this.transform.localPosition = new Vector3(this.transform.localPosition.x + (Time.deltaTime * (close.x - open.x)), this.transform.localPosition.y, this.transform.localPosition.z);
				else this.transform.localPosition = new Vector3(close.x, this.transform.localPosition.y, this.transform.localPosition.z);
				moving = ((Mathf.Abs(this.transform.localPosition.x - close.x) > buffer) || (Mathf.Abs(this.transform.localPosition.y - close.y) > .1f));
				//if (moving) print("Whatttt");
			} else {
				if (transform.parent.name == "Button") this.SendMessageUpwards("ChangeWord", "close door");


				if (Mathf.Abs(this.transform.localPosition.y - open.y) > buffer) this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + (Time.deltaTime * (open.y - close.y)), this.transform.localPosition.z);
				if (Mathf.Abs(this.transform.localPosition.x - open.x) > buffer) this.transform.localPosition = new Vector3(this.transform.localPosition.x + (Time.deltaTime * (open.x - close.x)), this.transform.localPosition.y, this.transform.localPosition.z);
				moving = ((Mathf.Abs(this.transform.localPosition.x - open.x) > buffer) || (Mathf.Abs(this.transform.localPosition.y - open.y) > .1f));
				//if (moving) print("Whatttt");

			}

	}
	void Open() {
		opened = true;
		//moving = true;
	}
	void Close() {
		opened = false;
		//moving = true;
	}
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "Player" && moving) {
			col.transform.SendMessage("changeHealth",Time.deltaTime * -1000);
		}

		if (col.rigidbody != null) {
			if (!moving && !opened && col.gameObject.GetComponent<JointScript>() != null && col.transform.name != "Player") {
				col.rigidbody.isKinematic = true;

			} else {
				col.rigidbody.isKinematic = false;
			}
		}
	}
	void OnCollisionStay2D(Collision2D col) {
		if (col.rigidbody != null) {
			if (!moving && !opened && col.gameObject.GetComponent<JointScript>() != null && col.transform.name != "Player") {
				col.rigidbody.isKinematic = true;
				
			} else {
				col.rigidbody.isKinematic = false;
			}
		}
	}
	void OnCollisionExit2D(Collision2D col) {
		if (col.rigidbody != null && col.gameObject.GetComponent<JointScript>() != null && col.transform.name != "Player") {
			col.rigidbody.isKinematic = false;
//			col.gameObject.SendMessage("LoseGO");
			
		}
	}
}
