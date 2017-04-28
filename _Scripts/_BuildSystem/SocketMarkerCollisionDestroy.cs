using UnityEngine;

public class SocketMarkerCollisionDestroy : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "SocketMarker")
            Destroy(other.gameObject);
         //   Debug.Log("OnTriggerStay Worked On " + other.gameObject.name);
    }
}
