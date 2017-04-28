using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketMarker : MonoBehaviour {

    public Vector3 setItemTransform = new Vector3(0, 0, 0);

    public string Direction = "";

    public bool isSocketOccured = false;

    private void Start()
    {
        if (Direction != null)
            setItemTransform = transform.position;
        else
            Debug.LogError("No direction set in socket: " + gameObject.name);
    }
}
