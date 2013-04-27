using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelInfo {
    public Dictionary<PlantType, int> target;
}
public class GameDriver : MonoBehaviour {

    public static GameDriver instance;

    LevelInfo currentLevel;
    Dictionary<PlantType, int> plants;

    void Start() {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        currentLevel = new LevelInfo();
        currentLevel.target = new Dictionary<PlantType, int>()
            { { PlantType.Tiny, 2 }, { PlantType.Medium, 2 } };
        //currentLevel.target[PlantType.Tiny] = 2;
        //currentLevel.target[PlantType.Medium] = 2;

        plants = new Dictionary<PlantType, int>();
    }

    void CheckComplete() {
        if(IsComplete()) {
            Debug.Log("Loading next level!");
        }
    }
    bool IsComplete() {
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

    public void PlantSpawned(PlantType type) {
        if(!plants.ContainsKey(type)) {
            plants[type] = 0;
        }
        plants[type]++;
        CheckComplete();
    }

}
