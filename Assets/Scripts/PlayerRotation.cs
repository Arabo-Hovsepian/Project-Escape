using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{

    public float speed = 200f;
    public float target = 0f;
    public float margin = 1f;
    
    public float angle = 0;

    PlayerMovement pm;

    float previousDifference = 0;

    // Start is called before the first frame update
    void Start()
    {
        pm = transform.parent.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pm.alive)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            return;
        }

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(hInput) > 0.01) hInput = Mathf.Sign(hInput);
        else hInput = 0;
        if (Mathf.Abs(vInput) > 0.01) vInput = Mathf.Sign(vInput);
        else vInput = 0;
        Vector2 direction = new Vector2(hInput, vInput);
        if (direction.magnitude > 0.01 && !pm.isPushing())
        {
            angle = (360 - Mathf.Atan(direction.x / direction.y) * 180 / Mathf.PI) % 360;
            if (direction.y < 0) angle += 180;
            target = angle = (360 - angle) % 360;
        }
        Vector3 current = transform.eulerAngles;
        float difference = (720 + target - current.y) % 360;
        float multiplier = 1f;
        if (difference > 120 && difference < 240) { multiplier = 2f; }
        if (difference < margin || (360 - difference) < margin)
        {
            transform.eulerAngles = new Vector3(current.x, target, current.z);
            previousDifference = 0;
        }
        else if (difference < 180)
        {
            if (previousDifference < 0 && difference < 10)
            {
                transform.eulerAngles = new Vector3(current.x, target, current.z);
                previousDifference = 0;
                return;
            }
            previousDifference = 1;
            transform.eulerAngles = new Vector3(current.x, (360 + current.y + speed * multiplier * Time.deltaTime) % 360, current.z);
        }
        else
        {
            if (previousDifference > 0 && difference < 10)
            {
                transform.eulerAngles = new Vector3(current.x, target, current.z);
                previousDifference = 0;
                return;
            }
            previousDifference = -1;
            transform.eulerAngles = new Vector3(current.x, (360 + current.y - speed * multiplier * Time.deltaTime) % 360, current.z);
        }
    }
}
