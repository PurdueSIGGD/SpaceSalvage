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
		if (opened) { //make it relative, add each time Time.deltaTime *  moverate * (pos1.x - pos2.x)
			if (this.transform.position.y > open.y + .1f || this.transform.position.y < open.y - .1f) this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + Time.deltaTime, this.transform.position.z);
			if (this.transform.position.y > open.x + .1f || this.transform.position.x < open.x - .1f) this.transform.position = new Vector3(this.transform.position.x + Time.deltaTime, this.transform.position.y, this.transform.position.z);
		} else {
			if (this.transform.position.y > close.y + .1f || this.transform.position.y < close.y - .1f) this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + Time.deltaTime, this.transform.position.z);
			if (this.transform.position.y > close.x + .1f || this.transform.position.x < close.x - .1f) this.transform.position = new Vector3(this.transform.position.x + Time.deltaTime, this.transform.position.y, this.transform.position.z);

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
