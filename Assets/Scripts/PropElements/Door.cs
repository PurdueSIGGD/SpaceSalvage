using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	private float time;
	private Vector3 open, close;
	public bool opened;
	// Use this for initialization
	void Start () {

		open = transform.FindChild("DoorOpen").transform.position;
		close = transform.FindChild("DoorClose").transform.position;

		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime;

		if (!opened) { //make it relative, add each time Time.deltaTime *  moverate * (pos1.x - pos2.x)
			this.SendMessageUpwards("ChangeWord", "open door");
			if (Mathf.Abs(this.transform.position.y - close.y) > .02f) this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (Time.deltaTime * (open.y - close.y)), this.transform.position.z);
			if (Mathf.Abs(this.transform.position.x - close.x) > .02) this.transform.position = new Vector3(this.transform.position.x + (Time.deltaTime * (close.x - open.x)), this.transform.position.y, this.transform.position.z);
		} else {
			this.SendMessageUpwards("ChangeWord", "close door");
			if (Mathf.Abs(this.transform.position.y - open.y) > .02f) this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (Time.deltaTime * (close.y - open.y)), this.transform.position.z);
			if (Mathf.Abs(this.transform.position.x - open.x) > .02f) this.transform.position = new Vector3(this.transform.position.x + (Time.deltaTime * (open.x - close.x)), this.transform.position.y, this.transform.position.z);

		}
	}
	void Open() {
		opened = true;
	}
	void Close() {
		opened = false;
	}
	void OnCollisionEnter2D(Collision2D col) {
		if (time < .2f) {
			Physics2D.IgnoreCollision(col.collider.collider2D, this.collider2D);
		}
	}
}
