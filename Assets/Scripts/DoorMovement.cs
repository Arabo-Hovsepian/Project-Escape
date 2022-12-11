using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : TriggerMovement
{
    public float target;
    public float speed = 50f;
    public float margin = 1f;
    bool start = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (start) {
            Vector3 current = transform.eulerAngles;
            float difference = (720+target - current.y)%360;
            if (Mathf.Abs(difference) < margin) {
                transform.eulerAngles = new Vector3(current.x, target, current.z);
            }
            else if (difference<180) {
                transform.eulerAngles = new Vector3(current.x, (360+current.y+speed*Time.deltaTime)%360, current.z);
            }
            else {
                transform.eulerAngles = new Vector3(current.x, (360+current.y-speed*Time.deltaTime)%360, current.z);
            }
        }
    }

    public override void Trigger() {
        start = true;
    }
}
