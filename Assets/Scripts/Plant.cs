using UnityEngine;
using System.Collections;

public enum PlantType { None, Tiny, Medium, Rose };

public abstract class Plant : MonoBehaviour {

    protected float actionCooldown;

    private float actionStart=-1;
    protected ProgressCircle pc;

    public PlantType type;

    public void Start() {
        pc = transform.FindChild("ProgressCircle").GetComponent<ProgressCircle>();
        pc.Percent = 0;
        actionStart = Time.time;
    }
    public void Update() {
        Grow();
    }

    public void Grow() {
        if(actionStart == -1) {
            return;
        }
        if(actionStart + actionCooldown <= Time.time) {
            actionStart = -1;
            DoAction();
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
        // roses don't boost themselves, or other roses
        if(type != PlantType.Rose) {
            if(actionStart != -1) {
                actionStart -= amount;
            }
        }
    }
    public abstract void DoAction();
}
