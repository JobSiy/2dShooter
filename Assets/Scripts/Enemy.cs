using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    GameObject player;
    [SerializeField] float recoilSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] int health;
    private bool iFrame;
    [SerializeField] GameObject explostionParticles;
    [SerializeField] bool nearEnemy;
    [SerializeField] GameObject wallTrigger;
    [SerializeField] GameObject[] detachables;
    public bool damage = false;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(damage == true)
        {
            damage = false;
            StartCoroutine(TakeDamage());
        }
        if(health <= 0)
        {
            for (i = 0; i < detachables.Length; i++)
            {

                detachables[i].transform.parent = null;
                if (detachables[i].GetComponent<Rigidbody2D>())
                {
                    detachables[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                }
                else
                {
                    detachables[i].AddComponent<Rigidbody2D>();
                }
               
            }
            Destroy(this.gameObject);
            Instantiate(explostionParticles, transform.position, Quaternion.identity);
        }
        if (wallTrigger.gameObject.GetComponent<CameraArea>().playerHere == true)
        {

            nearEnemy = true;
        }
        else
        {
            //  Debug.Log("AAA");
            nearEnemy = false;
        }
    }
    private void FixedUpdate()
    {
        if (nearEnemy == true)
        {
            if (Mathf.Abs(rb.velocity.x) < 10)
            {
                if (player.transform.position.x - this.transform.position.x > 3)
                {
                    rb.AddForce(new Vector2(moveSpeed, 0));
                }
                else if (player.transform.position.x - this.transform.position.x < -3)
                {
                    rb.AddForce(new Vector2(-moveSpeed, 0));
                }
            }
            else
            {
                rb.AddForce(new Vector2(-rb.velocity.x, 0));
            }
        }
       // rb.velocity =new Vector2(Mathf.Clamp(rb.velocity.x, -4, 4), rb.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        Debug.Log(collision.gameObject);
        if(collision.gameObject.layer == 9)
        {
            wallTrigger = collision.gameObject;
           
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].collider.gameObject.GetComponent<Rigidbody2D>())
        {
            rb.velocity = new Vector2(rb.velocity.x / collision.contacts[0].collider.gameObject.GetComponent<Rigidbody2D>().velocity.x, rb.velocity.y);
        }
        if(collision.gameObject.layer == 8)
        {
            if(iFrame == false)
            {
                iFrame = true;
                StartCoroutine(TakeDamage());
            }
            
            rb.AddForce(new Vector2((-collision.contacts[0].point.x + this.transform.position.x) * recoilSpeed, 0), ForceMode2D.Impulse);
            Debug.Log((-collision.contacts[0].point.x + this.transform.position.x));
        }
    }
    
    IEnumerator TakeDamage()
    {
        health--;
        yield return new WaitForSeconds(0.1f);
        iFrame = false;
    }
}
