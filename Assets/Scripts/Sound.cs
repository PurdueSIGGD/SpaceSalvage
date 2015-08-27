using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

	//AudioClip[] ItemCollision;
	public AudioClip motion = AudioClip.Create("00 - bump2", 50000, 2, 100, true, true);


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)) {
			GetComponent<AudioSource>().clip = motion;
			GetComponent<AudioSource>().Play();

			//if(audio.isPlaying) {
				//yield WaitForSeconds(motion.length());
			//}

		}
		else {
			GetComponent<AudioSource>().Pause();
		}
	}
}
