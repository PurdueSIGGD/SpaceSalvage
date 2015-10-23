using UnityEngine;
using System.Collections;

public class MultiResultStarter : MonoBehaviour {
	//For spawning an item that can have multiple possibilities
	private bool done;
	public float radius;
	public GameObject[] possibilities;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	public GameObject poopedOut () {
		int r = Random.Range(0, possibilities.Length);
		GameObject thingy = (GameObject)Instantiate(possibilities[r], this.transform.position, Quaternion.identity);
		thingy.transform.rotation = this.transform.rotation;
		done = true;
		return thingy;
	}
	void Update() {
		
		if (done) Destroy(this.gameObject);
	}
}
