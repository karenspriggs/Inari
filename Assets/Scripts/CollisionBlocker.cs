using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Referenced from: https://www.youtube.com/watch?v=-yjKyI8NfKA
public class CollisionBlocker : MonoBehaviour
{
    public CapsuleCollider2D characterCollider;
    public CapsuleCollider2D characterCollisionBlocker;

    void Start()
    {
        Physics2D.IgnoreCollision(characterCollider, characterCollisionBlocker, true);
    }
}
