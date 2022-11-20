using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropDown(CapsuleCollider2D playerCollider)
    {
        Physics2D.IgnoreCollision(playerCollider, gameObject.GetComponent<BoxCollider2D>());
        StartCoroutine(TurnCollisionBackOn(playerCollider));
    }

    IEnumerator TurnCollisionBackOn(CapsuleCollider2D playerCollider)
    {
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, gameObject.GetComponent<BoxCollider2D>(), false);
        yield break;
    }
}
