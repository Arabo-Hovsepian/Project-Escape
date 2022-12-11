using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    float timer = 0;
    float spinTime = 1.5f;
    bool collecting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (collecting) {
            timer += Time.deltaTime;
            Vector3 velocity;
            if (timer < spinTime) {
                velocity = new Vector3(0,2,0);
            }
            else {
                Vector3 direction = PlayerManager.instance.getPos() - transform.position;
                velocity = timer * direction.normalized;
                transform.localScale = new Vector3(0.3f,0.3f,0.3f)/(timer) + new Vector3(0.7f,0.7f, 0.7f);
            }
            transform.position += velocity * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider cl) {
        if (cl.gameObject.tag == "Player") {
            if (!collecting) collecting = true;
            else {
                collecting = false;
                GameManager.instance.CollectCoin();
                Destroy(this.gameObject);
            }
        }
    }
}
