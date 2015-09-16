using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

	/* The fader class is used for darkening the screen. It adapts to the camera's movement, scale, and placement.
	 * It should be a child of the camera class.
	 * It currently uses a sprite as the screen, but that can be changed if it poses future issues.
	 * It only fades on the player losing health, and should be controlled by other classes.
	 * Wait, then we don't need this script anymore if everything else changes it. 
	 * Oh haha this script is obselete, never mind. Don't use it.
	 */

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
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);                                      //get the target's position
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(.05f, .05f, point.z));   //change in distance
			Vector3 destination = transform.position + delta;												   //destination vector (messy)
			destination.Set (destination.x + bufferX, destination.y + bufferY, destination.z);				   //destinatino vector (fixed)
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);  //function to move
		}
	}


	void FadeToBlack(){

			

		}
}
