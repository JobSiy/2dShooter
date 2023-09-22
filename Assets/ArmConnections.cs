using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmConnections : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] armParts;
    [SerializeField] SpringJoint2D spring;
    int i = 0;
    bool disconnect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(i=0; i<armParts.Length; i++)
        {
            if (armParts[i] == null)
            {
                disconnect = true;
                Destroy(spring);
            }
            else
            {
                if (disconnect == true)
                {
                    armParts[i].transform.parent = null;
                }
            }
        }
        disconnect = false;
    }
}
