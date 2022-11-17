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
    public List<InariAttack> groundBasicAttacks;
    public List<InariAttack> airBasicAttacks;
    
    public int basicAttacksIndex = 0;


    private void Awake()
    {
        InitializeAttacks();
    }

    public void InitializeAttacks()
    {
        groundBasicAttacks = new List<InariAttack> 
        { 
            new InariAttack("PlayerDollBasicAttack", true, true, true), 
            new InariAttack("PlayerDollSwipeUp", true, true, true), 
            new InariAttack("PlayerDollStab", true, true, true) 
        };

        airBasicAttacks = new List<InariAttack>
        {
            new InariAttack("PlayerDollAirBasic", true, true, true),
            new InariAttack("PlayerDollAirSwipeUp", true, true, true),
            new InariAttack("PlayerDollAirStab", true, true, true)
        };
    }

    public bool CanBasicAttackCombo()
    {
        return CanBasicAttackCombo(false);
    }
    public bool CanBasicAttackCombo(bool isGrounded)
    {
        if (isGrounded)
        {
            return CanGroundAttackCombo();
        }
        else
        {
            return CanAirAttackCombo();
        }
    }

    public bool CanGroundAttackCombo()
    {
        return CanContinueCombo(groundBasicAttacks);
    }

    public bool CanAirAttackCombo()
    {
        return CanContinueCombo(airBasicAttacks);
    }

    public bool CanContinueCombo(List<InariAttack> attacks)
    {
        if (basicAttacksIndex < attacks.Count - 1) return true;
        return false;
    }

    public bool CanDashCancel()
    {
        return groundBasicAttacks[basicAttacksIndex].canDashCancel;
    }

    public bool CanJumpCancel()
    {
        return groundBasicAttacks[basicAttacksIndex].canJumpCancel;
    }

    public bool CanHeavyAttackCancel()
    {
        return groundBasicAttacks[basicAttacksIndex].canHeavyAttackCancel;
    }
}
