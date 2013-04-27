 using UnityEngine;
using System.Collections;

public class PlantTiny : Plant {

    private float seedCooldown = 3f;
    private float lastSeed = -1f;

     void Start () {
         type = PlantType.Medium;
         lastSeed = Time.time;
    }

    public override void Boost() {
    }
    public override void Grow() {
        if(Time.time - lastSeed >= seedCooldown) {
            Debug.Log(name + " Making seed!");
            lastSeed = Time.time;
        }
    }
}
