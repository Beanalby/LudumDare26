using UnityEngine;
using System.Collections;

public enum PlantType { None, Tiny, Medium, Rose };

public abstract class Plant : MonoBehaviour {

    [HideInInspector]
    public PlantType type;

    public abstract void Boost();
    public abstract void Grow();

    public void Attach(Ground ground) {
        Debug.Log("Attaching to " + ground);
        transform.parent = ground.getPlantPoint();
        ground.plant = this;
    }
}
