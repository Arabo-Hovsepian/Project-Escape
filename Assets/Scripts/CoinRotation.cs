using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    public float speed = 100f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        //distance (in angles) to rotate on each frame. distance = speed * time
        float angle = speed * Time.deltaTime;
        //rotate on Y
        transform.Rotate(Vector3.up * angle, Space.World);
    }

}
