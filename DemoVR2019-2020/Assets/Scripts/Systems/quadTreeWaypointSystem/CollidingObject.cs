using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingObject : MonoBehaviour
{
    public float width = 1f;
    public float height = 1f;
    public Color DebugColor = Color.cyan;
    public bool isIntersecting;
    private void OnDrawGizmos()
    {
        Gizmos.color = DebugColor;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 1f, height));
    }
    public bool Intersects(Transform obj)
    {
        isIntersecting= (obj.position.x >= (transform.position.x - width/2) &&
                         obj.position.x <= (transform.position.x + width / 2) &&
                         obj.position.z >= (transform.position.z - height / 2) &&
                         obj.position.z <= (transform.position.z + height / 2)) ;

        return isIntersecting;
    }
   
}
