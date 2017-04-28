using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PreviewSocket : MonoBehaviour
{

    public ItemBase Item;
    public int Number;

    [SerializeField]
    private Text NumB;
    [SerializeField]
    private InventoryEventHandler Handler;

    public static PreviewSocket _Socket;

    private void Awake()
    {
        _Socket = this;
    }

    private void Start()
    {
        Handler = GameObject.FindGameObjectWithTag("InventoryMain").GetComponent<InventoryEventHandler>();
        Item = ItemLibrary._ItemGenerator.ItemList[Handler.ID];
        NumB = transform.GetComponentInChildren<Text>();
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

        if (Item.OnWhiteOrEmptyPath != null)
        {
            GetComponent<Image>().sprite = Resources.Load(Item.OnWhiteOrEmptyPath, typeof(Sprite)) as Sprite;
        }
    }
}
