using UnityEngine;
using System.Collections;

public class OxygenStation : MonoBehaviour {

	public float oxygenAmt = 50;
	private GameObject Player;

	// Use this for initialization
	void Start () {

		//Recognize the player in the game
		Player = GameObject.Find("Player"); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerStay2D(Collider2D col){

		if(col.gameObject == Player){


			// variable ctrl is the CollisionController component of Player
			PlayerCollisionController ctrl = (PlayerCollisionController)Player.GetComponent ("PlayerCollisionController");


			// The player has to be disconnected from the tube controller (and losing oxygen)
			// and his oxygen must be lower than max
			// and the station must not have run out of its oxygen supply
		// all in order for the Oxygen Station  to actually do anything.
			if(((TubeController)(ctrl).GetComponent("TubeController")).ejected && ctrl.oxy < ctrl.startingoxy && oxygenAmt > 0){

				// store the delta of the time in one variable
				float d = Time.deltaTime*5;

				// in both if-statements, the amount of oxygen stored in the Station 
				// decreases upon use

				if(d + ctrl.oxy > ctrl.startingoxy){
					oxygenAmt -= ctrl.startingoxy - ctrl.oxy;
					ctrl.oxy = ctrl.startingoxy;
				}

				else{
					
					oxygenAmt -= d;
					ctrl.oxy += d;

				}

			}


		}



	}
}
