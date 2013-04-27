using UnityEngine;
using System.Collections;

public enum PickupType { seed, water, sun };

public delegate void pickupEventHandler(Pickup pickup);

public class Pickup : MonoBehaviour {

    public PickupType type;
    public Currency value;
    
    public pickupEventHandler pickupListeners;

    public void Clicked() {
        GardenDriver driver = GameObject.FindObjectOfType(typeof(GardenDriver)) as GardenDriver;
        driver.AddFunds(value);
        if(pickupListeners != null) {
            pickupListeners(this);
        }
        Destroy(gameObject);
    }
}
