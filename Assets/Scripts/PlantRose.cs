using UnityEngine;
using System.Collections;

public class PlantRose : Plant {

     new void Start () {
         base.Start();
         actionCooldown = 5f;
         type = PlantType.Medium;
    }

    public override void DoAction() {
        Debug.Log(name + " doing special rose thing!");
        StartGrow();
    }
}
