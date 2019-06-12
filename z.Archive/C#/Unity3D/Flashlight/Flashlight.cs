using System.Collections;
using UnityEngine;

public class Flashlight : MonoBehaviour {

    [SerializeField]
    Light spotLight;                        // flashlight light

    [SerializeField]
    AudioClip flashOnSound;                 
    [SerializeField]
    AudioClip flashOffSound;
    [SerializeField]
    AudioClip flashBlinkSound;

    [SerializeField]
    AudioSource audioSource;

    float maxBattery = 100f;
    [SerializeField]
    float currentBattery = 0;
    float batteryLow = 0.5f;                // how quickly the battery charge is going to zero

    float maxIntensity = 1f;                // the max light intensity  
    float maxRange = 30f;                   // the max light range 
    float maxAngle = 50f;                   // the max light angle  

    float timeBetweenBlinking;              // time between light blinking

    float batteryWidthBox;  
    bool blinked = false;               


    void Start () {
        spotLight.enabled = false;
        currentBattery = maxBattery;
	}
	
	void Update ()
    {
        batteryWidthBox = (Screen.width / 4) * (currentBattery / maxBattery);

        SwitchFlashlight();
        LightSettings();
    }

    void LightSettings()
    {
        // change the light in depends on the battery charge
        if (spotLight.enabled && currentBattery > 0)
        {
            spotLight.intensity = maxIntensity * currentBattery / 100f + maxIntensity / 2;
            spotLight.range = maxRange * currentBattery / 100f + maxRange;
            spotLight.spotAngle = maxAngle * currentBattery / 100f + maxAngle / 4;

            // light blinking
            if (currentBattery < 25f && !blinked)
            {
                timeBetweenBlinking = Random.Range(0, currentBattery / 5);
                StartCoroutine(BlinkLight());
            }
        }
        else
        {
            spotLight.enabled = false;
        }

        // lowers the battery charge
        if (spotLight.enabled)
        {
            currentBattery -= batteryLow * Time.deltaTime;
        }
    }

    void SwitchFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // play flashlight switch sound
            if (spotLight.enabled)
            {
                audioSource.PlayOneShot(flashOffSound);
            }
            else
            {
                audioSource.PlayOneShot(flashOnSound);
            }

            spotLight.enabled = !spotLight.enabled;                 // on - off flashlight
        }
    }

    IEnumerator BlinkLight()
    {
        if (spotLight.enabled)
        {
            blinked = true;
            yield return new WaitForSeconds(timeBetweenBlinking);
            if (spotLight.enabled)
            {
                audioSource.PlayOneShot(flashBlinkSound);
                spotLight.enabled = false;
                yield return new WaitForSeconds(0.1f);
                spotLight.enabled = true;
            }
            blinked = false;
        }
    }

    public void AddBattery(float addbattery)
    {
        currentBattery += addbattery;
        ChechBatteryLimit();
    }

    private void ChechBatteryLimit()
    {
        if (currentBattery > maxBattery)
        {
            currentBattery = maxBattery;
        }
        else if (currentBattery < 0)
        {
            currentBattery = 0;
        }
    }

    // GUI for flashlight
    void OnGUI()
    {
        GUI.Box(new Rect(5f, 25f, batteryWidthBox, 20), "Battery");
    }
}
