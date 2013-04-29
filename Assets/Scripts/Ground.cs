using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

    public Plant plant;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }
    public Transform getPlantPoint() {
        return transform.FindChild("PlantPoint").transform;
    }
    public bool IsPlantable(PlantType type) {
        return plant == null;
    }
}
