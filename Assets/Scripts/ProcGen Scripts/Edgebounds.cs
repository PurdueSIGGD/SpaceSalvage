using UnityEngine;
using System.Collections;

public class Edgebounds : MonoBehaviour
{
	GameObject p;
	int dir;
	Edgebounds newEdge;
	public Edgebounds leftEdge;
	public Edgebounds rightEdge;
	public Edgebounds fowardEdge;
	public Quaternion facing;
	public Vector3 facingVect;
	public Vector3 edgePos;
	enum sides{
		up = 0,
		down = 180,
		left = 90,
		right = 270
	};
	sides mySide;
	Vector2[] edgeSETTA = new Vector2[2];
	EdgeCollider2D thing;
	//should be the initial distance for the beginning spawning bounds of the game
	public float initialDistance;
	// Use this for initialization
	void Start ()
	{
		facing = this.transform.rotation;
		facingVect = this.transform.eulerAngles;
		edgePos = this.transform.position;
		p = GameObject.Find ("Player");

//		if (90 < facingVect.z & facingVect.z < 91) {
//			facingVect.z = 90;
//		}

		//goes from 0 to -90 so it needs to be changed to 270
		if (facingVect.z < 0) {
			facingVect.z = 270;
		}
		//goes from 270 to 360 so it needs to be changed to 0
		if (facingVect.z > 270) {
			facingVect.z = 0;
		}

		edgeSETTA[0] = new Vector2(initialDistance,0);
		edgeSETTA[1] = new Vector2(-initialDistance,0);
		this.GetComponent<EdgeCollider2D>().points = edgeSETTA;

		this.GetComponent<EdgeCollider2D> ().isTrigger = true;
	}

	//gets the moving fowards coordinates to find the location of the
	//hopefully the 90 degrees will change from 90.00000001 to 90 with the cast change
	Vector3 fowardCoords(Vector3 f){
		dir = (int)facingVect.z;
		switch(dir){
		case(270):
			return new Vector3(edgePos.x + (initialDistance*2),edgePos.y,edgePos.z);
			break;
		case(0):
			return new Vector3(edgePos.x,edgePos.y + (initialDistance*2),edgePos.z);
			break;
		case(180):
			return new Vector3(edgePos.x,edgePos.y - (initialDistance*2),edgePos.z);
			break;
		case(90):
			return new Vector3(edgePos.x - (initialDistance*2),edgePos.y,edgePos.z);
			break;
			//i needed to put in the default so the compiler would like me
		default:
			return new Vector3(0,0,0);
			break;
		}
	}

	Vector3 rightCoords(Vector3 f){
		dir = (int)facingVect.z;
		switch(dir){
		case(270):
			return new Vector3(edgePos.x + initialDistance,edgePos.y-initialDistance,edgePos.z);
			break;
		case(0):
			return new Vector3(edgePos.x +initialDistance,edgePos.y + initialDistance,edgePos.z);
			break;
		case(180):
			return new Vector3(edgePos.x -initialDistance,edgePos.y - initialDistance,edgePos.z);
			break;
		case(90):
			return new Vector3(edgePos.x - initialDistance,edgePos.y +initialDistance,edgePos.z);
			break;
			//i needed to put in the default so the compiler would like me
		default:
			return new Vector3(0,0,0);
			break;
		}
	}

	Vector3 leftCoords(Vector3 f){
		dir = (int)facingVect.z;
		switch(dir){
		case(270):
			return new Vector3(edgePos.x + initialDistance,edgePos.y + initialDistance,edgePos.z);
			break;
		case(0):
			return new Vector3(edgePos.x - initialDistance,edgePos.y +initialDistance,edgePos.z);
			break;
		case(180):
			return new Vector3(edgePos.x+initialDistance,edgePos.y - initialDistance,edgePos.z);
			break;
		case(90):
			return new Vector3(edgePos.x - initialDistance,edgePos.y - initialDistance,edgePos.z);
			break;
			//i needed to put in the default so the compiler would like me
		default:
			return new Vector3(0,0,0);
			break;
		}
	}

	//create new script for this
	void OnTriggerEnter2D(Collider2D thing){
		//It detects every object in the map before activating the update and not
		//the player. will probably need to change it. just trying to get it working now
		if (thing == p.GetComponent<PolygonCollider2D>()) {
			//instantiates the foward one
			fowardEdge = (Edgebounds)Instantiate(fowardEdge,fowardCoords(facingVect),facing);
			fowardEdge.initialDistance = this.initialDistance;
			//one to the right of the foward
			rightEdge = (Edgebounds)Instantiate(rightEdge,rightCoords(facingVect),facing);
			rightEdge.transform.Rotate(new Vector3(0,0,-90));
			rightEdge.initialDistance = this.initialDistance;
			//one to the left of the foward
			leftEdge = (Edgebounds)Instantiate(leftEdge,leftCoords(facingVect),facing);
			leftEdge.transform.Rotate(new Vector3(0,0,90));
			leftEdge.initialDistance = this.initialDistance;
			Destroy (this);
		}
	}
	//implement later
	void cleanbounds(){
		//gets rid of redundant colliders
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

