using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public float bufferX = 0, bufferY = 0;
	private float shakeQuantity = 0;
	public float startingSize, followSpeed;
	public float focusSize;
	private bool onoff = false;
	//static float timeIndex = .005f;

	void Start() {
		startingSize = this.GetComponent<Camera> ().orthographicSize;
		focusSize = startingSize;
	}

	void Update () 
	{
		//print(Input.mouseScrollDelta + " " + this.GetComponent<Camera> ().orthographicSize);
		//float size = Camera.main.main.orthographicSize + Input.mouseScrollDelta.y;
		//Camera.main.main.orthographicSize = size;
		if (Input.mouseScrollDelta.y > 0 && focusSize > 4) {
			focusSize -= 1;

		} else {
			if (Input.mouseScrollDelta.y < 0 && focusSize < 25) {
				focusSize += 1;
			} else {
				//if (focusSize + .5f > startingSize || focusSize - .5f < startingSize);
				float factor = ((focusSize - startingSize) * Time.deltaTime)/startingSize;
				startingSize += ((focusSize - startingSize) * Time.deltaTime); //move smoothly to zoom in our out
				bufferX += factor*bufferX; //to affect buffers in the correct scale
				bufferY += factor*bufferY;
				Vector3 Background = this.transform.FindChild("Background").localScale; //to make sure the background does not change
				Background = new Vector3(Background.x += Background.x * factor, Background.y += Background.y * factor, 1);
				this.transform.FindChild("Background").localScale = Background;

				Vector3 Faders = this.transform.FindChild("Fader").localScale; //to make sure the background does not change
				Faders = new Vector3(Faders.x += Faders.x * factor, Faders.y += Faders.y * factor, 1);
				this.transform.FindChild("Fader").localScale = Faders;
			}
		}
		if (target)
		{
			Vector3 MouseandTarget =(((Vector3)target.GetComponent<Rigidbody2D>().velocity) + 2*target.transform.position + (.5f)*(target.transform.position + Camera.main.ScreenToWorldPoint(Input.mousePosition)))/3;
			dampTime = (followSpeed)/Vector2.Distance(this.transform.position, MouseandTarget);
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(MouseandTarget);                                      //get the target's position
			Vector3 delta = MouseandTarget - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(.05f, .05f, point.z));   //change in distance
			Vector3 destination = transform.position + delta;												   //destination vector (messy)
			destination.Set (destination.x + bufferX, destination.y + bufferY, destination.z);				   //destinatino vector (fixed)
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);  //function to move

		}


		if (shakeQuantity > 0) {

				this.transform.eulerAngles = new Vector3 (0, 0, UnityEngine.Random.Range (1 * shakeQuantity, -1 * shakeQuantity));
				this.GetComponent<Camera> ().orthographicSize = startingSize + UnityEngine.Random.Range (.05f * shakeQuantity, -.05f * shakeQuantity);
				if (onoff) {
					shakeQuantity = 1; 
				} else {

					shakeQuantity -= 10 * Time.deltaTime;
				}
			
		} else {
				this.transform.eulerAngles = new Vector3 (0, 0, 0);
				this.GetComponent<Camera> ().orthographicSize = startingSize;
		}
	
	}

	void Shake(float amount) {
		shakeQuantity = amount;
		//onoff = false;
	}

	void ShakeOnOff(bool b) {
		onoff = b;
	}
}