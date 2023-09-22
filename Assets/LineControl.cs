using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] LineRenderer line;
    [SerializeField] PolygonCollider2D polyCollider;
    [SerializeField] GameObject ConnectedObject;
    private Vector3[] connectionPoints;
    private Vector2[] connectionPoints2d;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       /* connectionPoints[0] = this.transform.position;
        connectionPoints[1] = ConnectedObject.transform.position;
*/
        line.SetPosition(0, this.transform.position);
        line.SetPosition(1, ConnectedObject.transform.position);
        Vector2 offset = new Vector2(0.1f, 0.1f);
        polyCollider.points = new Vector2[]{ this.transform.position, ConnectedObject.transform.position, new Vector2(this.transform.position.x, this.transform.position.y) + offset,new Vector2( ConnectedObject.transform.position.x, ConnectedObject.transform.position.y) + offset };
      
      
    }
}
