using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour {

    [SerializeField]
    private BuildHandler Handler;

    private void Awake()
    {
        Handler = GameObject.FindGameObjectWithTag("Player").GetComponent<BuildHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Handler.BuildAllowed = false;

        Renderer[] _R = GetComponentsInChildren<Renderer>();

        foreach (Renderer R in _R)
        {
            R.material = null;
            R.material = Handler.BuildNotAllowedMaterial;
        }

        _R = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Handler.isCursorOnMarker)
        {
            Handler.BuildAllowed = false;

            Renderer[] _R = GetComponentsInChildren<Renderer>();

            foreach (Renderer R in _R)
            {
                R.material = null;
                R.material = Handler.BuildNotAllowedMaterial;
            }

            _R = null;
        }
        else
        {
            Handler.BuildAllowed = true;

            Renderer[] _R = GetComponentsInChildren<Renderer>();

            foreach (Renderer R in _R)
            {
                R.material = null;
                R.material = Handler.BuildAllowedMaterial;
            }

            _R = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Handler.BuildAllowed = true;

        Renderer[] _R = GetComponentsInChildren<Renderer>();

        foreach (Renderer R in _R)
        {
            R.material = null;
            R.material = Handler.BuildAllowedMaterial;
        }

        _R = null;
    }
}
