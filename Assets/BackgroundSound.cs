using UnityEngine;
using System.Collections;

public class BackgroundSound : MonoBehaviour {
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
    public AudioClip mOne;
    public AudioClip mTwo;
    public AudioClip mThree;
    public AudioClip mFour;
    public AudioClip mFive;
    public bool testPoint;
    public bool OriginalSolution;
    private bool firstPlay;

    public int timeInSeconds;
    private float timer;
    private bool isCommercial;
    private ArrayList commercials;
    private ArrayList tracks;
    private ArrayList aggregateMusic;
	// Use this for initialization
	void Start () {
		float audioVol = 1;
		if (PlayerPrefs.HasKey("audioVol")) {
			audioVol = PlayerPrefs.GetFloat("audioVol");
		} else {
			PlayerPrefs.SetFloat("audioVol", audioVol);
		}
		this.GetComponent<AudioSource>().volume = audioVol;
        commercials = new ArrayList();
        commercials.Add(one);
        commercials.Add(two);
        commercials.Add(three);
		commercials.Add(four);
		commercials.Add(five);
		commercials.Add(six);
		commercials.Add(seven);
		commercials.Add(eight);
		commercials.Add(nine);
		commercials.Add (ten);
        aggregateMusic = new ArrayList();
        tracks = new ArrayList();
        tracks.Add(mOne);
        tracks.Add(mTwo);
        tracks.Add(mThree);
        tracks.Add(mFour);
        tracks.Add(mFive);
        OriginalSolution = false;
        timeInSeconds = 180;
        timer = Time.fixedTime;
        testPoint = false;

        firstPlay = true;

        foreach (AudioClip sound in tracks)
        {
            aggregateMusic.Add(sound); //Seven Duplicates balnce out a 3:1 Ratio for 11 commercials
            aggregateMusic.Add(sound); //Remove Comment when all commercials in place
            aggregateMusic.Add(sound); //Remove Comment when all commercials in place
            aggregateMusic.Add(sound); //Remove Comment when all commercials in place
            aggregateMusic.Add(sound); //Remove Comment when all commercials in place
            aggregateMusic.Add(sound); //Remove Comment when all commercials in place
            aggregateMusic.Add(sound); //Remove Comment when all commercials in place
        }
        foreach (AudioClip sound in commercials)
        {
            aggregateMusic.Add(sound);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        #region Old Code
        /*
        if (!this.GetComponent<AudioSource>().isPlaying) //OST Only
        {
            this.GetComponent<AudioSource>().clip = (AudioClip)tracks[ChooseTrack()];
            this.GetComponent<AudioSource>().loop = true;
            this.GetComponent<AudioSource>().Play();
        }
        
        for (int i = 10; i > 0; i--) //Also Ads Only
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
        
        if (testPoint) //Ads Only
        {
            isCommercial = true;
            this.GetComponent<AudioSource>().Pause();
            AudioClip derp = (AudioClip)commercials[ChooseTrack()];
            this.GetComponent<AudioSource>().PlayOneShot(derp);
            //WaitForCommercial(derp);
            isCommercial = false;
            this.GetComponent<AudioSource>().UnPause();
            testPoint = false;
        }
        */
        #endregion
        
        if (OriginalSolution) //Currently Broken
        {
            if (!this.GetComponent<AudioSource>().isPlaying && !isCommercial) //OST Controller
            {
                this.GetComponent<AudioSource>().clip = (AudioClip)tracks[ChooseTrack(tracks)];
                this.GetComponent<AudioSource>().loop = true;
                this.GetComponent<AudioSource>().Play();
            }
            else if (testPoint) //Test Controller
            {
                isCommercial = true;
                this.GetComponent<AudioSource>().Pause();
                AudioClip derp = (AudioClip)commercials[ChooseTrack(commercials)];
                this.GetComponent<AudioSource>().PlayOneShot(derp);
                //WaitForCommercial(derp);
                isCommercial = false;
                this.GetComponent<AudioSource>().UnPause();
                testPoint = false;
            }
            else //Ads Controller
            {
                for (int i = 10; i > 0; i--)
                {
                    if (timer >= (timeInSeconds * i))
                    {
                        if (RollD3() == 0)
                        {
                            isCommercial = true;
                            this.GetComponent<AudioSource>().Pause();
                            AudioClip derp = (AudioClip)commercials[ChooseTrack(commercials)];
                            this.GetComponent<AudioSource>().PlayOneShot(derp);
                            //WaitForCommercial(derp);
                            isCommercial = false;
                            this.GetComponent<AudioSource>().UnPause();

                            /*
                            this.GetComponent<AudioSource>().Pause();
                            this.GetComponent<AudioSource>().PlayOneShot((AudioClip)commercials[ChooseTrack()]);
                            this.GetComponent<AudioSource>().UnPause();
                            */
                        }
                        else
                        {
                            timer = Time.fixedTime - (timeInSeconds * i);
                        }
                    }
                }
            }
        }

        if (!OriginalSolution) //Not Broken (yet)
        {
            if (!this.GetComponent<AudioSource>().isPlaying && firstPlay) //OST Controller
            {
                this.GetComponent<AudioSource>().clip = (AudioClip)tracks[ChooseTrack(tracks)];
                this.GetComponent<AudioSource>().loop = false;
                this.GetComponent<AudioSource>().Play();
                firstPlay = false;
            }
            
            if (!this.GetComponent<AudioSource>().isPlaying && !firstPlay) //Easy Way Out
            {
                this.GetComponent<AudioSource>().clip = (AudioClip)aggregateMusic[ChooseTrack(aggregateMusic)];
                this.GetComponent<AudioSource>().loop = false;
                this.GetComponent<AudioSource>().Play();
            }
        }
	}

    private int ChooseTrack(ArrayList list)
    {
        int value = 0;
        value = Random.Range(0, list.Count);
        return value;
    }

    private int RollD3()
    {
        int value = 0;
        value = Random.Range(0, 3);
        return value;
    }
}
