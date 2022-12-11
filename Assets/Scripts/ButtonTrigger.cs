using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{

    public TriggerMovement obj;
    // Start is called before the first frame update
    public GameObject buttonCenter;


    bool pushed = false;
    PlayerMovement pm;
    void Start()
    {
        pm = PlayerManager.instance.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((pm.transform.position - transform.position)).magnitude < 3) {
            pm.button = this;
        }
        else if (pm.button == this) {
            pm.button = null;
        }
    }

    public void Push() {
        if (pushed) return;
        float angle = -transform.eulerAngles.y-90;
        Vector3 offset = new Vector3(Mathf.Cos(angle/180*Mathf.PI),0,Mathf.Sin(angle/180*Mathf.PI));
        buttonCenter.transform.position += offset*0.2f;
        pushed = true;
        if (obj.tag == "Door") {
            obj.Trigger();
        }
        else {
            print(obj.tag + "'s trigger is not found");
        }
    }
}
