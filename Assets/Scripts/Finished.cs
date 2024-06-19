using UnityEngine;
using System.Collections;

public class Finished : MonoBehaviour {

    private float start=-1;
    public float fadeDuration = 1f;
    private Material whiteout;
    public void Start() {
        whiteout = transform.Find("Whiteout").GetComponent<Renderer>().material;
        start = Time.time;
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
    }
}
