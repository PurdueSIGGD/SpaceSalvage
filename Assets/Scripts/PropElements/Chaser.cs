using UnityEngine;
using System.Collections;

public class Chaser : MonoBehaviour {
	GameObject Player, barrel;
	bool focused;
	// Use this for initialization
	void Start () {
		barrel = this.transform.FindChild("Barrel").gameObject;
		Player = GameObject.Find("Player");
	}
	void Focus(bool b) {
		if (!focused || Vector3.Distance(this.transform.position,Player.transform.position) > 4) focused = b;
	}
	// Update is called once per frame
	void Update () {

		this.transform.FindChild("Beam").transform.eulerAngles = new Vector3(0,0,this.transform.FindChild("Beam").transform.eulerAngles.z + Time.deltaTime * 150);
		if (focused) {

			float thetaersnenig;
			Vector3 pz = this.transform.position;
			thetaersnenig = (Mathf.Atan( ((pz.y - (Player.transform.position.y)) /(pz.x - Player.transform.position.x)))); //angle from mouse to me, formatting later
			thetaersnenig = thetaersnenig/2;
			if (thetaersnenig < 0) {
				thetaersnenig+= Mathf.PI/2;
			}
			if (pz.y - Player.transform.position.y < 0) {
				thetaersnenig+= Mathf.PI/2;
			}
			thetaersnenig = thetaersnenig * 2 * Mathf.Rad2Deg; //fooooormatting
			//print(thetaersnenig);
			//print( new Vector2(Mathf.Cos(Mathf.Deg2Rad * (thetaersnenig+180)),Mathf.Sin(Mathf.Deg2Rad * (thetaersnenig+180))));
			
			/*if (barrel.transform.eulerAngles.z != thetaersnenig) {
				barrel.transform.eulerAngles = new Vector3(0,0,barrel.transform.eulerAngles.z+Time.deltaTime);
			}*/
			barrel.transform.eulerAngles = new Vector3(0,0,thetaersnenig);
			this.rigidbody2D.AddForce(6 * Time.deltaTime * (Player.transform.position - this.transform.position) * Vector3.Distance(Player.transform.position, this.transform.position));
		} else {
			this.rigidbody2D.AddForce(-1 * this.rigidbody2D.velocity);
			barrel.transform.eulerAngles = new Vector3(0,0,barrel.transform.eulerAngles.z+ 30 * Time.deltaTime);
		}

	}
}
