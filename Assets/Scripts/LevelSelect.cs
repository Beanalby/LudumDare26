using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelect : MonoBehaviour {

    public GUISkin skin;
    public AudioClip levelSelected;

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
        GUI.skin = skin;
        if (levelSelectedStart != -1) {
            return;
        }
        int offset = 0;
        int buttonHeight = 50;
        int count = 1;
        foreach(LevelInfo level in GameConfig.instance.levels) {
            GUI.enabled = !level.isComplete;
            string desc = "stage " + count + ": plant ";
            bool firstItem = true;
            foreach(KeyValuePair<PlantType, int> pair in level.target) {
                if (firstItem) {
                    firstItem = false;
                } else {
                    desc += ", ";
                }
                desc += pair.Value + " " + pair.Key;
            }

            if(GUI.Button(new Rect(0, offset, 350, buttonHeight), desc)) {
                selectedLevel = level;
                levelSelectedStart = Time.time;
                AudioSource.PlayClipAtPoint(levelSelected, Camera.main.transform.position);
            }
            offset += buttonHeight;
            count++;
        }
    }
}
