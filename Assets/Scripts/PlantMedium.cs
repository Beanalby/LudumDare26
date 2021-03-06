using UnityEngine;
using System.Collections;

public class PlantMedium : Plant {

    public GameObject seedPrefab;
    private Transform seedPoint;

    new void Start () {
        base.Start();
        actionCooldown = 3f;
        seedPoint = transform.Find("SeedPoint");
    }

    public override void DoAction() {
        GameObject seed = Instantiate(seedPrefab, seedPoint.position, seedPoint.rotation) as GameObject;
        pickupEventHandler onPickup = delegate(Pickup obj) {
            StartGrow();
        };
        seed.GetComponent<Pickup>().pickupListeners += onPickup;
        pc.gameObject.SetActive(false);
    }
}
