using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InariAttack
{
    public string Name;

    public InariAttack(string name)
    {
        Name = name;
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
            new InariAttack("PlayerDollBasicAttack"), 
            new InariAttack("PlayerDollSwipeUp"), 
            new InariAttack("PlayerDollStab") 
        };
    }

    public bool CanBasicAttackCombo()
    {
        if (basicAttacksIndex < basicAttacks.Count) return true;
        return false;
    }
}
