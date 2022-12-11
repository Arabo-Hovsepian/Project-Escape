using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;

    GameObject player;
    Animator anim;
    PlayerMovement pm;

    
    public GameObject character;
    public Light backup;
    public Renderer point;
    public Renderer leg1;
    public Renderer leg2;
    public Renderer leg3;
    public Renderer leg4;
    public Light halo;

    public GameObject explosion;
    float explosionTime;
    float deathTime = 1f;
    public float deathTimer;
    float intensity;

    public bool isDying = false;
    public bool isExploding = false;

    void Awake() {
        if (instance == null) {
            instance = this;
            player = this.gameObject.transform.GetChild(0).gameObject;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = player.GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        intensity = halo.intensity;
        explosionTime = explosion.GetComponent<ParticleSystem>().main.duration;
        deathTimer = explosionTime+deathTime+1;
    }

    // Update is called once per frame
    void Update()
    {
        if (point != null && deathTimer < deathTime+explosionTime) {  
            deathTimer += Time.deltaTime;
            if (deathTimer < deathTime) {
                float r, b, g, a = r = b = g = 1f - deathTimer / deathTime;
                Color customColor = new Color(r, b, g, a);
                point.material.SetColor("_EmissionColor", customColor);
                leg1.material.SetColor("_EmissionColor", customColor);
                leg2.material.SetColor("_EmissionColor", customColor);
                leg3.material.SetColor("_EmissionColor", customColor);
                leg4.material.SetColor("_EmissionColor", customColor);
                halo.intensity = intensity - deathTimer / deathTime * intensity / 2;
                transform.position += new Vector3(0, 2*Time.deltaTime, 0);
            }
            if (deathTimer >= deathTime && !isExploding) {
                var vfx = Instantiate(explosion, transform.position, Quaternion.identity);
                character.SetActive(false);
                backup.enabled = true;
                Destroy(vfx, vfx.GetComponent<ParticleSystem>().main.duration);
                isExploding = true;
            }
            else if (deathTimer >= deathTime+explosionTime) {
                isExploding = false;
                isDying = false;
                Reborn();
                GameManager.instance.LoadLevel();
            }
        }
    }

    public void SetPosition(Vector3 p) {
        transform.position = p;
    }

    public bool PlayerCanPushBox(){
        return pm.canPushBox;
    }
    
    public bool PlayerHasPushBox(){
        return pm.hasPushedBox;
    }
    public bool PlayerCanPushButton(){
        return pm.canPushButton;
    }
    public bool PlayerHasPushButton(){
        return pm.hasPushedButton;
    }

    public bool PlayerHasDied(){
        return !pm.alive;
    }

    public void Die() {
        if (pm.alive) {
            GameManager.instance.gs.PlayerDie();
            anim.SetBool("isDead", true);
            deathTimer = 0f;
            pm.alive = false;
            pm.StopMoving();
            isDying = true;
        }
    }

    public void Reborn() {
        anim.SetBool("isDead", false);
        pm.alive = true;
        character.SetActive(true);
        backup.enabled = false;
        SetPosition(new Vector3(0,1,0));
        pm.rb.isKinematic = false;
        float r, b, g, a = r = b = g = 1f;
        Color customColor = new Color(r, b, g, a);
        point.material.SetColor("_EmissionColor", customColor);
        leg1.material.SetColor("_EmissionColor", customColor);
        leg2.material.SetColor("_EmissionColor", customColor);
        leg3.material.SetColor("_EmissionColor", customColor);
        leg4.material.SetColor("_EmissionColor", customColor);
        halo.intensity = intensity;
        deathTimer = deathTime+explosionTime + 1;
    }

    public Vector3 getPos() {
        return pm.cl.center + transform.position;
    }
}
