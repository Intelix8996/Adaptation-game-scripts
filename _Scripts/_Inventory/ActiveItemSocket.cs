using UnityEngine;
using UnityEngine.UI;

public class ActiveItemSocket : MonoBehaviour {

    public int ID = 0;
    public bool isActive = false;

    private void FixedUpdate()
    {
        ID = GetComponent<InventorySocket>().Item.Id;

        if (isActive)
            GetComponent<Image>().color = new Color(0, 0, 255, 255);
        else
            GetComponent<Image>().color = new Color(255, 255, 255, 255);
    }

}
