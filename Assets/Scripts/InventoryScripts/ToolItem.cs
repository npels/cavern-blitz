using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolItem : Item {

    public enum ToolType {
        WEAPON,
        PICKAXE
    }

    [Tooltip("The type of tool this is.")]
    public ToolType type;

    [Header("Weapon Variables")]
    [Tooltip("The damage that weapon does")]
    public float attackDamage;
    [Tooltip("How quickly this weapon attacks")]
    public float attackSpeed;

    [Header("Tool Variables")]
    [Tooltip("How quickly this tool mines")]
    public float miningSpeed;
    [Tooltip("The rock damage that this tool does")]
    public float miningDamage;

    public virtual void EquipItem() {
        return;
    }

    public virtual void RemoveItem() {
        return;
    }
}
