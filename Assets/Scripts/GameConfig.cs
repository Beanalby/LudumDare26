using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Spawnable {
    public PlantType type;
    public GameObject prefab;
    public Currency cost;
}

public class LevelInfo {
    public Dictionary<PlantType, int> target;
    public Currency startingFunds;
    public bool isComplete;
    
    public LevelInfo(Dictionary<PlantType, int> target) : this(target, new Currency(0,0,0)) {
    }

    public LevelInfo(Dictionary<PlantType, int> target, Currency startingFunds) {
        this.target = target;
        this.startingFunds = startingFunds;
        isComplete = false;
    }
}


public class GameConfig : MonoBehaviour {
    public static GameConfig instance;
    public Spawnable[] spawnables;
    public LevelInfo[] levels;
    
    void Awake() {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        levels = new LevelInfo[] {
            new LevelInfo(new Dictionary<PlantType,int>() {
                { PlantType.Tiny, 1 } }),
            new LevelInfo(new Dictionary<PlantType,int>() {
                { PlantType.Tiny, 2}, {PlantType.Medium, 2} })
        };


    }
}
