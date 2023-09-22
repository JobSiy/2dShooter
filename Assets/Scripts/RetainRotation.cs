using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetainRotation : MonoBehaviour
{
    [SerializeField] float initRot;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float rotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        initRot = transform.eulerAngles.z; 
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.eulerAngles.z > initRot)
        {
            
        }
        transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(transform.eulerAngles.z, initRot, rotSpeed));
    }
}
