using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider2D))]
public class Player : Mover // seniau Fighter   // MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;
    //private float lastHealShown = 0.0f; pats sukuriau, bandziau teksto spam panaikint
    //private float cooldown = 1.0f;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        
    }
    protected override void ReceiveDamage(Damage dmg) // rodo hitsplats.
    {
        if (!isAlive)  // turetu nerodyt dmg jei esi jau dead.
            return;

        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }
    //private BoxCollider2D boxCollider;
    //private Vector3 moveDelta;
    //private RaycastHit2D hit;

    
    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
    }

    // Start is called before the first frame update
    //   ####nebereikia situ####
    //private void Start()
    //{
    //    boxCollider = GetComponent<BoxCollider2D>();
    //}

    // Update is called once per frame
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if(isAlive)
            UpdateMotor(new Vector3(x, y, 0));

        //reset moveDelta
        // seniau :  moveDelta = new Vector3(x, y, 0);

        // Swap sprite direction, wether you're going right or left

    }

    public void SwapSprite(int skinId)
    {
        GetComponent<SpriteRenderer>().sprite = GameManager.instance.playerSprites[skinId];
    }
    public void OnLevelUp()
    {
        maxHitpoint++;
        hitpoint = maxHitpoint;
    }
    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            OnLevelUp();
        }
    }

    public void Heal(int healingAmount)
    {
        if (hitpoint == maxHitpoint)
            return;

        hitpoint += healingAmount;
        if (hitpoint > maxHitpoint)
            hitpoint = maxHitpoint;

        //if (Time.time > lastHealShown) bandziau text spam panaikint
        //{
        //    lastHealShown += cooldown;
            GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
            GameManager.instance.OnHitpointChange();

        //}
        
     
    }

    public void Respawn()
    {
        Heal(maxHitpoint);
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }
}
