using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float moveSpeed = 5f;

    public GameObject pixel;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool lookingRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lookingRight = true;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(movement.x < 0 && lookingRight == true){
            Flip();
        }
        if(movement.x > 0 && lookingRight == false){
            Flip();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            moveSpeed = 1;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift)){
            moveSpeed = 5;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void Flip()
    {
        lookingRight = !lookingRight;

        Vector3 scale = pixel.transform.localScale;
        scale.x *= -1;
        pixel.transform.localScale = scale;
    }
}
