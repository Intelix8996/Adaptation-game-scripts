using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationArea : MonoBehaviour {

    public bool isInRadZone = false;
    [SerializeField]
    private GameObject StatsFile;

    private void OnTriggerEnter(Collider other)
    {
        StatsFile = other.gameObject;

        if (other.tag == "Player")
        {
            isInRadZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isInRadZone = false;
        }
    }

    private void Update()
    {
        if (isInRadZone)
        {
            StatsFile.GetComponent<CharStats>().Radiation += StatsFile.GetComponent<CharStats>().RadIncome;
        }
    }
}
