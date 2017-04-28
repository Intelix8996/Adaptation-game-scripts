using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMeasure : MonoBehaviour {

    [SerializeField]
    private float Speed = -1;

    [SerializeField]
    private Vector3 Origin;
    [SerializeField]
    private Vector3 NewOrigin;

    public IEnumerator Measurer ()
    {
        while (true)
        {
            Origin = transform.position;
            yield return new WaitForSeconds(1f);
            NewOrigin = transform.position;
            Speed = Vector3.Distance(Origin, NewOrigin);
            Debug.Log("Speed: " + Speed + "uM/s");
            Console._Console.PrintOther(" <color=#c0c0c0ff>Speed:</color> " + Speed + "uM/s");
        }
    }
}
