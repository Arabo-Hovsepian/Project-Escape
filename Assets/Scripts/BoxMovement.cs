using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    PlayerMovement pm;
    Vector2 target;

    Collider cl;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        pm = PlayerManager.instance.GetComponent<PlayerMovement>();
        target = new Vector2(transform.position.x, transform.position.z);
        cl = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        if ((pm.transform.position - transform.position).magnitude < 4) {
            pm.box = this;
        }
        else if (pm.box == this) {
            pm.box = null;
        }
        Vector2 current = new Vector2(pos.x, pos.z);
        if ((target - current).magnitude>0.05f) {
            Vector2 direction = (target - current).normalized;
            float size_x = cl.bounds.size.x;
            float size_y = cl.bounds.size.y;
            float size_z = cl.bounds.size.z;
            float length = size_x / 2 + 0.1f;
            Vector3 offset_w = new Vector3(size_x/2-0.1f,0,0);
            if (Mathf.Abs(direction.y) < 0.5) {
                offset_w = new Vector3(0,0,size_z/2-0.1f);
                length = size_z / 2 + 0.1f;
            }
            Vector3 offset_h = new Vector3(0, size_y/2-0.1f,0);
            Vector3 origin0 = cl.bounds.center;
            Vector3 origin1 = cl.bounds.center + offset_w + offset_h;
            Vector3 origin2 = cl.bounds.center - offset_w + offset_h;
            Vector3 origin3 = cl.bounds.center + offset_w - offset_h;
            Vector3 origin4 = cl.bounds.center - offset_w - offset_h;
            Ray ray0 = new Ray(origin0, new Vector3(direction.x, 0, direction.y));
            Ray ray1 = new Ray(origin1, new Vector3(direction.x, 0, direction.y));
            Ray ray2 = new Ray(origin2, new Vector3(direction.x, 0, direction.y));
            Ray ray3 = new Ray(origin3, new Vector3(direction.x, 0, direction.y));
            Ray ray4 = new Ray(origin4, new Vector3(direction.x, 0, direction.y));
            var hit = Physics.Raycast(ray0, length) ||
                      Physics.Raycast(ray1, length) ||
                      Physics.Raycast(ray2, length) ||
                      Physics.Raycast(ray3, length) ||
                      Physics.Raycast(ray4, length);
            if (hit) {
                target = current;
                return;
            }
            transform.position = pos + speed*new Vector3(direction.x, 0, direction.y)*Time.deltaTime;
        } else {
            transform.position = new Vector3(target.x, pos.y, target.y);
        }
    }

    public void Push(Vector3 direction) {
        Vector3 temp = direction*transform.localScale.x;
        target = target + new Vector2(temp.x, temp.z);
    }
}
