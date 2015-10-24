using UnityEngine;
using System.Collections;
//Currently NOT Tested!!! Please Shout at Bill if this is breaking anything
public class RopeAudioController : MonoBehaviour {
    public AudioClip extend;
    public AudioClip extendFaster;
    public AudioClip retract;
    public AudioClip retractFaster;

    public float cClipTimer;
    private float timer;

    public KeyCode kextend = KeyCode.LeftShift;
    public KeyCode kretract = KeyCode.LeftControl;

    void Start()
    {
        cClipTimer = 1.5f;
    }

	void FixedUpdate () {
        if ((Input.GetKey(kextend) || Input.GetKey(kretract)) && !this.GetComponent<AudioSource>().isPlaying)
        {
            this.GetComponent<AudioSource>().PlayOneShot(extend);
            
            if (timer == 0f)
            {
                timer = Time.deltaTime;
            }
        }
        else if ((Input.GetKey(kextend) || Input.GetKey(kretract)) && this.GetComponent<AudioSource>().isPlaying && (cClipTimer <= timer))
        {
            this.GetComponent<AudioSource>().clip = extendFaster;
            this.GetComponent<AudioSource>().loop = true;
            this.GetComponent<AudioSource>().Play();
        }
        else
        {
            this.GetComponent<AudioSource>().Stop();
            timer = 0f;
        }
	}
}
