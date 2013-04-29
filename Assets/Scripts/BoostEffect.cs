using UnityEngine;
using System.Collections;

public class BoostEffect : MonoBehaviour {

    private float scaleSpeed = 1.5f;
    private float scaleDuration = .5f;
    private float start;
    private Material mat;
    private Color color;
    // Use this for initialization
    void Start() {
        start = Time.time;
        mat = GetComponentInChildren<Renderer>().material;
        color = mat.color;
    }
    
    // Update is called once per frame
    void Update () {
        float percent = (Time.time - start) / scaleDuration;
        if (percent >= 1) {
            Destroy(gameObject);
        } else {
            float newScale = 1+percent*scaleSpeed;
            transform.localScale = new Vector3(newScale, newScale, newScale);
            color.a = 1-percent;
            mat.color = color;
        }
    }
}
