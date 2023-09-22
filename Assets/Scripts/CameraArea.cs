using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject cam;
    public  bool playerHere;
    [SerializeField] float camScale;
    void Start()
    {
        if(camScale == 0)
        {
            camScale = 5;
        }
        cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            cam.GetComponent<Camera>().orthographicSize = camScale;
            cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
           // playerHere = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
            playerHere = true;
        }
        else
        {
            
        }
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            playerHere = false;
        }
    }
}
