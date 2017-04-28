using UnityEngine;

public class Item : MonoBehaviour {

    public int IdLocal = -1;
    public int Amount = 1;
    public string Name;
    public string IconPath;
    [Multiline]
    public string Description;

    private void Start()
    {
        if (IdLocal >= 0 && IdLocal < ItemLibrary._ItemGenerator.ItemList.Count)
        {
            Name = ItemLibrary._ItemGenerator.ItemList[IdLocal].Name;
            IconPath = ItemLibrary._ItemGenerator.ItemList[IdLocal].IconPath;
            Description = ItemLibrary._ItemGenerator.ItemList[IdLocal].Description;
        }
    }
}
