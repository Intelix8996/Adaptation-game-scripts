using UnityEngine;
using System.Collections.Generic;

public class ItemLibrary : MonoBehaviour
{
    public static ItemLibrary _ItemGenerator;

    public List<ItemBase> ItemList = new List<ItemBase>();

    void Awake()
    {
        _ItemGenerator = this;
    }

    public ItemBase ItemGen(int win_id)
    {
        ItemBase item = new ItemBase();

        item.Name = ItemList[win_id].Name;
        item.Id = ItemList[win_id].Id;
        item.IconPath = ItemList[win_id].IconPath;
        item.Description = ItemList[win_id].Description;

        return item;
    }

}