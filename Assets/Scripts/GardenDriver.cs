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

    public AudioClip plantAtached;

    private Currency funds;
    Spawnable buying;
    private float spinSpeed = 2f;

    private float gardenCompleteStart = -1;
    int clickMask;
    int groundLayer;
    private Material whiteout;
    private float levelStart;
    private float fadeinDuration = 1f;

    public void Start() {
        funds = new Currency(10, 0, 0);

        buying = null;
        groundLayer = LayerMask.NameToLayer("Ground");
        clickMask = 1 << groundLayer | 1 << LayerMask.NameToLayer("Pickup");
        whiteout = transform.FindChild("Whiteout").renderer.material;
        whiteout.color = new Color(1, 1, 1, 1);
        levelStart = Time.time;
    }

    public void Update() {
        if (gardenCompleteStart == -1) {
            if (fadeinDuration != -1) {
                float percent = Time.time - levelStart;
                if (percent >= 1) {
                    whiteout.color = new Color(1, 1, 1, 0);
                    fadeinDuration = -1;
                } else {
                    whiteout.color = new Color(1, 1, 1, 1 - percent);
                }
            }
            if (Input.GetMouseButtonDown(0)) {
                // see if we're over ground
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickMask)) {
                    if (hit.collider.gameObject.layer == groundLayer) {
                        if (buying != null && funds.canAfford(buying.cost)) {
                            SpawnPlant(hit.collider.gameObject);
                        }
                    } else {
                        hit.collider.gameObject.SendMessage("Clicked");
                    }
                }
            }
        } else {
            float percent = (Time.time - gardenCompleteStart) / GameDriver.instance.gardenCompleteDuration;
            whiteout.color = new Color(1, 1, 1, percent);
            transform.RotateAround(Vector3.zero, Vector3.up, -(Time.time - gardenCompleteStart) * spinSpeed);
        }
    }
    public void OnGUI() {
        if (gardenCompleteStart != -1) {
            return;
        }
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
                    AudioSource.PlayClipAtPoint(s.selected, Camera.main.transform.position);
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
        AudioSource.PlayClipAtPoint(plantAtached, Camera.main.transform.position);
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
    public void GardenComplete() {
        gardenCompleteStart = Time.time;
    }
}
