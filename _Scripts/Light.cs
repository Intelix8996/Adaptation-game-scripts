using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour {

    [Range(0, 360)]
    public float Angle = 45f;
    [Range(0, 10)]
    public float Speed = 0.01f;

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.AngleAxis(Angle, Vector3.right);
        Angle += Speed;

        if (Angle >= 361)
        {
            Angle = 1f;
        }
    }
}
