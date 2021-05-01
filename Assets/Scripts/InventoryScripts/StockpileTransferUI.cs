using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileTransferUI : MonoBehaviour {

    public GameObject inventoryObject;
    public GameObject stockpileObject;

    public GameObject armorChestObject;
    public GameObject armorGloveObject;
    public GameObject armorBootObject;
    public GameObject trinketObject;

    private Inventory playerInventory;
    private Inventory stockpileInventory;

    private List<InventorySlot> playerMenuSlots;
    private List<InventorySlot> stockpileSlots;

    private EquipmentSlot armorChestSlot;
    private EquipmentSlot armorGloveSlot;
    private EquipmentSlot armorBootSlot;
    private EquipmentSlot trinketSlot;
    public ToolSlot leftSlot;
    public ToolSlot rightSlot;

    private bool inventoryOpened = false;
    private bool itemIsSelected = false;

    public InventorySlot selectedItem;

    private void OnEnable() {
        playerInventory = BaseManager.instance.playerInventory;
        if (playerInventory.stacks == null) {
            stockpileInventory.InitInventory();
        }

        stockpileInventory = BaseManager.instance.stockpileInventory;
        if (stockpileInventory.stacks == null) {
            stockpileInventory.InitInventory();
        }

        armorChestSlot = armorChestObject.GetComponent<EquipmentSlot>();
        armorGloveSlot = armorGloveObject.GetComponent<EquipmentSlot>();
        armorBootSlot = armorBootObject.GetComponent<EquipmentSlot>();
        trinketSlot = trinketObject.GetComponent<EquipmentSlot>();

        inventoryObject.GetComponent<Inventory>().stacks = playerInventory.stacks;
        stockpileInventory.GetComponent<Inventory>().stacks = stockpileInventory.stacks;

        playerInventory.UpdateUIEvent += UpdateUI;
        stockpileInventory.UpdateUIEvent += UpdateUI;

        playerMenuSlots = new List<InventorySlot>();
        stockpileSlots = new List<InventorySlot>();

        InventorySlot[] slots = inventoryObject.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots) {
            if (!(slot is EquipmentSlot) && !(slot is ToolSlot)) playerMenuSlots.Add(slot);
        }

        slots = stockpileObject.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots) {
            stockpileSlots.Add(slot);
        }

        UpdateUI();
    }

    private void FixedUpdate() {
        if (itemIsSelected) {
            FollowCurser();
        }
    }

    public void OpenInventory() {
        itemIsSelected = false;
        inventoryOpened = true;
        gameObject.SetActive(true);
        stockpileInventory.LoadBaseInventory();
        playerInventory.SavePlayerInventory();
        BaseManager.instance.player.GetComponent<PlayerMovement>().canMove = false;
        BaseManager.instance.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        UpdateUI();
    }

    public void CloseInventory() {
        if (itemIsSelected) {
            int index = playerInventory.TryAddItem(selectedItem.stack.item, selectedItem.stack.count);
            if (index == -1) {
                Destroy(selectedItem.itemObject);
                selectedItem.itemObject = null;
                selectedItem.stack = null;
            } else {
                InventorySlot slot = playerMenuSlots[index];
                Destroy(slot.itemObject);
                slot.itemObject = selectedItem.itemObject;
                slot.itemObject.transform.SetParent(slot.transform);
                selectedItem.itemObject = null;
                selectedItem.stack = null;
            }
        }
        itemIsSelected = false;
        inventoryOpened = false;
        gameObject.SetActive(false);
        stockpileInventory.SaveBaseInventory();
        playerInventory.SavePlayerInventory();
        BaseManager.instance.player.GetComponent<PlayerMovement>().canMove = true;

        UpdateUI();
    }

    public void UpdateUI() {
        for (int i = 0; i < playerInventory.stacks.Count; i++) {
            playerMenuSlots[i].UpdateSlot(playerInventory.stacks[i]);
        }

        for (int i = 0; i < stockpileInventory.stacks.Count; i++) {
            stockpileSlots[i].UpdateSlot(stockpileInventory.stacks[i]);
        }

        armorChestSlot.UpdateSlot(new ItemStack(false, PlayerAttributes.chestArmor, 1));
        armorGloveSlot.UpdateSlot(new ItemStack(false, PlayerAttributes.glovesArmor, 1));
        armorBootSlot.UpdateSlot(new ItemStack(false, PlayerAttributes.bootsArmor, 1));
        trinketSlot.UpdateSlot(new ItemStack(false, PlayerAttributes.trinket, 1));
        leftSlot.UpdateSlot(new ItemStack(false, PlayerAttributes.leftHand, 1));
        rightSlot.UpdateSlot(new ItemStack(false, PlayerAttributes.rightHand, 1));
    }

    #region Inventory Organization Functions
    public void OnClick(GameObject item) {
        if (inventoryOpened && itemIsSelected) {
            PutSelectedItem(item);

            UpdateUI();
        } else if (inventoryOpened && !itemIsSelected) {
            PickupItem(item);

            UpdateUI();
        }
    }

    public void OnClickEquipment(GameObject item) {
        if (inventoryOpened && itemIsSelected) {
            if (selectedItem.stack.item is EquipmentItem) {
                EquipmentItem equipment = (EquipmentItem)selectedItem.stack.item;
                if (equipment.type == item.GetComponent<EquipmentSlot>().slotType) {
                    PutSelectedItem(item);
                    PlayerAttributes.SwapEquipment(equipment);
                    UpdateUI();
                }
            }
        } else if (inventoryOpened && !itemIsSelected) {
            EquipmentItem equipment = item.GetComponentInChildren<EquipmentItem>();
            PickupItem(item);
            PlayerAttributes.RemoveEquipment(equipment);
            UpdateUI();
        }
    }

    public void OnClickTool(GameObject item) {
        if (inventoryOpened && itemIsSelected) {
            if (selectedItem.stack.item is ToolItem) {
                ToolItem tool = (ToolItem)selectedItem.stack.item;
                PutSelectedItem(item);
                PlayerAttributes.SwapTool(tool, item == leftSlot.gameObject);
                UpdateUI();
            }
        } else if (inventoryOpened && !itemIsSelected) {
            ToolItem tool = item.GetComponentInChildren<ToolItem>();
            PickupItem(item);
            PlayerAttributes.RemoveTool(tool, item == leftSlot.gameObject);
            UpdateUI();
        }
    }

    private void PutSelectedItem(GameObject item) {
        int index = GetIndexOfItem(item);
        bool inPlayerInventory = playerMenuSlots.Contains(item.GetComponent<InventorySlot>());
        InventorySlot slot;
        if (index < -1) {
            switch (index) {
                case -2:
                    slot = armorChestSlot;
                    break;
                case -3:
                    slot = armorGloveSlot;
                    break;
                case -4:
                    slot = armorBootSlot;
                    break;
                case -5:
                    slot = trinketSlot;
                    break;
                case -6:
                    slot = leftSlot;
                    break;
                case -7:
                    slot = rightSlot;
                    break;
                default:
                    slot = null;
                    break;
            }
        } else {
            slot = inPlayerInventory ? playerMenuSlots[index] : stockpileSlots[index];
        }
        ItemStack stack = slot.stack;
        GameObject obj = slot.itemObject;

        if (stack.item == null) {
            slot.stack = selectedItem.stack;
            slot.itemObject = selectedItem.itemObject;

            selectedItem.stack = null;
            selectedItem.itemObject = null;

            if (inPlayerInventory && index > -1) playerInventory.SetItemStack(index, slot.stack);
            else if (index > -1) stockpileInventory.TryAddItem(slot.stack.item, slot.stack.count, false);

            slot.itemObject.transform.SetParent(slot.transform);

            itemIsSelected = false;
        } else {
            if (inPlayerInventory) {
                slot.stack.allowStockpile = false;
                slot.stack.maxCount = slot.stack.item.maxStack;
            }
            if (slot.stack.item == selectedItem.stack.item && slot.stack.maxCount >= slot.stack.count + selectedItem.stack.count) {
                slot.stack.count += selectedItem.stack.count;

                selectedItem.stack = null;
                Destroy(selectedItem.itemObject);
                selectedItem.itemObject = null;

                if (inPlayerInventory && index > -1) playerInventory.SetItemStack(index, slot.stack);
                else if (index > -1) stockpileInventory.SetItemStack(index, slot.stack);

                itemIsSelected = false;
            } else if (slot.stack.item.maxStack < slot.stack.count) {
                selectedItem.UpdateSlot(new ItemStack(false, slot.stack.item, slot.stack.item.maxStack));
                slot.stack.count -= slot.stack.item.maxStack;

                if (inPlayerInventory && index > -1) playerInventory.SetItemStack(index, slot.stack);
                else if (index > -1) stockpileInventory.SetItemStack(index, slot.stack);

                slot.itemObject.transform.SetParent(slot.transform);
                selectedItem.itemObject.transform.SetParent(selectedItem.transform);

                itemIsSelected = true;
            } else {
                slot.stack = selectedItem.stack;
                slot.itemObject = selectedItem.itemObject;

                selectedItem.stack = stack;
                selectedItem.itemObject = obj;

                if (inPlayerInventory && index > -1) playerInventory.SetItemStack(index, slot.stack);
                else if (index > -1) stockpileInventory.TryAddItem(slot.stack.item, slot.stack.count, false);

                slot.itemObject.transform.SetParent(slot.transform);
                selectedItem.itemObject.transform.SetParent(inventoryObject.transform);

                itemIsSelected = true;
            }
        }
    }

    private void PickupItem(GameObject item) {
        int index = GetIndexOfItem(item);
        bool inPlayerInventory = playerMenuSlots.Contains(item.GetComponent<InventorySlot>());
        InventorySlot slot;
        if (index < -1) {
            switch (index) {
                case -2:
                    slot = armorChestSlot;
                    break;
                case -3:
                    slot = armorGloveSlot;
                    break;
                case -4:
                    slot = armorBootSlot;
                    break;
                case -5:
                    slot = trinketSlot;
                    break;
                case -6:
                    slot = leftSlot;
                    break;
                case -7:
                    slot = rightSlot;
                    break;
                default:
                    slot = null;
                    break;
            }
        } else {
            slot = inPlayerInventory ? playerMenuSlots[index] : stockpileSlots[index];
        }

        if (slot.stack.item != null) {
            if (slot.stack.item.maxStack < slot.stack.count) {

                selectedItem.UpdateSlot(new ItemStack(false, slot.stack.item, slot.stack.item.maxStack));
                slot.stack.count -= slot.stack.item.maxStack;

                if (inPlayerInventory && index > -1) playerInventory.SetItemStack(index, slot.stack);
                else if (index > -1) stockpileInventory.SetItemStack(index, slot.stack);

                selectedItem.itemObject.transform.SetParent(selectedItem.transform);
            } else {
                selectedItem.stack = slot.stack;
                selectedItem.itemObject = slot.itemObject;

                slot.stack = new ItemStack(!inPlayerInventory);
                slot.itemObject = null;

                if (inPlayerInventory && index > -1) playerInventory.SetItemStack(index, slot.stack);
                else if (index > -1) stockpileInventory.SetItemStack(index, slot.stack);

                selectedItem.itemObject.transform.SetParent(selectedItem.transform);
            }

            itemIsSelected = true;
        } else {
            selectedItem.stack = null;
            selectedItem.itemObject = null;

            itemIsSelected = false;
        }
    }

    private void FollowCurser() {
        Vector2 mousePos = Input.mousePosition;
        selectedItem.itemObject.transform.position = mousePos;
    }

    private int GetIndexOfItem(GameObject item) {
        int result = playerMenuSlots.IndexOf(item.GetComponent<InventorySlot>());
        if (result >= 0) {
            return result;
        } else {
            result = stockpileSlots.IndexOf(item.GetComponent<InventorySlot>());
            if (result >= 0) return result;
        }
        if (item == armorChestObject) return -2;
        if (item == armorGloveObject) return -3;
        if (item == armorBootObject) return -4;
        if (item == trinketObject) return -5;
        if (item == leftSlot.gameObject) return -6;
        if (item == rightSlot.gameObject) return -7;
        else return -1;
    }

    #endregion 
}
