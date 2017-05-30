using UnityEngine;

[System.Serializable]
public class ItemBase
{
    public string Name;
    public int Id;
    public string Type;
    public string IconPath;
    public string OnWhiteOrEmptyPath;
    [Multiline]
    public string Description;
    public GameObject Model;
}