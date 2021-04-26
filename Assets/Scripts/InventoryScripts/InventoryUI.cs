using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    #region Inventory Variables
    public GameObject inventoryBar;
    public GameObject inventoryMenu;
    public bool inBase = false;

    private List<InventorySlot> menuSlots;
    private List<InventorySlot> barSlots;

    public EquipmentSlot armorChestSlot;
    public EquipmentSlot armorGloveSlot;
    public EquipmentSlot armorBootSlot;
    public EquipmentSlot trinketSlot;
    public ToolSlot leftSlot;
    public ToolSlot rightSlot;

    private Inventory inventory;

    private bool inventoryOpened = false;
    #endregion

    #region Player Variables
    private GameObject player;
    private PlayerInteractions playerInteractions;
    private PlayerMovement playerMovement;
    private PlayerAttributes playerAttributes;
    #endregion

    #region Inventory Organization Variables
    private bool itemIsSelected;
    public InventorySlot selectedItem;
    #endregion

    #region Unity Functions 
    void Start() {
        inventory = inBase ? BaseManager.instance.playerInventory : GameManager.instance.inventory;
        inventory.UpdateUIEvent += UpdateUI;

        //Set up UI
        menuSlots = new List<InventorySlot>();
        InventorySlot[] slots = inventoryMenu.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots) {
            if (!(slot is EquipmentSlot) && !(slot is ToolSlot)) menuSlots.Add(slot);
        }

        barSlots = new List<InventorySlot>();
        if (inventory.numPrioritySlots > 0) {
            slots = inventoryBar.GetComponentsInChildren<InventorySlot>();
            foreach (InventorySlot slot in slots) {
                barSlots.Add(slot);
            }
        }

        inventoryBar.SetActive(true && !inBase);
        inventoryMenu.SetActive(false);

        player = inBase ? BaseManager.instance.player : GameManager.instance.player;
        playerInteractions = player.GetComponent<PlayerInteractions>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerAttributes = player.GetComponent<PlayerAttributes>();

        //Set up Inventory Organization
        itemIsSelected = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) && !inBase) {
            if (inventoryOpened) {
                CloseInventory();
            } else {
                OpenInventory();
            }
        }
    }
    private void FixedUpdate() {
        if (itemIsSelected) {
            FollowCurser();
        }
    }
    #endregion 

    #region Menu Functions
    public void OpenInventory()
    {
        itemIsSelected = false;
        inventoryOpened = true;
        inventoryMenu.SetActive(true);
        inventoryBar.SetActive(false);
        playerInteractions.SetMenuOpen(true);
        playerMovement.SetMenuOpen(true);
        playerMovement.canMove = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        UpdateUI();
    }

    public void CloseInventory()
    {
        if (itemIsSelected) {
            int index = inventory.TryAddItem(selectedItem.stack.item, selectedItem.stack.count);
            if (index == -1) {
                Destroy(selectedItem.itemObject);
                selectedItem.itemObject = null;
                selectedItem.stack = null;
            } else {
                InventorySlot slot = menuSlots[index];
                Destroy(slot.itemObject);
                slot.itemObject = selectedItem.itemObject;
                slot.itemObject.transform.SetParent(slot.transform);
                selectedItem.itemObject = null;
                selectedItem.stack = null;
            }
        }
        itemIsSelected = false;
        inventoryOpened = false;
        inventoryBar.SetActive(true && !inBase);
        inventoryMenu.SetActive(false);
        playerInteractions.SetMenuOpen(false);
        playerMovement.SetMenuOpen(false);
        playerMovement.canMove = true;

        UpdateUI();
    }
    #endregion

    public void UpdateUI() {
        for (int i = 0; i < inventory.stacks.Count; i++) {
            menuSlots[i].UpdateSlot(inventory.stacks[i]);
        }

        for (int i = 0; i < inventory.numPrioritySlots; i++) {
            barSlots[i].UpdateSlot(inventory.stacks[inventory.GetPriorityIndex(i)]);
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
        }
        else if (inventoryOpened && !itemIsSelected) {
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

    public void PutSelectedItem(GameObject item) {
        int index = GetIndexOfItem(item);
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
            slot = menuSlots[index];
        }
        ItemStack stack = slot.stack;
        GameObject obj = slot.itemObject;

        if (stack.item == null) {
            slot.stack = selectedItem.stack;
            slot.itemObject = selectedItem.itemObject;

            selectedItem.stack = null;
            selectedItem.itemObject = null;

            if (index > -1) inventory.SetItemStack(index, slot.stack);

            slot.itemObject.transform.SetParent(slot.transform);

            itemIsSelected = false;
        } else {
            slot.stack = selectedItem.stack;
            slot.itemObject = selectedItem.itemObject;

            selectedItem.stack = stack;
            selectedItem.itemObject = obj;

            if (index > -1) inventory.SetItemStack(index, slot.stack);

            slot.itemObject.transform.SetParent(slot.transform);
            selectedItem.itemObject.transform.SetParent(inventoryMenu.transform);
            
            itemIsSelected = true;
        }
    }

    private void PickupItem(GameObject item) {
        int index = GetIndexOfItem(item);
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
            slot = menuSlots[index];
        }

        if (slot.stack.item != null) {
            selectedItem.stack = slot.stack;
            selectedItem.itemObject = slot.itemObject;

            slot.stack = new ItemStack(inventory.allowStockpile);
            slot.itemObject = null;

            if (index > -1) inventory.SetItemStack(index, slot.stack);

            selectedItem.itemObject.transform.SetParent(inventoryMenu.transform);

            itemIsSelected = true;
        } else {
            selectedItem.stack = null;
            selectedItem.itemObject = null;

            itemIsSelected = false;
        }
    }

    private void FollowCurser()
    {
        Vector2 mousePos = Input.mousePosition;
        selectedItem.itemObject.transform.position = mousePos;
    }

    // Returns -1 if item is null
    private int GetIndexOfItem(GameObject item)
    {
        int slot = menuSlots.IndexOf(item.GetComponent<InventorySlot>());
        if (slot >= 0) return slot;
        if (item == armorChestSlot.gameObject) return -2;
        if (item == armorGloveSlot.gameObject) return -3;
        if (item == armorBootSlot.gameObject) return -4;
        if (item == trinketSlot.gameObject) return -5;
        if (item == leftSlot.gameObject) return -6;
        if (item == rightSlot.gameObject) return -7;
        else return -1;
    }

    #endregion 
}
