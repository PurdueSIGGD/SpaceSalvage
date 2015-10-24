using UnityEngine;
using System.Collections;

public class AwesomeCommercials : MonoBehaviour {
    public AudioClip one;
    public AudioClip two;
    public AudioClip three;
    public AudioClip four;
    public AudioClip five;
    public AudioClip six;
    public AudioClip seven;
    public AudioClip eight;
    public AudioClip nine;
    public AudioClip ten;
    public bool testPoint;
    private bool isCommercial;

    public int timeInSeconds;
    private float timer;
    private ArrayList commercials;
	// Use this for initialization
	void Start () {
        commercials = new ArrayList();
        commercials.Add(one);
        commercials.Add(two);
        commercials.Add(three);

        timeInSeconds = 180;
        timer = Time.fixedTime;
	}
	
	// Update is called once per frame
	void Update () {
	    for (int i = 10; i > 0; i--)
        {
            if (timer >= (timeInSeconds * i))
            {
                if (RollD3() == 0)
                {
                    this.GetComponent<AudioSource>().Pause();
                    this.GetComponent<AudioSource>().PlayOneShot((AudioClip)commercials[ChooseTrack()]);
                    this.GetComponent<AudioSource>().UnPause();
                }
                else
                {
                    timer = Time.fixedTime - (timeInSeconds * i);
                }
            }
        }

        if (testPoint)
        {
            isCommercial = true;
            this.GetComponent<AudioSource>().Pause();
            AudioClip derp = (AudioClip)commercials[ChooseTrack()];
            this.GetComponent<AudioSource>().PlayOneShot(derp);
            WaitForCommercial(derp);
            isCommercial = false;
            this.GetComponent<AudioSource>().UnPause();
            testPoint = false;
        }
	}

    private IEnumerator WaitForCommercial(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
    }

    private int ChooseTrack()
    {
        int value = 0;
        value = Random.Range(0, commercials.Count);
        return value;
    }

    public bool IsAiring()
    {
        return isCommercial;
    }

    private int RollD3()
    {
        int value = 0;
        value = Random.Range(0, 3);
        return value;
    }
}
