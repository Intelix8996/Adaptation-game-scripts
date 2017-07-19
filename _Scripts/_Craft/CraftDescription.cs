using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CraftDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    private ItemBase Item;

    [SerializeField]
    private GameObject Description;

    private bool addPos = false;
    private float interpolateValue = 0;

    private void Start()
    {
        Item = ItemLibrary._ItemGenerator.ItemList[GetComponent<CraftPart>().ID];
    }

    private void Update()
    {
        if (addPos)
        {
            Description.GetComponent<Text>().color = Color.Lerp(new Color(0, 0, 0, 0), new Color(255, 255, 255, 255), interpolateValue);
            interpolateValue += 0.01f * Time.deltaTime;
        }
        if (interpolateValue >= 1)
            addPos = false;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        Description.GetComponent<Text>().text = Item.Name;
        Description.transform.position = transform.position + new Vector3(0, -80, 0);
        addPos = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        Description.GetComponent<Text>().color = new Color(0, 0, 0, 0);
        addPos = false;
        interpolateValue = 0;
    }
}