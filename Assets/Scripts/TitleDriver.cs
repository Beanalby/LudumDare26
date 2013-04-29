using UnityEngine;
using System.Collections;

public class TitleDriver : MonoBehaviour {

    public AudioClip titleSound;

    private float zoomSpeed = 1f;
    private float fadeDuration = 1f;
    private Material whiteout;
    float start;
    float fadeDelay = 1.5f;

    // Use this for initialization
    void Start () {
        whiteout = transform.FindChild("Whiteout").renderer.material;
        whiteout.color = new Color(1, 1, 1, 0);
        start = Time.time;
        AudioSource.PlayClipAtPoint(titleSound, Camera.main.transform.position);
    }
    
    // Update is called once per frame
    void Update () {
        Vector3 pos = transform.localPosition;
        pos.z += zoomSpeed * Time.deltaTime;
        transform.localPosition = pos;

        if (Time.time > start + fadeDelay) {
            float percent = (Time.time - (start + fadeDelay)) / fadeDuration;
            if (percent >= 1) {
                Application.LoadLevel("StageSelect");
            } else {
                whiteout.color = new Color(1, 1, 1, percent);
            }
        }
    }
}
