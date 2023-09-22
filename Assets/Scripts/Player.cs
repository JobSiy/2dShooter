using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] bool grounded;
    [SerializeField] GameObject gunMount;
    [SerializeField] bool jumping = false;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject gunpoint;
    [SerializeField] bool reload = false;
    [SerializeField] float bulletSpeedMod;
    [SerializeField] float PlayerSpeed;
    [SerializeField] float jumpMult = 8;
    [SerializeField] GameObject playerCam;
    [SerializeField] GameObject distanceJoint;
    [SerializeField] LayerMask connectMask;
    [SerializeField] float chargeDist;
    [SerializeField] bool canCharge;
    [SerializeField] GameObject teleportParticles1;
    [SerializeField] GameObject teleportParticles2;
    [SerializeField] LayerMask hitIgnore;
    [SerializeField] float cDist;
    [SerializeField] bool charging;
    // Start is cal boolled before the first frame update
    void Start()
    {
        grounded = true;
        teleportParticles1.GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Charge();

        if (charging == false)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(gunMount.transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            gunMount.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    public void Charge()
    {
        SpringJoint2D x;
        LayerMask mask = 1;
        //Physics2D.IgnoreRaycastLayer = 9;
        //~LayerMask.GetMask("Player"),~LayerMask.GetMask("CameraArea"),
        RaycastHit2D hit = Physics2D.Raycast(gunpoint.transform.position, gunpoint.transform.right, 100f,   ~hitIgnore);
        cDist = chargeDist;
        if (hit == true)
        {
           // Debug.Log(hit.transform.gameObject);
            var dist = Vector2.Distance(hit.point, this.transform.position);
            
        }
        else
        {
            if (cDist > 2)
            {
                cDist -= 10 * Time.deltaTime;
            }
        }

            if (canCharge == true)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    teleportParticles1.SetActive(true);
                    teleportParticles1.GetComponent<ParticleSystem>().Play();
                }
                if (Input.GetKey(KeyCode.LeftShift))
                {
                if (canCharge == true)
                {
                    charging = true;
                }
                    distanceJoint.gameObject.SetActive(true);
                    rb.drag = 10;
                    distanceJoint.transform.position = new Vector2(gunpoint.transform.position.x + (cDist * gunpoint.transform.right.x ), gunpoint.transform.position.y + (cDist * gunpoint.transform.right.y ));
                    if (chargeDist < Vector2.Distance(hit.point, gunpoint.transform.position) - 1)
                    {
                        if (chargeDist < 6)
                        {
                            chargeDist += Time.deltaTime * 10;
                        }
                    }
                    else
                    {
                    //chargeDist -= Time.deltaTime * 10;
                    if (chargeDist > Vector2.Distance(hit.point, gunpoint.transform.position)){
                        chargeDist = Vector2.Distance(hit.point, gunpoint.transform.position);
                    }
                    //chargeDist = 0;
                }

                }
                else
                {
                    distanceJoint.gameObject.SetActive(false);
                    rb.drag = 0;
                 //   distanceJoint.transform.position = gunpoint.transform.position;
                    chargeDist = 0;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift) && canCharge == true)
                {
                    charging = false; 
                    teleportParticles1.SetActive(false);
                    canCharge = false;
                    StartCoroutine(reCharge());
                    this.transform.position = distanceJoint.transform.position;
                    Instantiate(teleportParticles2, this.transform.position, Quaternion.identity);
                }
            }
        
       
    }
    IEnumerator reCharge()
    {

        yield return new WaitForSeconds(1f);
        canCharge = true;
    }
    /*  if (hit == true) {
                 distanceJoint.SetActive(true);
                 Debug.Log(hit.transform.gameObject);
                 distanceJoint.transform.position = hit.point;
             }
             else
             {
                 distanceJoint.SetActive(false);
             }
         }
         if (!Input.GetKey(KeyCode.LeftShift))
         {
             distanceJoint.SetActive(false);
         }
        */
    private void Move()
    {
         float wantedSpeed = 0;
        if (Input.GetKey(KeyCode.D))
        {
            wantedSpeed += PlayerSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            wantedSpeed -= PlayerSpeed;
        }
        if (Input.GetKey(KeyCode.Space) && grounded == true && jumping == false)
        {
            jumping = true;
            StartCoroutine(jumpSpace());
            rb.AddForce(transform.up * jumpMult, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.Mouse0) && reload == false)
        {
            reload = true;
           StartCoroutine( Shoot());
        }

        if (grounded == true)
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, wantedSpeed, 0.5f), rb.velocity.y);
        }
        else 
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, wantedSpeed, 0.01f), rb.velocity.y);
        }

    }
    IEnumerator Shoot()
    {
        var bul = Instantiate(bullet, gunpoint.transform.position, gunpoint.transform.rotation);
        bul.GetComponent<Rigidbody2D>().velocity = gunpoint.transform.right * bulletSpeedMod;
        yield return new WaitForSeconds(0.5f);
        reload = false;

    }
    IEnumerator jumpSpace()
    {
        
        yield return new WaitForSeconds(0.9f);
        jumping = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 9)
        {
            grounded = true;
        }
       /* if (collision.gameObject.layer == 9)
        {
            playerCam.transform.position = new Vector3( collision.transform.position.x, collision.transform.position.y, playerCam.transform.position.z);
        }*/
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        grounded = false;
    }
}
