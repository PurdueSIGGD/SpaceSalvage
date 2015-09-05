using UnityEngine;
using System.Collections;

public class Background_Controller : MonoBehaviour {
	private GameObject Camera;
	// Use this for initialization
	void Start () {
		Camera = GameObject.Find("Camera");
		this.transform.position = Camera.transform.position + Vector3.forward*10;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = Camera.transform.position + Vector3.forward*10;

	}
}
