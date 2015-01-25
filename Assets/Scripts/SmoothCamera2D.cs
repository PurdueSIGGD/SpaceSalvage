using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	void Update () 
	{
		if (target)
		{
			Vector3 point = camera.WorldToViewportPoint(target.position);

			point.Set (point.x - 1000, point.y, point.z);
			point.y += 300;
			//Vector3 point = new Vector3(camera.WorldToViewportPoint(target.position).x ,camera.WorldToViewportPoint(target.position).y) ;
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(.05f, .05f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

		}
	}
}