using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameDriver : MonoBehaviour {

    public static GameDriver instance;
    public AudioClip gardenComplete;

    public bool IsGardenComplete;
    public float gardenCompleteDuration = 3;

    public LevelInfo currentLevel;
    Dictionary<PlantType, int> plants;
    private GameObject gardenDriver;

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
        InitLevel();
    }
    public void OnLevelWasLoaded() {
        InitLevel();
    }

    void InitLevel() {
        IsGardenComplete = false;
        plants = new Dictionary<PlantType, int>();
        gardenDriver = GameObject.Find("GardenCam");
    }
    bool IsAllComplete() {
        foreach(LevelInfo level in GameConfig.instance.levels) {
            if(!level.isComplete) {
                return false;
            }
        }
        return true;
    }
    public IEnumerator GardenComplete() {
        // let the sound effect for planting finish
        yield return new WaitForSeconds(.5f);
        gardenDriver.SendMessage("GardenComplete");
        IsGardenComplete = true;
        AudioSource.PlayClipAtPoint(gardenComplete, Camera.main.transform.position);
        yield return new WaitForSeconds(gardenCompleteDuration);
        currentLevel.isComplete = true;
        currentLevel = null;
        if(IsAllComplete()) {
            Application.LoadLevel("Finish");
        } else {
            Application.LoadLevel("StageSelect");
        }
    }
    void CheckLevelComplete() {
        if(IsLevelComplete()) {
            StartCoroutine(GardenComplete());
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
