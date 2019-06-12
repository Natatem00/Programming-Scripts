using System.Collections;
using UnityEngine;

public class ChargeBattery : MonoBehaviour, Pickable {

    Flashlight flashlight;

    void Start()
    {
        flashlight = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<Flashlight>();
    }

    void Pickable.PickUp()
    {
        flashlight.AddBattery(Random.Range(5f, 20f));
        Destroy(gameObject);
    }
}
