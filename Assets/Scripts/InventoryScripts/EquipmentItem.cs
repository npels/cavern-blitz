using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : Item {

    public enum EquipmentType {
        CHEST,
        GLOVES,
        BOOTS,
        TRINKET
    }

    [Tooltip("The type of equipment this is.")]
    public EquipmentType type;

    [Header("Armor Variables")]
    [Tooltip("The amount of armor that this piece of equipment provides.")]
    public float armorValue;
    [Tooltip("The movement speed bonus that this piece of armor provides.")]
    public float speedBonus;
    [Tooltip("The mining speed bonus that this piece of armor provides.")]
    public float miningSpeedBonus;
    [Tooltip("The attack speed bonus that this piece of armor provides.")]
    public float attackSpeedBonus;

    public virtual void EquipItem() {
        return;
    }

    public virtual void RemoveItem() {
        return;
    }
}
