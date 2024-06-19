using UnityEngine;
using System.Collections;

public enum PlantType { None, Tiny, Medium, Flower };

public abstract class Plant : MonoBehaviour {

    public AudioClip actionSound;
    public GameObject actionEffect;

    protected float actionCooldown;

    private float actionStart=-1;
    protected ProgressCircle pc;

    public PlantType type;

    public void Start() {
        pc = transform.Find("ProgressCircle").GetComponent<ProgressCircle>();
        pc.Percent = 0;
        actionStart = Time.time;
    }
    public void Update() {
        Grow();
    }

    public void Grow() {
        if(actionStart == -1 || GameDriver.instance.IsGardenComplete) {
            return;
        }
        if(actionStart + actionCooldown <= Time.time) {
            actionStart = -1;
            if (actionSound) {
                AudioSource.PlayClipAtPoint(actionSound, Camera.main.transform.position);
            }
            DoAction();
            if (actionEffect) {
                GameObject obj = Instantiate(actionEffect) as GameObject;
                Vector3 pos = transform.position;
                pos.y += .02f;
                obj.transform.position = pos;
            }
            pc.Percent = 0;
        } else {
            pc.Percent = (Time.time - actionStart) / actionCooldown;
        }
    }
    protected void StartGrow() {
        actionStart = Time.time;
        if(!pc.gameObject.activeSelf) {
            pc.gameObject.SetActive(true);
        }
    }
    public void Attach(Ground ground) {
        transform.parent = ground.getPlantPoint();
        transform.position = transform.parent.position;
        ground.plant = this;
    }
    public virtual void Boost(float amount) {
        // flowers don't boost themselves, or other flowers
        if(type != PlantType.Flower) {
            if(actionStart != -1) {
                actionStart -= amount;
            }
        }
    }
    public abstract void DoAction();
}
