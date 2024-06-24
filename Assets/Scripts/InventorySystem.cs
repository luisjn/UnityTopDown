using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private GameObject inventoryFrame;
    [SerializeField] private Button[] slots;

    private int availableItems = 0;

    public void NewItem(ItemSO itemData)
    {
        slots[availableItems].gameObject.SetActive(true);
        slots[availableItems].GetComponent<Image>().sprite = itemData.icon;
        availableItems++;
    }
    
    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            int slotIndex = i;
            slots[i].onClick.AddListener(() => SlotClicked(slotIndex));
        }
    }

    private void SlotClicked(int slotIndex)
    {
        Debug.Log("Slot clicked " + slotIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryFrame.SetActive(!inventoryFrame.activeSelf);
        }
    }
}