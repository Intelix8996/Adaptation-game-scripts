using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour {

    private int _i = 0;
    private int rand = -1;

    private float x;
    private float y = 10f;
    private float yRot = 90f;
    private float z;

    [SerializeField]
    private GameObject ItemSpawn;
    [SerializeField]
    private GameObject Item1;
    [SerializeField]
    private GameObject Item2;
    private GameObject ItemB;

    private void Start()
    {
        _i = Random.Range(3, 25);
        EarlySpawnCrates();
    }

    void EarlySpawnCrates()
    {
        for (int i = 0; i < _i; i++)
        {
            rand = Random.Range(1, 3);

            switch (rand)
            {
                case 1: ItemSpawn = Item1; break;
                case 2: ItemSpawn = Item2; break;
            }

            x = Random.Range(-37f, 65f);
            z = Random.Range(-55f, 170f);
            yRot = Random.Range(-360f, 360f);
            ItemB = Instantiate(ItemSpawn, new Vector3(x, y, z), new Quaternion(0, yRot, 0, yRot)) as GameObject;
            ItemB.transform.parent = GameObject.FindGameObjectWithTag("DroppedItemsFolder").transform;
            ItemB.transform.eulerAngles = new Vector3(0, yRot, 0);
            ItemB = null;
        }

        ItemB = null;
        Destroy(ItemB);
    }

    public void CustomSpawnWood (int num)
    {
        ItemSpawn = Item2;

        for (int i = 0; i < num; i++)
        {
            x = Random.Range(-37f, 65f);
            z = Random.Range(-55f, 170f);
            yRot = Random.Range(-360f, 360f);
            ItemB = Instantiate(ItemSpawn, new Vector3(x, y, z), new Quaternion(0, yRot, 0, yRot)) as GameObject;
            ItemB.transform.parent = GameObject.FindGameObjectWithTag("DroppedItemsFolder").transform;
            ItemB.transform.eulerAngles = new Vector3(0, yRot, 0);
            ItemB = null;
        }

        ItemB = null;
        Destroy(ItemB);
    }

}
