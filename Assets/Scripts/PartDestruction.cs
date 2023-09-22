using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDestruction : MonoBehaviour
{
    [SerializeField] int hitPoints;
    [SerializeField] GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hitPoints <= 0)
        {
            Destroy(this.gameObject);
        }
        if (hitPoints < 2 && particles)
        {
            particles.GetComponent<ParticleSystem>().startColor = Color.black;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            hitPoints -= 1;
        }
    }
}
