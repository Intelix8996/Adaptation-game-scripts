using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySocket : MonoBehaviour, IBeginDragHandler, IDragHandler, IEventSystemHandler, IPointerEnterHandler, IDropHandler, IEndDragHandler
{

    public ItemBase Item;
    public int Number;

    [SerializeField]
    private Text NumB;
    [SerializeField]
    private InventoryEventHandler Handler;

    public static InventorySocket _Socket;

    private void Awake()
    {
        _Socket = this;
    }

    private void Start()
    {
        Item = ItemLibrary._ItemGenerator.ItemList[0];
        NumB = transform.GetComponentInChildren<Text>();
        Handler = GameObject.FindGameObjectWithTag("InventoryMain").GetComponent<InventoryEventHandler>();
    }

    private void FixedUpdate()
    {
        NumB.text = Convert.ToString(Number);
        
        if (Number <= 1)
        {
            NumB.gameObject.SetActive(false);
        }
        else
        {
            NumB.gameObject.SetActive(true);
        }

        if (Item.IconPath != null)
        {
            GetComponent<Image>().sprite = Resources.Load(Item.IconPath, typeof(Sprite)) as Sprite;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Animator>().Play("SocketAnim");
    }

    public void OnBeginDrag(PointerEventData data)
    {
        Handler.ID = Item.Id;
        Handler.AMOUNT = Number;
        Handler.socketNameBuffer = gameObject.name;
        Item = ItemLibrary._ItemGenerator.ItemList[0];
        Number = 0;

        Handler.isDraggedOnNewSlot = false;

        Handler.CreatePreivew();
    }

    public void OnDrag(PointerEventData data)
    {
    }
    
    public void OnDrop(PointerEventData data)
    {
        if (Item.Id == 0)
        {
            Item = ItemLibrary._ItemGenerator.ItemList[Handler.ID];
            Number = Handler.AMOUNT;
            Handler.isDraggedOnNewSlot = true;
        }
        else if (Item.Id == Handler.ID)
        {
            Item = ItemLibrary._ItemGenerator.ItemList[Handler.ID];
            Number += Handler.AMOUNT;
            Handler.isDraggedOnNewSlot = true;
        }
        else if (Item.Id != Handler.ID)
        {
            GameObject[] socketGO = GameObject.FindGameObjectsWithTag("InventorySocket");

            foreach (GameObject b in socketGO)
            {
                if (b.name == Handler.socketNameBuffer)
                {
                    b.GetComponent<InventorySocket>().Item = ItemLibrary._ItemGenerator.ItemList[Item.Id];
                    b.GetComponent<InventorySocket>().Number = Number;
                }
            }

            socketGO = GameObject.FindGameObjectsWithTag("ActiveItemSocket");

            foreach (GameObject b in socketGO)
            {
                if (b.name == Handler.socketNameBuffer)
                {
                    b.GetComponent<InventorySocket>().Item = ItemLibrary._ItemGenerator.ItemList[Item.Id];
                    b.GetComponent<InventorySocket>().Number = Number;
                }
            }

            socketGO = null;

            Handler.isDraggedOnNewSlot = true;

            Item = ItemLibrary._ItemGenerator.ItemList[Handler.ID];
            Number = Handler.AMOUNT;
        }

        Handler.isDragged = false;
        Handler.DestroyPreview();
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (!Handler.isDraggedOnNewSlot)
        {
            Item = ItemLibrary._ItemGenerator.ItemList[Handler.ID];
            Number = Handler.AMOUNT;

            Handler.isDragged = false;
            Handler.DestroyPreview();
        }
    }
}
