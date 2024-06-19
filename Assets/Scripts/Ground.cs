using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

    public Plant plant;

    private Transform plantable;
    private float plantableStart = -1;
    public bool Plantable {
        get { return plantableStart != -1; }
        set {
            if (value) {
                plantable.gameObject.SetActive(true);
                plantableStart = Time.time;
            } else {
                plantable.gameObject.SetActive(false);
                plantableStart = -1;
            }
        }
    }
    // Use this for initialization
    void Start () {
        plantable = transform.Find("Plantable");
        plantable.gameObject.SetActive(false);
    }
    
    // Update is called once per frame
    void Update () {
        if (plantableStart != -1) {
            float newScale = 1 - Mathf.Abs(Mathf.Cos(4* Time.time - plantableStart) / 4);
            plantable.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
    public Transform getPlantPoint() {
        return transform.Find("PlantPoint").transform;
    }
    public bool IsPlantable(PlantType type) {
        return plant == null;
    }
    public void SetPlantableEffect(PlantType type) {
        Plantable = IsPlantable(type);
    }
    public void ClearPlantableEffect() {
        Plantable = false;
    }
}
