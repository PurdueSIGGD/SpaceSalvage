using UnityEngine;
using System.Collections;

public class OSTController : MonoBehaviour {
    public AudioClip one;
    public AudioClip two;
    public AudioClip three;
    public AudioClip four;
    public AudioClip five;

    private ArrayList tracks;
    private AwesomeCommercials ads;

    void Start()
    {
        tracks = new ArrayList();
        tracks.Add(one);
        tracks.Add(two);
        tracks.Add(three);
        tracks.Add(four);
        tracks.Add(five);
    }

    private int ChooseTrack()
    {
        int value = 0;
        value = Random.Range(0, 5);
        return value;
    }

    void Update()
    {
        if (!this.GetComponent<AudioSource>().isPlaying)
        {
            this.GetComponent<AudioSource>().clip = (AudioClip)tracks[ChooseTrack()];
            this.GetComponent<AudioSource>().loop = true;
            this.GetComponent<AudioSource>().Play();
        }
    }

}
