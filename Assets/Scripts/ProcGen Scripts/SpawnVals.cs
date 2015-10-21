using UnityEngine;
using System.Collections;

public class SpawnVals : MonoBehaviour {
	public int count;
	public bool enemies;
	public bool hazards;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter2D(Collision2D col) {
		if (Time.timeSinceLevelLoad < .5f && col.transform.GetComponent<SpawnVals>() != null) {
			//if (col.gameObject != null) GameObject.Destroy(this.gameObject); //to minimize clipping, quick fix with the time we have

		}
	}
}
