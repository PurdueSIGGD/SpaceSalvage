using UnityEngine;
using System.Collections;

public class Pivoter : MonoBehaviour {
	float f;
	// Use this for initialization
	void Start () {
		f = Random.Range(-7f, 7f);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.rotation = Quaternion.Euler(0,0,this.transform.rotation.eulerAngles.z + f*Time.deltaTime);
	}
}
