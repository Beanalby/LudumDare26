using UnityEngine;
using System.Collections;

public enum PickupType { seed, water, sun };

public delegate void pickupEventHandler(Pickup pickup);

public class Pickup : MonoBehaviour {

    public PickupType type;
    public Currency value;
    public AudioClip pickupSound;
    private float spinSpeed = 100f;

    public pickupEventHandler pickupListeners;

    public void Update() {
        Vector3 angles = transform.localRotation.eulerAngles;
        angles.y += spinSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(angles);
    }

    public void Clicked() {
        GardenDriver driver = GameObject.FindObjectOfType(typeof(GardenDriver)) as GardenDriver;
        driver.AddFunds(value);
        if(pickupListeners != null) {
            pickupListeners(this);
        }
        if (pickupSound) {
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
        }
        Destroy(gameObject);
    }
}
