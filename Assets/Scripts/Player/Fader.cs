using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

	// Use this for initialization


	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public float bufferX = 0, bufferY = 0;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (target)
		{
			Vector3 point = camera.WorldToViewportPoint(target.position);                                      //get the target's position
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(.05f, .05f, point.z));   //change in distance
			Vector3 destination = transform.position + delta;												   //destination vector (messy)
			destination.Set (destination.x + bufferX, destination.y + bufferY, destination.z);				   //destinatino vector (fixed)
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);  //function to move
		}
	}


	void FadeToBlack(){

			

		}
}
