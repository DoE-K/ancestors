using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShipScript : MonoBehaviour
{
    public float moveSpeed = 2f; 
    public GameObject player;
    public Transform shipTransform;
    public Transform playerTransform;
    public Collider2D playerCollider;
    public Canvas mapUI;
    public KeyCode boardKey = KeyCode.E;
    public Transform shipCenter;

    private bool playerNearby = false; 
    private bool playerOnBoard = false; 

    private bool sceneChangeStarted = false;

    void Start(){
        Collider2D playerCollider = player.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(boardKey))
        {
            BoardShip();
        }

        if (playerOnBoard)
        {
            playerCollider.enabled = false;
            mapUI.enabled = false;
            MoveShip();
        }
    }

    private void BoardShip()
    {
        playerTransform.position = shipCenter.position;  
        playerOnBoard = true;

        if (!sceneChangeStarted)
        {
            sceneChangeStarted = true;
            StartCoroutine(ChangeSceneAfterDelay(15f));
        }
    }

    private void MoveShip()
    {
        shipTransform.transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        playerTransform.transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
