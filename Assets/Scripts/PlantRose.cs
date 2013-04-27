using UnityEngine;
using System.Collections;

public class PlantRose : Plant {

    private float pulseRange = 1.5f;
    private float boostAmount = .5f;

    private int plantMask;
     new void Start () {
         base.Start();
         actionCooldown = 1.5f;
         type = PlantType.Medium;
         plantMask = 1 << LayerMask.NameToLayer("Plant");
    }

    public override void DoAction() {
        SendBoost();
        StartGrow();
    }
    private void SendBoost() {
        foreach(Collider plant in Physics.OverlapSphere(transform.position, pulseRange, plantMask)) {
            plant.GetComponent<Plant>().Boost(boostAmount);
        }
    }
}
