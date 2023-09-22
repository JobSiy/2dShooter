using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMinion : MonoBehaviour
{
    [SerializeField] GameObject pivot;
    [SerializeField] float turnSpeed;
    [SerializeField] bool shoot;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject gunPoint;
    [SerializeField] float reloadTime;
    [SerializeField] Animator anim;
    [SerializeField] bool right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        // Vector3 playerPos = GameObject.Find("Player").transform.position;
        playerPos.z = 5.23f;

        Vector3 objectPos = pivot.transform.position;
        playerPos.x = playerPos.x - objectPos.x;
        playerPos.y = playerPos.y - objectPos.y;

        float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;
        angle = angle + 90;
        //pivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0,Mathf.Lerp(pivot.transform.rotation.z, angle, 0.7f)));
        pivot.transform.rotation = Quaternion.RotateTowards(pivot.transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * turnSpeed);
        if (shoot == true)
        {
            shoot = false;
            var x = Instantiate(bullet, gunPoint.transform.position, pivot.transform.rotation);
            x.GetComponent<Rigidbody2D>().AddForce(-x.transform.up * 200);
            StartCoroutine(Shoot());
        }
    }
    IEnumerator changeDir()
    {
        yield return new WaitForSeconds(Random.Range(3, 6)); 
    }
    IEnumerator Shoot()
    {

        yield return new WaitForSeconds(reloadTime);
        shoot = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetTrigger("Extract");
        if (collision.gameObject.layer == 11)
        {
            anim.SetBool("Extract", true);
        }
    }
}
