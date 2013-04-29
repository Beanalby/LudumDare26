using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Spawnable {
    public PlantType type;
    public GameObject prefab;
    public Currency cost;
    public AudioClip selected;
}

public class LevelInfo {
    public Dictionary<PlantType, int> target;
    public Currency startingFunds;
    public bool isComplete;
    public List<PlantType> disabled;

    public LevelInfo(Dictionary<PlantType, int> target) : this(target, new Currency(0,0,0), new PlantType[] {}) { }
    public LevelInfo(Dictionary<PlantType, int> target, PlantType[] disabled): this(target, new Currency(0, 0, 0), disabled) { }
    public LevelInfo(Dictionary<PlantType, int> target, Currency funds): this(target, funds, new PlantType[] {}){ }
    public LevelInfo(Dictionary<PlantType, int> target, Currency startingFunds, PlantType[] disabled) {
        this.target = target;
        this.startingFunds = startingFunds;
        isComplete = false;
        this.disabled = new List<PlantType>(disabled);
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
            new LevelInfo(
                new Dictionary<PlantType,int>() { { PlantType.Tiny, 3 } },
                new Currency(1, 0, 0),
                new PlantType[] { PlantType.Medium, PlantType.Flower }),
            new LevelInfo(
                new Dictionary<PlantType,int>() { { PlantType.Tiny, 2}, {PlantType.Medium, 2}},
                new Currency(2, 0, 0),
                new PlantType[] { PlantType.Flower }),
            new LevelInfo(
                new Dictionary<PlantType,int>() { {PlantType.Flower, 3}, {PlantType.Medium, 2}},
                new Currency(4, 0, 0))
        };
    }
}
