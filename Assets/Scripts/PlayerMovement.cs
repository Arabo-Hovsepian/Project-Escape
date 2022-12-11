using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public CapsuleCollider cl;
    Animator anim;

    public GameObject click;

    public bool alive = true;

    public float walkSpeed = 4.0f;
    public float jumpSpeed = 8.0f;
    // Start is called before the first frame update

    public BoxMovement box = null;
    public bool canPushBox = false;

    public ButtonTrigger button = null;
    public bool canPushButton = false;

    public bool hasPushedButton = false;
    public bool hasPushedBox = false;


    float pushTimer = 1f;

    bool walking = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<CapsuleCollider>();
        anim = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive){
            WalkHandler();
            JumpHandler();
            PushHandler();
            HeightCheck();
        }
        else {
            var exitInput = Input.GetKeyDown(KeyCode.Escape);
            if (exitInput) {
                GameManager.instance.LoadLevel("StartScene");
                PlayerManager.instance.Reborn();
            }
        }
    }

    void WalkHandler() {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(hInput, vInput);
        anim.SetBool("isWalking",  direction.magnitude> 0.01);
        direction.Normalize();
        if (direction.magnitude > 0.01 && !isPushing()) {
            rb.velocity = new Vector3(direction.x*walkSpeed, rb.velocity.y, direction.y*walkSpeed);
            anim.SetTrigger("walk");
            if (isGrounded()&&!walking) {
                walking = true;
                PlayerManager.instance.GetComponent<AudioSource>().Play(0);
            }
        }
        else {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            anim.ResetTrigger("walk");
            if (walking) {
                walking = false;
                PlayerManager.instance.GetComponent<AudioSource>().Pause();
            }
        }
        if (!isGrounded()) {
            walking = false;
            PlayerManager.instance.GetComponent<AudioSource>().Pause();
        }
    }
    

    void JumpHandler() {
        var jumpInput = Input.GetButtonDown("Jump");
        var jumpInputReleased = Input.GetButtonUp("Jump");
        bool grounded = isGrounded();
        anim.SetBool("isGrounded", grounded);
        anim.SetFloat("upVelocity", rb.velocity.y);
        if (jumpInput && isGrounded() && !isPushing()){
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            anim.SetTrigger("jump");
        }
        else {
            anim.ResetTrigger("jump");
        }
        if (jumpInputReleased && rb.velocity.y > 0) {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y/2, rb.velocity.z);
        }
    }

    bool isGrounded() {
        float sizeX = cl.bounds.size.x;
        float sizeY = cl.bounds.size.y;
        float sizeZ = cl.bounds.size.z;
        Vector3 pos = cl.bounds.center;
        Vector3 bottom1 = pos + new Vector3(0, -sizeY/2 + 0.1f, 0);
        Vector3 bottom2 = pos + new Vector3(sizeX/2, -sizeY/2 + 0.1f, sizeZ/2);
        Vector3 bottom3 = pos + new Vector3(-sizeX/2, -sizeY/2 + 0.1f,sizeZ/2);
        Vector3 bottom4 = pos + new Vector3(sizeX/2, -sizeY/2 + 0.1f, -sizeZ/2);
        Vector3 bottom5 = pos + new Vector3(-sizeX/2, -sizeY/2 + 0.1f, -sizeZ/2);
        bool grounded1 = Physics.Raycast(bottom1, new Vector3(0, -1, 0), 0.15f);
        if (grounded1) return true;
        bool grounded2 = Physics.Raycast(bottom1, new Vector3(0, -1, 0), 0.15f);
        if (grounded2) return true;
        bool grounded3 = Physics.Raycast(bottom1, new Vector3(0, -1, 0), 0.15f);
        if (grounded3) return true;
        bool grounded4 = Physics.Raycast(bottom1, new Vector3(0, -1, 0), 0.15f);
        if (grounded4) return true;
        bool grounded5 = Physics.Raycast(bottom1, new Vector3(0, -1, 0), 0.15f);
        if (grounded5) return true;
        return false;
    }

    void PushHandler() {
        pushTimer += Time.deltaTime;
        if (isPushing()) return;
        var pushInput = Input.GetKeyDown(KeyCode.E);
        Vector3 start = cl.bounds.center;
        float angle = -(anim.gameObject.GetComponent<PlayerRotation>().angle-90)/180*Mathf.PI;
        Vector3 direction = new Vector3(Mathf.Cos(angle),0,Mathf.Sin(angle)).normalized;
        if (Mathf.Abs(direction.x) + Mathf.Abs(direction.z) > 1.1f) return;
        Ray ray = new Ray(start, direction);
        RaycastHit hit;
        canPushBox = box != null && Physics.Raycast(ray, out hit) && hit.transform.GetComponent<BoxMovement>()==box;
        if (pushInput && canPushBox){
            anim.SetTrigger("kick");
            box.Push(direction);
            PushEffect();
            hasPushedBox = true;
            pushTimer = 0f;
            GameManager.instance.gs.PlayerBump();
        }
        else {
            anim.ResetTrigger("kick");
        }

        RaycastHit hit2;
        canPushButton = button != null && Physics.Raycast(ray, out hit2) && hit2.transform.GetComponent<ButtonTrigger>() == button;
        if (pushInput && canPushButton)
        {
            anim.SetTrigger("bump");
            button.Push();
            PushEffect();
            hasPushedButton = true;
            pushTimer = 0f;
            GameManager.instance.gs.ClickButton();
        }
        else {
            anim.ResetTrigger("bump");
        }
        bool playerIsPushing = isPushing();
        anim.SetBool("isBumping", playerIsPushing);
    }

    public bool isPushing() {
        return pushTimer < 1f;
    }

    public void StopMoving() {
        rb.velocity = new Vector3(0,0,0);
        rb.isKinematic = true;
        walking = false;
        PlayerManager.instance.GetComponent<AudioSource>().Pause();
    }

    public void PushEffect() {
        var vfx = Instantiate(click, anim.transform.position +1.3f* anim.transform.forward+new Vector3(0,1,0), anim.transform.rotation);
        Destroy(vfx, vfx.GetComponent<ParticleSystem>().main.duration);
    }

    void HeightCheck() {
        if (transform.position.y < -40) {
            PlayerManager.instance.Die();
        }
    }
}
