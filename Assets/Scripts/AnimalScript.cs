using UnityEngine;

public class AnimalScript : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float minMoveTime = 1f;
    public float maxMoveTime = 3f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    private Vector2 moveDirection;
    private float moveTimer;
    private float waitTimer;
    private bool isMoving;

    void Start()
    {
        ChooseNewDirection();
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

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

    void ChooseNewDirection()
    {
        // Zufällige Bewegungsrichtung (N, S, W, O oder Diagonal)
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        moveTimer = Random.Range(minMoveTime, maxMoveTime);
        isMoving = true;
    }
}
