using UnityEngine;
using System.Collections;

public class Edgebounds : MonoBehaviour
{
	
	public Edgebounds leftEdge;
	public Edgebounds rightEdge;
	public Edgebounds fowardEdge;
	public Quaternion facing;
	public Vector3 facingVect;
	public Vector3 edgePos;
	
	GameObject p;
	int dir;
	Vector2[] edgeSETTA = new Vector2[2];
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
		
		//goes from 0 to -90 so it needs to be changed to 
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
			return Vector3.zero;
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
			return Vector3.zero;
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
			return Vector3.zero;
			break;
		}
	}
	
	//create new script for this
	void OnTriggerEnter2D(Collider2D thing){
		Edgebounds eC;
		// detecting if it is in the same space as another EdgeCollider2D
		// if it is it will delete that object and itself
		if(thing.GetComponentInParent<Edgebounds>() != null){
			eC = thing.GetComponentInParent<Edgebounds>();
			if(this.edgePos.x == eC.edgePos.x){
				if(this.edgePos.y == eC.edgePos.y){
					Destroy(eC.gameObject);
					Destroy(this.gameObject);
				}
			}
		}
		// Creates the 3 borders then destroys itself
		if (p != null && thing == p.GetComponent<PolygonCollider2D>() && !thing.GetComponent<TurretRanger>()) {
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
			
			//Generates stuff within the boundaries
			//needs to generate from bottom left to top right
			GameObject ProcGenController = GameObject.Find ("ProcGenController");
			switch((int)facingVect.z){
			case(270):	//generate a 1x3 block right of the entered region
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * 3, edgePos.y + initialDistance * 2);
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * 3, edgePos.y + initialDistance * 0);
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * 3, edgePos.y + initialDistance * -2);
				break;
			case(0):	//genrate a 3x1 block above the entered region
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * 2, edgePos.y + initialDistance * 3);
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * 0, edgePos.y + initialDistance * 3);
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * -2, edgePos.y + initialDistance * 3);
				break;
			case(180):	//genrate a 3x1 block below the entered region
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * 2, edgePos.y + initialDistance * -3);
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * 0, edgePos.y + initialDistance * -3);
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * -2, edgePos.y + initialDistance * -3);
				break;
			case(90):	//generate a 1x3 block left of the entered region
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * -3, edgePos.y + initialDistance * 2);
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * -3, edgePos.y + initialDistance * 0);
				ProcGenController.GetComponent<ProcGen>().Generate(edgePos.x + initialDistance * -3, edgePos.y + initialDistance * -2);
				break;
			}
			
			Destroy (this.gameObject);
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

