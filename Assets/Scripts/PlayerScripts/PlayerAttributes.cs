using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour {

    public static EquipmentItem chestArmor;
    public static EquipmentItem glovesArmor;
    public static EquipmentItem bootsArmor;
    public static EquipmentItem trinket;

    public static float armorValue;
    public static float speedBonus;
    public static float miningSpeedBonus;
    public static float attackSpeedBonus;

    private void OnEnable() {
        if (chestArmor != null) chestArmor.EquipItem();
        if (glovesArmor != null) glovesArmor.EquipItem();
        if (bootsArmor != null) bootsArmor.EquipItem();
        if (trinket != null) trinket.EquipItem();
    }

    private void OnDisable() {
        if (chestArmor != null) chestArmor.RemoveItem();
        if (glovesArmor != null) glovesArmor.RemoveItem();
        if (bootsArmor != null) bootsArmor.RemoveItem();
        if (trinket != null) trinket.RemoveItem();
    }

    public static void AddEquipment(EquipmentItem item) {
        switch (item.type) {
            case EquipmentItem.EquipmentType.CHEST:
                chestArmor = item;
                break;
            case EquipmentItem.EquipmentType.GLOVES:
                glovesArmor = item;
                break;
            case EquipmentItem.EquipmentType.BOOTS:
                bootsArmor = item;
                break;
            case EquipmentItem.EquipmentType.TRINKET:
                trinket = item;
                break;
        }
        armorValue += item.armorValue;
        speedBonus += item.speedBonus;
        miningSpeedBonus += item.miningSpeedBonus;
        attackSpeedBonus += item.attackSpeedBonus;
    }

    public static void RemoveEquipment(EquipmentItem item) {
        if (item == null) return;
        switch (item.type) {
            case EquipmentItem.EquipmentType.CHEST:
                chestArmor = null;
                break;
            case EquipmentItem.EquipmentType.GLOVES:
                glovesArmor = null;
                break;
            case EquipmentItem.EquipmentType.BOOTS:
                bootsArmor = null;
                break;
            case EquipmentItem.EquipmentType.TRINKET:
                trinket = null;
                break;
        }
        armorValue -= item.armorValue;
        speedBonus -= item.speedBonus;
        miningSpeedBonus -= item.miningSpeedBonus;
        attackSpeedBonus -= item.attackSpeedBonus;
    }

    public static void SwapEquipment(EquipmentItem item) {
        EquipmentItem oldItem = null;
        switch (item.type) {
            case EquipmentItem.EquipmentType.CHEST:
                oldItem = chestArmor;
                chestArmor = item;
                break;
            case EquipmentItem.EquipmentType.GLOVES:
                oldItem = glovesArmor;
                glovesArmor = item;
                break;
            case EquipmentItem.EquipmentType.BOOTS:
                oldItem = bootsArmor;
                bootsArmor = item;
                break;
            case EquipmentItem.EquipmentType.TRINKET:
                oldItem = trinket;
                trinket = item;
                break;
        }

        if (oldItem != null) {
            armorValue -= oldItem.armorValue;
            speedBonus -= oldItem.speedBonus;
            miningSpeedBonus -= oldItem.miningSpeedBonus;
            attackSpeedBonus -= oldItem.attackSpeedBonus;
        }

        armorValue += item.armorValue;
        speedBonus += item.speedBonus;
        miningSpeedBonus += item.miningSpeedBonus;
        attackSpeedBonus += item.attackSpeedBonus;
    }
}
