using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InariAttack
{
    public string Name;
    public bool canDashCancel;
    public bool canJumpCancel;
    public bool canHeavyAttackCancel;

    public InariAttack(string name) : this(name, true, true, true) { }
    public InariAttack(string name, bool _canDashCancel, bool _canJumpCancel, bool _canHeavyAttackCancel)
    {
        Name = name;
        canDashCancel = _canDashCancel;
        canJumpCancel = _canJumpCancel;
        canHeavyAttackCancel = _canHeavyAttackCancel;
    }
}
public class PlayerAttacks : MonoBehaviour
{
    public List<InariAttack> basicAttacks;
    public int basicAttacksIndex = 0;

    private void Awake()
    {
        InitializeAttacks();
    }

    public void InitializeAttacks()
    {
        basicAttacks = new List<InariAttack> 
        { 
            new InariAttack("PlayerDollBasicAttack", true, true, true), 
            new InariAttack("PlayerDollSwipeUp", true, true, true), 
            new InariAttack("PlayerDollStab", true, true, true) 
        };
    }

    public bool CanBasicAttackCombo()
    {
        if (basicAttacksIndex < basicAttacks.Count-1) return true;
        return false;
    }

    public bool CanDashCancel()
    {
        return basicAttacks[basicAttacksIndex].canDashCancel;
    }

    public bool CanJumpCancel()
    {
        return basicAttacks[basicAttacksIndex].canJumpCancel;
    }

    public bool CanHeavyAttackCancel()
    {
        return basicAttacks[basicAttacksIndex].canHeavyAttackCancel;
    }
}
