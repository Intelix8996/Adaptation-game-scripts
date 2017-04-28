using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherMove : MonoBehaviour {

    [SerializeField]
    private GameObject Water;
    public bool inWater = false;


    private void Start()
    {
        Water = GameObject.FindGameObjectWithTag("Water");
    }
    private void OnTriggerEnter(Collider other)
    {
        inWater = true;
    }
    private void OnTriggerExit(Collider other)
    {
        inWater = false;
    }
    private void OnTriggerStay(Collider other)
    {
        Water.GetComponent<BoxCollider>().isTrigger = false;
        Water.GetComponent<BoxCollider>().center = new Vector3(-3.278255e-07f, -1.5455f, 3.278255e-07f);
    }
}
