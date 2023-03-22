using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable // priest tai paveldejo is "Collidable"
{
    /*protected override void OnCollide(Collider2D coll)
    {
        // base.OnCollide(coll);
        Debug.Log("Grant coins");
    } CIA OLD */
    public Sprite emptyChest;
    public int coinAmount = 5;

    protected override void OnCollect()
    {
              // base.OnCollect(); // arba collected = true;
              // Debug.Log("Grant coins");

        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.coins += coinAmount;
            GameManager.instance.ShowText("+" + coinAmount + " Coins!", 25, Color.yellow, transform.position, Vector3.up * 50, 1.5f);
            Debug.Log("Grant " + coinAmount + " Coins!");
        }

    }
}
