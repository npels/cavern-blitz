using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour {

    public static EquipmentItem chestArmor;
    public static EquipmentItem glovesArmor;
    public static EquipmentItem bootsArmor;
    public static EquipmentItem trinket;

    public static ToolItem leftHand;
    public static ToolItem rightHand;

    public static float armorValue;
    public static float speedBonus;
    public static float miningSpeedBonus;
    public static float attackSpeedBonus;

    public static float attackDamage;
    public static float attackSpeed;

    public static float miningSpeed;
    public static float miningDamage;

    private void OnEnable() {
        if (chestArmor != null) chestArmor.EquipItem();
        if (glovesArmor != null) glovesArmor.EquipItem();
        if (bootsArmor != null) bootsArmor.EquipItem();
        if (trinket != null) trinket.EquipItem();
        if (leftHand != null) leftHand.EquipItem();
        if (rightHand != null) rightHand.EquipItem();
    }

    private void OnDisable() {
        if (chestArmor != null) chestArmor.RemoveItem();
        if (glovesArmor != null) glovesArmor.RemoveItem();
        if (bootsArmor != null) bootsArmor.RemoveItem();
        if (trinket != null) trinket.RemoveItem();
        if (leftHand != null) leftHand.RemoveItem();
        if (rightHand != null) rightHand.RemoveItem();
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

    public static void AddTool(ToolItem item, bool isLeft) {
        if (isLeft) leftHand = item;
        else rightHand = item;

        if (item.type == ToolItem.ToolType.PICKAXE) {
            miningSpeed += item.miningSpeed;
            miningDamage += item.miningDamage;
        } else {
            attackDamage += item.attackDamage;
            attackSpeed += item.attackSpeed;
        }
    }

    public static void RemoveTool(ToolItem item, bool isLeft) {
        if (item == null) return;
        if (isLeft) leftHand = null;
        else rightHand = null;

        if (item.type == ToolItem.ToolType.PICKAXE) {
            miningSpeed -= item.miningSpeed;
            miningDamage -= item.miningDamage;
        } else {
            attackDamage -= item.attackDamage;
            attackSpeed -= item.attackSpeed;
        }
    }

    public static void SwapTool(ToolItem item, bool isLeft) {
        ToolItem oldTool = isLeft ? leftHand : rightHand;
        if (isLeft) leftHand = item;
        else rightHand = item;

        if (oldTool != null) {
            if (oldTool.type == ToolItem.ToolType.PICKAXE) {
                miningSpeed -= oldTool.miningSpeed;
                miningDamage -= oldTool.miningDamage;
            } else {
                attackDamage -= oldTool.attackDamage;
                attackSpeed -= oldTool.attackSpeed;
            }
        }

        if (item.type == ToolItem.ToolType.PICKAXE) {
            miningSpeed += item.miningSpeed;
            miningDamage += item.miningDamage;
        } else {
            attackDamage += item.attackDamage;
            attackSpeed += item.attackSpeed;
        }
    }
}
