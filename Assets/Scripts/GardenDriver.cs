using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Currency {
    public int seed;
    public int water;
    public int sun;
    public Currency(int seed, int water, int sun) {
        this.seed = seed;
        this.water = water;
        this.sun = sun;
    }
    public bool canAfford(Currency item) {
        return this.seed >= item.seed
            && this.water >= item.water
            && this.sun >= item.sun;
    }
    public static Currency operator +(Currency funds, Currency stuff) {
        return new Currency(funds.seed + stuff.seed,
            funds.water + stuff.water,
            funds.sun + stuff.sun);
    }
    public static Currency operator -(Currency funds, Currency cost) {
        return new Currency(funds.seed - cost.seed,
            funds.water - cost.water,
            funds.sun - cost.sun);
    }
}

public class GardenDriver : MonoBehaviour {

    private Currency funds;
    Spawnable buying;

    int clickMask;
    int groundLayer;

    public void Start() {
        funds = new Currency(10, 0, 0);

        buying = null;
        groundLayer = LayerMask.NameToLayer("Ground");
        clickMask = 1 << groundLayer | 1 << LayerMask.NameToLayer("Pickup");
    }

    public void Update() {
        if(Input.GetMouseButtonDown(0)) {
            // see if we're over ground
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickMask)) {
                if(hit.collider.gameObject.layer == groundLayer) {
                    if(buying != null && funds.canAfford(buying.cost)) {
                        SpawnPlant(hit.collider.gameObject);
                    }
                } else {
                    hit.collider.gameObject.SendMessage("Clicked");
                }
            }
        }
    }
    public void OnGUI() {
        int pos = 0;
        int buttonSize = 25;
        foreach(Spawnable s in GameConfig.instance.spawnables) {
            string msg = "Buy + " + s.type + "(" + s.cost.seed + ", "
                + s.cost.sun + ", " + s.cost.water + ")";
            GUI.enabled = funds.canAfford(s.cost);
            if(GUI.Button(new Rect(0, pos, 200, buttonSize), msg)) {
                if(buying == s) {
                    buying = null;
                } else {
                    buying = s;
                }
            }
            pos += buttonSize;
        }
        GUI.enabled = true;
        GUI.Label(new Rect(Screen.width - 100, 0, 100, 25), "Seeds: " + funds.seed);

        string target = "target: ";
        if(GameDriver.instance.currentLevel != null) {
            foreach(KeyValuePair<PlantType, int> pair in GameDriver.instance.currentLevel.target) {
                target += pair.Key + " (" + pair.Value + ") ";
            }
            GUI.Label(new Rect(0, Screen.height - 50, Screen.width, 50), target);
        }
    }

    public void SpawnPlant(GameObject ground) {
        funds -= buying.cost;
        Plant plant = (Instantiate(buying.prefab) as GameObject).GetComponent<Plant>();
        plant.Attach(ground.GetComponent<Ground>());
        GameDriver.instance.PlantSpawned(plant.type);
        buying = null;
    }
    public void AddFunds(Currency stuff) {
        funds = funds + stuff;
    }
    public Spawnable getSpawnable(PlantType type) {
        foreach(Spawnable s in GameConfig.instance.spawnables) {
            if(s.type == type) {
                return s;
            }
        }
        return null;
    }
}
