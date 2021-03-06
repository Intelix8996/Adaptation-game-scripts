﻿using UnityEngine;
using UnityEngine.UI;

public class InventoryManadger : MonoBehaviour {

    public InventorySocket[] Inventory = new InventorySocket[15];
    public ActiveItemSocket[] ActiveSlots = new ActiveItemSocket[5];
    public ActiveItemSocket SelectedSocket;

    [SerializeField]
    private GameObject CraftingTable;

    public bool isInventoryOpened = false;

    private void Start()
    {
        foreach (InventorySocket sc in Inventory)
        {
            sc.gameObject.SetActive(isInventoryOpened);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !GetComponent<Console>().isConsoleActive)
        {
            isInventoryOpened = !isInventoryOpened;

            foreach (InventorySocket sc in Inventory)
            {
                sc.GetComponent<Image>().enabled = isInventoryOpened;
                sc.NumB.GetComponent<Text>().enabled = isInventoryOpened;
            }

            CraftingTable.SetActive(isInventoryOpened);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            MakeSocketActive(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            MakeSocketActive(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            MakeSocketActive(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            MakeSocketActive(3);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            MakeSocketActive(4);
        if (SelectedSocket != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !GetComponent<BuildHandler>().isBlueprintActive)
            {
                SelectedSocket.isActive = false;
                SelectedSocket = null;
            }
        }

        if (SelectedSocket != null && SelectedSocket.ID == 1)
            GetComponent<BuildHandler>().isBlueprintActive = true;
        else
            GetComponent<BuildHandler>().isBlueprintActive = false;
    }

    public void MakeSocketActive(int indx)
    {
        if (SelectedSocket != null)
            SelectedSocket.isActive = false;

        SelectedSocket = ActiveSlots[indx];
        SelectedSocket.isActive = true;
    }
}
