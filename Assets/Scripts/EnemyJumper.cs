using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumper : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject rotateIndicator;
    [SerializeField] bool charging = true;
    [SerializeField] bool going = false;
    [SerializeField] float rotation;
    [SerializeField] float speed;
    [SerializeField] GameObject tip;
    [SerializeField] GameObject player;
    [SerializeField] int randNum;
    [SerializeField] bool searching = false;
    [SerializeField] bool disabled;
    [SerializeField] float waitTime;
    [SerializeField] float timeElapsedSinceGo;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        randNum = Random.Range(0,10);
    }

    // Update is called once per frame
    void Update()
    {
       
        if (disabled == false) {


            timeElapsedSinceGo += Time.deltaTime;
            if (timeElapsedSinceGo > waitTime + 1.5f)
            {
                going = true;
                timeElapsedSinceGo = 0;
            }
            if (going == true)
            {
                timeElapsedSinceGo = 0;
            }
            rb.gravityScale = 0;

            LayerMask mask = new LayerMask();
            int grnd = 1 << LayerMask.NameToLayer("Enemy");
            int fly = 1 << LayerMask.NameToLayer("CameraArea");
            mask = grnd | fly;
            RaycastHit2D hit = Physics2D.Raycast(tip.transform.position, tip.transform.up, 30f, ~mask);
            rotateIndicator.transform.eulerAngles = new Vector3(0, 0, rotation);
         

                if (going == true)
                {
                    rb.velocity = rotateIndicator.transform.up * speed;
                }

                if (going == false)
                {
                    if (randNum > 5)
                    {
                        if (Vector2.Distance(hit.point, tip.transform.position) < 6)
                        {
                            rotation = Random.Range(0, 360);
                        }
                    }
                    else
                    {
                        float angle = Mathf.Atan2(player.transform.position.y - this.transform.position.y, player.transform.position.x - this.transform.position.x) * Mathf.Rad2Deg;
                        rotation = angle - 90;
                    }
                   

                    // mask = LayerMask.GetMask("Enemy"), LayerMask.GetMask("Enemy");
                    
                    if (hit.collider != null)
                    {
                     
                    }
                }
        }
        else
        {
            rb.gravityScale = 1;
        }
    }
    IEnumerator Charge()
    {
       
        randNum = Random.Range(0, 10);
       
        yield return new WaitForSeconds(Random.Range(waitTime -1, waitTime +1));
        going = true;
        charging = false;

    }
    IEnumerator GetUp()
    {
        yield return new WaitForSeconds(1);
        disabled = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
       
        if (collision.gameObject.layer == 8 && disabled == false)
        {

            if (collision.gameObject.layer == 8)
            {
              
            }
            else
            {
                if (charging == false)
                {
                    charging = true;
                    
                    StartCoroutine(Charge());
                }
            }

        }
        else
        {
            if (collision != null)
            {
              //  Debug.Log(collision.transform.gameObject);
               
                searching = true;
                
            }
          
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        searching = false;
        going = true;
        charging = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.layer == 8)
        {
            going = false;
            StartCoroutine(GetUp());
            disabled = true;
        }
        else
        {
            going = false;
            StartCoroutine(Charge());
        }
        
    }
}
