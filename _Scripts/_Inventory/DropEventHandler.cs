using UnityEngine;
using UnityEngine.EventSystems;

public class DropEventHandler : MonoBehaviour, IEventSystemHandler, IDropHandler
{
    [SerializeField]
    private InventoryEventHandler Handler;

    public void OnDrop(PointerEventData data)
    {
        if (Handler.ID != 0 && ItemLibrary._ItemGenerator.ItemList[Handler.ID].Model != null)
        {
            GameObject BufferObj = Instantiate(ItemLibrary._ItemGenerator.ItemList[Handler.ID].Model, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity) as GameObject;
            BufferObj.GetComponent<Item>().Amount = Handler.AMOUNT;
            BufferObj = null;
            Handler.isDraggedOnNewSlot = true;
        }
        else
        {
            Debug.LogWarning("StandartModel is Missing");
            Handler.isDraggedOnNewSlot = true;
        }

        Handler.DestroyPreview();

        Handler.isDragged = false;
    }
}
