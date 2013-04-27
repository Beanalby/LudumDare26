using UnityEngine;
using System;
using System.Collections;

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
    public static Currency operator -(Currency funds, Currency cost) {
        return new Currency(funds.seed - cost.seed,
            funds.water - cost.water,
            funds.sun - cost.sun);
    }
}

[Serializable]
public class Spawnable {
    public PlantType type;
    public GameObject prefab;
    public Currency cost;
}

public class GardenDriver : MonoBehaviour {

    public Spawnable[] spawnables;
    private Currency funds;
    Spawnable buying;

    int groundLayer;

    public void Start() {
        funds = new Currency(4, 0, 0);

        buying = null;
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    public void Update() {
        if(Input.GetMouseButtonDown(0) && buying != null) {
            // see if we're over ground
            Debug.Log("Shooting ray at layer " + groundLayer);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, groundLayer)) {
                Debug.Log("Hit " + hit.collider.name);
                if(funds.canAfford(buying.cost)) {
                    funds -= buying.cost;
                    GameObject obj = Instantiate(buying.prefab) as GameObject;
                    obj.GetComponent<Plant>().Attach(hit.collider.GetComponent<Ground>());
                    buying = null;
                }
            }
        }
    }
    public void OnGUI() {
        int pos = 0;
        int buttonSize = 25;
        foreach(Spawnable s in spawnables) {
            string msg = "Buy + " + s.type + "(" + s.cost.seed + ", "
                + s.cost.sun + ", " + s.cost.water + ")";
            GUI.enabled = funds.canAfford(s.cost);
            if(GUI.Button(new Rect(0, pos, 200, buttonSize), msg)) {
                if(buying == s) {
                    Debug.Log("Clearing buying");
                    buying = null;
                } else {
                    Debug.Log("Buying " + s.type);
                    buying = s;
                }
            }
            pos += buttonSize;
        }
    }
    public Spawnable getSpawnable(PlantType type) {
        foreach(Spawnable s in spawnables) {
            if(s.type == type) {
                return s;
            }
        }
        return null;
    }
}
