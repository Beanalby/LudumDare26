using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameDriver : MonoBehaviour {

    public static GameDriver instance;

    public LevelInfo currentLevel;
    Dictionary<PlantType, int> plants;

    void Start() {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if(currentLevel == null) {
            currentLevel = GameConfig.instance.levels[0];
        }
        plants = new Dictionary<PlantType, int>();
    }
    public void OnLevelWasLoaded() {
        plants = new Dictionary<PlantType, int>();
    }

    bool IsAllComplete() {
        foreach(LevelInfo level in GameConfig.instance.levels) {
            if(!level.isComplete) {
                return false;
            }
        }
        return true;
    }
    void CheckLevelComplete() {
        if(IsLevelComplete()) {
            Debug.Log("Loading next level!");
            currentLevel.isComplete = true;
            currentLevel = null;
            if(IsAllComplete()) {
                Application.LoadLevel("Finish");
            } else {
                Application.LoadLevel("StageSelect");
            }
        }
    }
    bool IsLevelComplete() {
        foreach(KeyValuePair<PlantType, int> pair in currentLevel.target) {
            if(!plants.ContainsKey(pair.Key)) {
                return false;
            }
            if(plants[pair.Key] < pair.Value) {
                return false;
            }
        }
        return true;
    }
    public void LoadLevel(LevelInfo level) {
        currentLevel = level;
        Application.LoadLevel("Garden");
    }
    public void PlantSpawned(PlantType type) {
        if(!plants.ContainsKey(type)) {
            plants[type] = 0;
        }
        plants[type]++;
        CheckLevelComplete();
    }
}
