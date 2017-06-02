using UnityEngine;
using UnityEngine.UI;

public class CraftPart : MonoBehaviour {

    public int ID = 0;
    public int Amount = 0;
    public GameObject Text;

    private void Start()
    {
        if (ID != 0)
            Text.GetComponent<Text>().text = Amount.ToString();

        GetComponent<Image>().sprite = Resources.Load(ItemLibrary._ItemGenerator.ItemList[ID].IconPath, typeof(Sprite)) as Sprite;
    }
}
