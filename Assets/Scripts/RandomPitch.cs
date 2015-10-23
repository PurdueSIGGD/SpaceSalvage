using UnityEngine;
using System.Collections;

public class RandomPitch : MonoBehaviour {

	// Use this for initialization

    public float RandomPitchValue()
    {
        float result = 0;

        //Find random value
        float randomTemp = Random.Range(-300, 300);

        //convert to proper format (-3.00 to 3.00)
        result = randomTemp / 100;

        return result;
    }

	public float RandomPitchValue(int lowerBound, int upperBound)
    {
        float result = 0;

        //Define range of Pitch (-300 to 300)
        if (lowerBound < -300)
        {
            lowerBound = -300;
        }
        if (upperBound > 300)
        {
            upperBound = 300;
        }

        //Find random value
        float randomTemp = Random.Range(lowerBound, upperBound);

        //convert to proper format (-3.00 to 3.00)
        result = randomTemp / 100;

        return result;
    }
}
