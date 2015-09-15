using UnityEngine;
using System.Collections;

public class Edgebounds : MonoBehaviour
{
	GameObject p;
	int dir;
	Edgebounds newEdge;
	Edgebounds leftEdge;
	Edgebounds rightEdge;
	Edgebounds fowardEdge;
	Quaternion facing;
	Vector3 edgePos;
	public GameObject nextSpawn;
	//should be the initial distance for the beginning spawning bounds of the game
	public float initialDistance;
	// Use this for initialization
	void Start ()
	{
		edgePos = GetComponentInParent<Transform> ().position;
		p = GameObject.Find ("Player");
		facing = GetComponentInParent<Transform> ().rotation;

		if(-180 > facing.z){
			facing.z = 90;
		}
		if(180 < facing.z){
			facing.z = -90;
		}

		if (facing.z == 180 || facing.z == -180 || facing.z == 0) {
			this.GetComponent<EdgeCollider2D>().points[0] = new Vector2(edgePos.x,edgePos.y+initialDistance);
			this.GetComponent<EdgeCollider2D>().points[1] = new Vector2(edgePos.x,edgePos.y-initialDistance);
		}
		if (facing.z == 90 || facing.z == -90) {
			this.GetComponent<EdgeCollider2D>().points[0] = new Vector2(edgePos.x+initialDistance,edgePos.y);
			this.GetComponent<EdgeCollider2D>().points[1] = new Vector2(edgePos.x-initialDistance,edgePos.y);
		}

		this.GetComponent<EdgeCollider2D> ().isTrigger = true;
	}

	//gets the moving fowards coordinates to find the location of the 
	Vector3 fowardCoords(Quaternion f){
		dir = (int)facing.z;
		switch(dir){
		case(0):
			return new Vector3(edgePos.x + (initialDistance*2),edgePos.y,edgePos.z);
			break;
		case(90):
			return new Vector3(edgePos.x,edgePos.y + (initialDistance*2),edgePos.z);
			break;
		case(-90):
			return new Vector3(edgePos.x,edgePos.y - (initialDistance*2),edgePos.z);
			break;
		case(180):
			return new Vector3(edgePos.x - (initialDistance*2),edgePos.y,edgePos.z);
			break;
		case(-180):
			return new Vector3(edgePos.x - (initialDistance*2),edgePos.y,edgePos.z);
			break;
			//i needed to put in the default so the compiler would like me
		default:
			return new Vector3(0,0,0);
			break;
		}
	}

	Vector3 rightCoords(Quaternion f){
		dir = (int)facing.z;
		switch(dir){
		case(0):
			return new Vector3(edgePos.x + initialDistance,edgePos.y-initialDistance,edgePos.z);
			break;
		case(90):
			return new Vector3(edgePos.x +initialDistance,edgePos.y + initialDistance,edgePos.z);
			break;
		case(-90):
			return new Vector3(edgePos.x -initialDistance,edgePos.y - initialDistance,edgePos.z);
			break;
		case(180):
			return new Vector3(edgePos.x - initialDistance,edgePos.y +initialDistance,edgePos.z);
			break;
		case(-180):
			return new Vector3(edgePos.x - initialDistance,edgePos.y+initialDistance,edgePos.z);
			break;
			//i needed to put in the default so the compiler would like me
		default:
			return new Vector3(0,0,0);
			break;
		}
	}

	Vector3 leftCoords(Quaternion f){
		dir = (int)facing.z;
		switch(dir){
		case(0):
			return new Vector3(edgePos.x + initialDistance,edgePos.y + initialDistance,edgePos.z);
			break;
		case(90):
			return new Vector3(edgePos.x - initialDistance,edgePos.y +initialDistance,edgePos.z);
			break;
		case(-90):
			return new Vector3(edgePos.x+initialDistance,edgePos.y - initialDistance,edgePos.z);
			break;
		case(180):
			return new Vector3(edgePos.x - initialDistance,edgePos.y - initialDistance,edgePos.z);
			break;
		case(-180):
			return new Vector3(edgePos.x -initialDistance,edgePos.y - initialDistance,edgePos.z);
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
			fowardEdge = (Edgebounds)Instantiate(fowardEdge,fowardCoords(facing),facing);
			fowardEdge.initialDistance = this.initialDistance;
			//one to the right of the foward
			rightEdge = (Edgebounds)Instantiate(rightEdge,rightCoords(facing),facing);
			rightEdge.facing.SetFromToRotation(new Vector3(facing.x,facing.y,facing.z),new Vector3(facing.x,facing.y,facing.z+90));
			rightEdge.initialDistance = this.initialDistance;
			//one to the left of the foward
			leftEdge = (Edgebounds)Instantiate(leftEdge,leftCoords(facing),facing);
			leftEdge.facing.SetFromToRotation(new Vector3(facing.x,facing.y,facing.z),new Vector3(facing.x,facing.y,facing.z-90));
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

