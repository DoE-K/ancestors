using UnityEngine;

public abstract class AnimalScript : MonoBehaviour
{
    [Header("General Settings")]
    public string speciesName = "Animal";
    public bool isMale;
    public bool isPredator;

    [Header("Stats")]
    public float speed = 3f;

    public Vector2 moveDirection;
    public float moveTimer;
    public float waitTimer;
    public bool isMoving;
    public float minMoveTime = 1f;
    public float maxMoveTime = 3f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;
    public bool movingRight = true;

    protected virtual void Start()
    {
        gameObject.name = speciesName;

        ChooseNewDirection();
    }

    protected virtual void Update()
    {
        Move();
        Act();
    }

    protected virtual void Move()
    {
        if (isMoving)
        {
            if (moveDirection.x < 0f && transform.localScale.x > 0f)
            {
                Flip();
            }
            else if (moveDirection.x > 0f && transform.localScale.x < 0f)
            {
                Flip();
            }

            transform.Translate(moveDirection * speed * Time.deltaTime);

            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0f)
            {
                isMoving = false;
                waitTimer = Random.Range(minWaitTime, maxWaitTime);
            }
        }
        else
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                ChooseNewDirection();
            }
        }
    }

    protected virtual void ReturnHome()
    {
        Debug.Log("nach hause gehen");
    }

    public void Flip()
    {
        movingRight = !movingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ChooseNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        moveTimer = Random.Range(minMoveTime, maxMoveTime);
        isMoving = true;
    }

    public abstract void Act();
}