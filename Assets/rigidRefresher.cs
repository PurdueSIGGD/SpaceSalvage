using UnityEngine;
using System.Collections;

public class rigidRefresher : MonoBehaviour {
	float time = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*time+=Time.deltaTime;
		if (time > 5 || false) {
			GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
			Rigidbody2D[] rigids = GameObject.FindObjectsOfType<Rigidbody2D>();
			foreach (GameObject go in gos) {
				if (go.GetComponent<RigidIgnorer>()) go.BroadcastMessage("refresh_rigids", rigids);
			}
		}*/
	}
}
