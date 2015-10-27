using UnityEngine;
using System.Collections;
//Currently NOT Tested!!! Please Shout at Bill if this is breaking anything
public class RopeAudioController : MonoBehaviour {
    public AudioClip extend;
    public AudioClip extendFaster;

	private float timer;

	public bool kextend;
	public bool kretract;
	private bool switcheroo;

	static float ropetime = 1.880f;

    void Start()
    {

	}

	void FixedUpdate () {
		if (kextend || kretract) {
			timer += Time.deltaTime;
		//	if(!this.GetComponent<AudioSource>().isPlaying) print(this.GetComponent<AudioSource>().isPlaying);
			if (this.GetComponent<AudioSource>().isPlaying || timer > ropetime) { 
				if (timer > ropetime) {
					if (!switcheroo) {
						//this.transform.FindChild("Looper").GetComponent<AudioSource>().clip = extendFaster;
						this.transform.FindChild("Looper").GetComponent<AudioSource>().Play();
						this.GetComponent<AudioSource>().Stop();
					}
					switcheroo = true;
					
				} else {
					//this.GetComponent<AudioSource>().clip = extend;
					switcheroo = false;
				}
			} else {
				if (!switcheroo) {
					this.GetComponent<AudioSource>().clip = extend;
					this.GetComponent<AudioSource>().Play();
				}
			}
		} else {
			switcheroo = false;
			this.transform.FindChild("Looper").GetComponent<AudioSource>().Stop();
			this.GetComponent<AudioSource>().Stop();
			timer = 0;
		}
       /* if ((kextend || kretract) && !this.GetComponent<AudioSource>().isPlaying)
        {
			this.GetComponent<AudioSource>().clip = extend;
			this.GetComponent<AudioSource>().Play();
            
            if (timer == 0)
            {
                timer = Time.deltaTime;
            }
        }
		else if ((kextend || kretract) &&  this.GetComponent<AudioSource>().isPlaying && (cClipTimer <= timer))
        {
            this.GetComponent<AudioSource>().clip = extendFaster;
            this.GetComponent<AudioSource>().loop = true;
            this.GetComponent<AudioSource>().Play();
        }
        else
        {
            this.GetComponent<AudioSource>().Stop();
            timer = 0f;
        }*/
	}
}
