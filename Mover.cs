using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    private Vector3 originalSize;

    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    public float ySpeed = 0.75f;
    public float xSpeed = 1.0f;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        originalSize = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    //private void FixedUpdate()
    //{

        //float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");

        ////reset moveDelta
        //moveDelta = new Vector3(x, y, 0);

        //// Swap sprite direction, wether you're going right or left
        //if (moveDelta.x > 0)
        //    transform.localScale = Vector3.one;
        //else if (moveDelta.x < 0)
        //    transform.localScale = new Vector3(-1, 1, 1);

        ////Make sure we can move in this direction, by casting a box there first, if the box returns null, we're free to move.
        //hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector3(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        //if (hit.collider == null)
        //{
        //    // Make rat move
        //    transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        //}

        //hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector3(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        //if (hit.collider == null)
        //{
        //    // Make rat move
        //    transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        //}

    //}

    protected virtual void UpdateMotor(Vector3 input)
    {
        //reset moveDelta
       moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0); // pries tai buvo : 
       // moveDelta = new Vector3(input.x, input.y, 0);
       // moveDelta = input;
        // Swap sprite direction, wether you're going right or left
        if (moveDelta.x > 0)
            transform.localScale = originalSize;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(originalSize.x * -1 , originalSize.y, originalSize.z); // paciam gale paeditinom i sita

        // Add push vector, if any                                       cia npc pushas prisidejo 
        moveDelta += pushDirection;                          /// cia

        //Reduce push force every frame, based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);   /// cia 

        //Make sure we can move in this direction, by casting a box there first, if the box returns null, we're free to move.
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector3(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            // Make rat move
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector3(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            // Make rat move
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }

}
