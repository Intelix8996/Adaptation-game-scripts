using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftEventHandler : MonoBehaviour, IPointerClickHandler  {

    [SerializeField]
    private InventoryManadger Manadger;

    [SerializeField]
    private int CraftItemID = 0;

    [SerializeField]
    private int CraftItemAmount = 0;

    [SerializeField]
    private GameObject Part1;
    [SerializeField]
    private GameObject Part2;
    [SerializeField]
    private GameObject Part3;
    [SerializeField]
    private GameObject Part4;

    [SerializeField]
    private GameObject Preview;

    private void Start()
    {
        Preview.GetComponent<Image>().sprite = Resources.Load(ItemLibrary._ItemGenerator.ItemList[CraftItemID].IconPath, typeof(Sprite)) as Sprite;

        if (CraftItemAmount > 1)
            Preview.GetComponentInChildren<Text>().text = CraftItemAmount.ToString();
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (Verify())
            Craft();
        else
            Debug.Log("Unable to Craft");
    }
    
    private bool Verify()
    {
        bool isPart1OK = false;
        bool isPart2OK = false;
        bool isPart3OK = false;
        bool isPart4OK = false; 

        bool isEmptyOK = false;

        if (Part1.GetComponent<CraftPart>().ID == 0)
            isPart1OK = true;
        if (Part2.GetComponent<CraftPart>().ID == 0)
            isPart2OK = true;
        if (Part3.GetComponent<CraftPart>().ID == 0)
            isPart3OK = true;
        if (Part4.GetComponent<CraftPart>().ID == 0)
            isPart4OK = true;

        foreach (InventorySocket sc in Manadger.Inventory)
        {
            if (sc.Item.Id == Part1.GetComponent<CraftPart>().ID && sc.Number >= Part1.GetComponent<CraftPart>().Amount)
                isPart1OK = true;
            if (sc.Item.Id == Part2.GetComponent<CraftPart>().ID && sc.Number >= Part2.GetComponent<CraftPart>().Amount)
                isPart2OK = true;
            if (sc.Item.Id == Part3.GetComponent<CraftPart>().ID && sc.Number >= Part3.GetComponent<CraftPart>().Amount)
                isPart3OK = true;
            if (sc.Item.Id == Part4.GetComponent<CraftPart>().ID && sc.Number >= Part4.GetComponent<CraftPart>().Amount)
                isPart4OK = true;
            if (sc.Item.Id == 0)
                isEmptyOK = true;
        }

        if (isPart1OK && isPart2OK && isPart3OK && isPart4OK && isEmptyOK)
            return true;
        else
            return false;
    }

    private void Craft()
    {
        foreach (InventorySocket sc in Manadger.Inventory)
        {
            if (sc.Item.Id == Part1.GetComponent<CraftPart>().ID && sc.Number >= Part1.GetComponent<CraftPart>().Amount)
                sc.Number -= Part1.GetComponent<CraftPart>().Amount;
            if (sc.Item.Id == Part2.GetComponent<CraftPart>().ID && sc.Number >= Part2.GetComponent<CraftPart>().Amount)
                sc.Number -= Part2.GetComponent<CraftPart>().Amount;
            if (sc.Item.Id == Part3.GetComponent<CraftPart>().ID && sc.Number >= Part3.GetComponent<CraftPart>().Amount)
                sc.Number -= Part3.GetComponent<CraftPart>().Amount;
            if (sc.Item.Id == Part4.GetComponent<CraftPart>().ID && sc.Number >= Part4.GetComponent<CraftPart>().Amount)
                sc.Number -= Part4.GetComponent<CraftPart>().Amount;

            if (sc.Number == 0)
                sc.Item = ItemLibrary._ItemGenerator.ItemList[0];
        }

        foreach (InventorySocket sc in Manadger.Inventory)
        {
            if (sc.Item.Id == 0)
            {
                sc.Item = ItemLibrary._ItemGenerator.ItemList[CraftItemID];
                sc.Number += CraftItemAmount;
                break;
            }
            else if (sc.Item.Id == CraftItemID)
            {
                sc.Number += CraftItemAmount;
                break;
            }
        }
    }
}
