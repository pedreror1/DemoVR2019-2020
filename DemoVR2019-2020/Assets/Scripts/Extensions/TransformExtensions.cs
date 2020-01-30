 
using UnityEngine;

public static class TransformExtensions 
{
    public static void  LookAtY(this Transform instance, Vector3 position, Vector3 target)
    {
        Vector3 direction = target - position;
        direction.y = 0;

        instance.rotation = Quaternion.LookRotation(direction);

    }
}
