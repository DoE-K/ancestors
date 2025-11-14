using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShipScript : MonoBehaviour
{
    public float moveSpeed = 2f;           // Geschwindigkeit nach links
    public GameObject player;
    public Transform shipTransform;
    public Transform playerTransform;               // Referenz zum Spieler
    public Collider2D playerCollider;
    public Canvas mapUI;
    public KeyCode boardKey = KeyCode.E;   // Taste zum Einsteigen
    public Transform shipCenter;           // Punkt in der Mitte des Schiffs

    private bool playerNearby = false;     // Ist der Spieler in der Nähe?
    private bool playerOnBoard = false;    // Ist der Spieler an Bord?

    private bool sceneChangeStarted = false;

    void Start(){
        Collider2D playerCollider = player.GetComponent<Collider2D>();
    }

    void Update()
    {
        
        
        // Wenn Spieler in der Nähe und Taste gedrückt
        if (playerNearby && Input.GetKeyDown(boardKey))
        {
            BoardShip();
        }

        // Wenn Spieler an Bord, bewege das Schiff
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
        /*
        // Collider deaktivieren (damit der Spieler nicht mit dem Schiff kollidiert)
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        if (playerCollider != null)
            playerCollider.enabled = false;
            Debug.LogWarning("playerCollider is null!");*/

        if (!sceneChangeStarted)
        {
            sceneChangeStarted = true;
            StartCoroutine(ChangeSceneAfterDelay(15f)); // 10 Sekunden warten
        }
    }


    private void MoveShip()
    {
        shipTransform.transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        playerTransform.transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        //ScoreManager.SaveFinalScore("Player1");
    }

    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Szene wechseln – z. B. nächste Szene im Build-Index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Nähe erkennen
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
