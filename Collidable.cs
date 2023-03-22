using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider2D))]

public class Collidable: MonoBehaviour
{   private BoxCollider2D boxCollider;
    public ContactFilter2D filter;
    private Collider2D[] hits = new Collider2D[10];

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        //Collision work
        boxCollider.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            // one way to do it: if(this.tag == "NPC_0")
            // Debug.Log(hits[i].name);

            OnCollide(hits[i]);

            // the array is not cleaned up, so we do it ourself
            hits[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {
        Debug.Log("OnCollide was not implemented in " + coll.name);
    }
       
}
