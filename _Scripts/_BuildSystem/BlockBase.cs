using UnityEngine;

[System.Serializable]
public class BlockBase {

    public string Name = "";
    public int Id = 0;
    public GameObject Prefab;
    public string IconPath = "";
    [Multiline]
    public string Description = "";
}
