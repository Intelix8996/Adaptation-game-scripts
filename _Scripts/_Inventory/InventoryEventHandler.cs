using UnityEngine;

public class InventoryEventHandler : MonoBehaviour {

    public int ID = 0;
    public int AMOUNT = 0;

    public string socketNameBuffer = "";

    private GameObject PreviewIcon_B;
    [SerializeField]
    private GameObject PreviewSocket;


    public bool isDragged = false;
    public bool isDraggedOnNewSlot = false;

    private void FixedUpdate()
    {
        if (isDragged)
        {
            PreviewIcon_B.transform.position = Input.mousePosition;
        }
        else
        {
            DestroyPreview();
        }
    }

    public void DestroyPreview()
    {
        GameObject[] GO = GameObject.FindGameObjectsWithTag("PreviewSocket");

        foreach (GameObject O in GO)
        {
            Destroy(O);
        }

        GO = null;
    }

    public void CreatePreivew()
    {
        if (ID != 0)
        {
            PreviewIcon_B = Instantiate(PreviewSocket, Input.mousePosition, Quaternion.identity) as GameObject;
            PreviewIcon_B.GetComponent<PreviewSocket>().Item = ItemLibrary._ItemGenerator.ItemList[ID];
            PreviewIcon_B.GetComponent<PreviewSocket>().Number = AMOUNT;
            PreviewIcon_B.transform.SetParent(gameObject.transform);
            isDragged = true;
        }
    }
}
