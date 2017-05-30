using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySocket : MonoBehaviour, IBeginDragHandler, IDragHandler, IEventSystemHandler, IPointerEnterHandler, IDropHandler, IEndDragHandler, IPointerClickHandler, IPointerExitHandler
{

    public ItemBase Item;
    public int Number;

    [SerializeField]
    private Text NumB;
    [SerializeField]
    private InventoryEventHandler Handler;
    [SerializeField]
    private GameObject Description;

    [SerializeField]
    private bool isDescriptionVisible = false;
    [SerializeField]
    private GameObject DescriptionIcon;
    [SerializeField]
    private GameObject DescriptionName;
    [SerializeField]
    private GameObject DescriptionDescription;
    [SerializeField]
    private GameObject DescriptionType;

    public static InventorySocket _Socket;

    private void Awake()
    {
        _Socket = this;
    }

    private void Start()
    {
        NumB = transform.GetComponentInChildren<Text>();
        Handler = GameObject.FindGameObjectWithTag("InventoryMain").GetComponent<InventoryEventHandler>();
        Item = ItemLibrary._ItemGenerator.ItemList[0];
        DescriptionIcon.GetComponent<Image>().sprite = Resources.Load("NullOnEmpty", typeof(Sprite)) as Sprite;
        DescriptionIcon.GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }

    private void FixedUpdate()
    {
        NumB.text = Convert.ToString(Number);
        
        if (Number <= 1)
            NumB.gameObject.SetActive(false);
        else
            NumB.gameObject.SetActive(true);

        if (Item.IconPath != null)
            GetComponent<Image>().sprite = Resources.Load(Item.IconPath, typeof(Sprite)) as Sprite;

        if (isDescriptionVisible)
        {
            Description.transform.position = Input.mousePosition + new Vector3(-125, 150, -50);
            DescriptionIcon.GetComponent<Image>().color = Color.Lerp(new Color(255, 255, 255, 0), new Color(255, 255, 255, 255), 1);
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

    public void OnPointerClick(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left && Item.Id != 0)
        {
            DescriptionName.GetComponent<Text>().text = Item.Name;
            DescriptionType.GetComponent<Text>().text = Item.Type;
            DescriptionDescription.GetComponent<Text>().text = Item.Description;
            DescriptionIcon.GetComponent<Image>().sprite = Resources.Load(Item.OnWhiteOrEmptyPath, typeof(Sprite)) as Sprite;

            Description.GetComponent<Animator>().Play("In");
            isDescriptionVisible = true;
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (isDescriptionVisible)
        {
            isDescriptionVisible = false;
            Description.GetComponent<Animator>().Play("Out");
            Description.transform.position = new Vector3(20000,20000,-15);
        }
    }
}
