using UnityEngine;
using System.Collections;

public class DestructionStation : MonoBehaviour {
	//Aka "lasers"
	//Damage player and cut cable


	public float damagerate = 1;	
	private GameObject Player;
	public GameObject particle;
	private float laserVolume;
	private bool playing;

	// Use this for initialization
	void Start () {
		this.gameObject.layer = 14;
		//Recognize the player in the game
		Player = GameObject.Find("Player"); 
		laserVolume = 0;
	}

	void OnTriggerStay2D(Collider2D col){

		if (col.gameObject == Player) {
			//if (!GameObject.Find("LaserHit").GetComponent<AudioSource>().isPlaying) GameObject.Find("LaserHit").GetComponent<AudioSource>().Play();


			attack ();
			GameObject.Find ("Camera").SendMessage ("ShakeOnOff", true); //shaking
			GameObject.Find("Camera").SendMessage("Shake",1);

			return;
		}
		if (col.GetComponent<JointScript> () != null) {
			col.SendMessage("BrokenJoint");
		}
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject == Player) {
			playing = true;
			GameObject.Find ("Camera").SendMessage ("ShakeOnOff", true);

				attack ();
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject == Player) {
			playing = false;
			//GameObject.Find("LaserHit").GetComponent<AudioSource>().Stop();
			GameObject.Find("Camera").SendMessage("ShakeOnOff", false);
		}
	}

	void attack(){
		Player.SendMessage("changeHealth", 10 *  Time.deltaTime * -1 * damagerate);
	}

	// Update is called once per frame
	void Update () {
//		if (laserVolume != 0) print(laserVolume);
		if (playing) {
			if (laserVolume < 1) { 
				laserVolume+=Time.deltaTime * 10;
			} else  {
				laserVolume = 1;
			}
			if (!GameObject.Find("LaserHit").GetComponent<AudioSource>().isPlaying) {
				GameObject.Find("LaserHit").GetComponent<AudioSource>().Play();
			}
			
		} else {
			if (laserVolume > 0) {
				laserVolume-=Time.deltaTime * 10;
			} else {
				laserVolume = 0;
				//GameObject.Find("LaserHit").GetComponent<AudioSource>().Stop();
			}
			
		}
		if (/*GameObject.Find("LaserHit").GetComponent<AudioSource>().volume != 0*/ laserVolume != 0) GameObject.Find("LaserHit").GetComponent<AudioSource>().volume = laserVolume;
		//transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 5);
	}
}
