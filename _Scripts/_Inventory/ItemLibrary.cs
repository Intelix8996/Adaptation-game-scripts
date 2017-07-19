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
}