using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailBoss : MonoBehaviour
{
    [SerializeField] int hitPoints;
    [SerializeField] GameObject pivot;
    [SerializeField] float turnSpeed;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject leftTip;
    [SerializeField] GameObject rightTip;
    [SerializeField] bool right;
    [SerializeField] bool shoot;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject minion;
    [SerializeField] bool reCharge;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    [SerializeField] Animator anim;
    [SerializeField] bool doShoot;
    [SerializeField] bool decide;
    [SerializeField] bool deploy;
    [SerializeField] GameObject rightDeploy;
    [SerializeField] GameObject leftDeploy;
    [SerializeField] bool minionRight;
    [SerializeField] bool moveRight;
    [SerializeField] bool move;
    // Start is called before the first frame update
    void Start()
    {
        move = true;
    }
     
    // Update is called once per frame
    void Update()
    {
        if (moveRight)
        {
            rb.AddForce(Vector2.right * 1000 * Time.deltaTime);
        }
        else
        {
            rb.AddForce(-Vector2.right * 1000 * Time.deltaTime);
        }
        if(move == true)
        {
            move = false;
            moveRight = !moveRight;
            StartCoroutine(Move());
        }
        if (doShoot == true && deploy == false)
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
                StartCoroutine(Shoot());
            }
        }
        else
        {
            if (deploy == true)
            {
                if(leftDeploy == null & rightDeploy == null)
                {
                    deploy = false;
                    doShoot = true;
                }
                if (pivot.transform.eulerAngles.z == 0)
                {
                    deploy = false;
                    if (minionRight == true && rightDeploy != null)
                    {
                        minionRight = false;
                        var deployed = Instantiate(minion, rightDeploy.transform.position, this.transform.rotation);
                        deployed.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 700);
                    }
                    else
                    {
                        if (leftDeploy != null)
                        {
                            minionRight = true;
                            var deployed = Instantiate(minion, leftDeploy.transform.position, this.transform.rotation);
                            deployed.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 700);
                        }
                    }
                }
            }
            pivot.transform.rotation = Quaternion.RotateTowards(pivot.transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * turnSpeed * 2);
        }
        if(decide == true)  
        {
            decide = false;
            StartCoroutine(Decide());
        }
    }
    IEnumerator Move()
    {
        
        yield return new WaitForSeconds(Random.Range(3f, 6f));
        move = true;
    }
    IEnumerator Decide()
    {
        int randNum = Random.Range(1, 11);
        if(randNum > 4)
        {
            doShoot = true;
            yield return new WaitForSeconds(Random.Range(8, 10));
        }
        else
        {
            deploy = true;
            doShoot = false;
        }
        yield return new WaitForSeconds(3);
        decide = true;
    }

    IEnumerator Shoot()
    {
        if(right == true)
        {
            anim.SetTrigger("RShoot");
            yield return new WaitForSeconds(0.2f);
            var bulletOut =Instantiate(bullet, rightTip.transform.position, pivot.transform.rotation);
            bulletOut.GetComponent<Rigidbody2D>().AddForce(-bulletOut.transform.up * 500);
        }
        else
        {
            anim.SetTrigger("LShoot");
            yield return new WaitForSeconds(0.2f); 
            var bulletOut1 = Instantiate(bullet, leftTip.transform.position, pivot.transform.rotation);
            bulletOut1.GetComponent<Rigidbody2D>().AddForce(-bulletOut1.transform.up * 500);
        }
        right = !right;
        yield return new WaitForSeconds(1f);
        shoot = true;
    }
}
