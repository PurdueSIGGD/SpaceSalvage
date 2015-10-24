using UnityEngine;
using System.Collections;

public class MenuPurchaseSounds : MonoBehaviour {
    public AudioClip one;
    public AudioClip two;
    public AudioClip three;
    private ArrayList coins;
    private int previousCash;
    private int currentCash;
    private UpgradeScript item;
	// Use this for initialization
	void Start () {
        coins = new ArrayList();
        coins.Add(one);
        coins.Add(two);
        coins.Add(three);
   }
	
	// Update is called once per frame
	void Update () {
        previousCash = currentCash;
        currentCash = item.ReportCash();
        
        if (currentCash < previousCash)
        {
            this.GetComponent<AudioSource>().PlayOneShot((AudioClip)coins[ChooseTrack()]);
        }
	}

    private int ChooseTrack()
    {
        int value = 0;
        value = Random.Range(0, 2);
        return value;
    }
}
