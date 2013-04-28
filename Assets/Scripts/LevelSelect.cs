using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

    private float fadeDuration = 1f;
    private float levelSelectedStart = -1f;
    private LevelInfo selectedLevel;
    private float start=-1;
    private Material whiteout;

    public void Start() {
        whiteout = transform.FindChild("Whiteout").renderer.material;
        start = Time.time;
        levelSelectedStart = -1;
        whiteout.color = new Color(1, 1, 1, 1);
    }
    public void Update() {
        if (start != -1) {
            float percent = (Time.time - start) / fadeDuration;
            if (percent >= 1) {
                start = -1;
                whiteout.color = new Color(1, 1, 1, 0);
            } else {
                whiteout.color = new Color(1, 1, 1, 1 - percent);
            }
        } 
        if (levelSelectedStart != -1) {
            float percent = (Time.time - levelSelectedStart) / fadeDuration;
            if (percent >= 1) {
                GameDriver.instance.LoadLevel(selectedLevel);
            } else {
                whiteout.color = new Color(1, 1, 1, percent);
            }
        }
    }
    public void OnGUI() {
        if (levelSelectedStart != -1) {
            return;
        }
        int offset = 0;
        int buttonHeight = 30;
        foreach(LevelInfo level in GameConfig.instance.levels) {
            GUI.enabled = !level.isComplete;
            if(GUI.Button(new Rect(0, offset, 100, buttonHeight), "Level " + offset)) {
                selectedLevel = level;
                levelSelectedStart = Time.time;
            }
            offset += buttonHeight;
        }
    }
}
