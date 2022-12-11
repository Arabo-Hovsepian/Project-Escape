using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour
{
    public AudioClip ButtonClickOn;
    public AudioClip ButtonClickOff;
    public AudioClip Coinv1;
    public AudioClip Coinv2;
    public AudioClip DeathSound;
    public AudioClip HeadBump;

    AudioSource ads;
    // Start is called before the first frame update
    void Start()
    {
        ads = GetComponent<AudioSource>();
    }
    public void ClickButton() {
        ads.PlayOneShot(ButtonClickOn, 1);
    }
    public void CollectCoin() {
        ads.PlayOneShot(Coinv1, 1);
    }
    public void PlayerDie() {
        ads.PlayOneShot(DeathSound, 1);
    }
    public void PlayerBump() {
        ads.PlayOneShot(HeadBump, 1);
    }
}
