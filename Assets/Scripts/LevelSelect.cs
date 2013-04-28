using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

    public void OnGUI() {
        int offset = 0;
        int buttonHeight = 30;
        foreach(LevelInfo level in GameConfig.instance.levels) {
            GUI.enabled = !level.isComplete;
            if(GUI.Button(new Rect(0, offset, 100, buttonHeight), "Level " + offset)) {
                GameDriver.instance.LoadLevel(level);
            }
            offset += buttonHeight;
        }
    }
}
