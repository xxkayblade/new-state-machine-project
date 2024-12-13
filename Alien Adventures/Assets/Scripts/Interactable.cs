using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    public Transform respawnPoint;

    [Header("Player Information")]
    private int currentHealth;
    public int collectedCoins;
    private int maxCoins;
    public GameObject collectibles;

    [Header("Tags")]
    private const string deathTag = "Death";
    private const string coinTag = "Coin";
    private const string respawnTag = "RespawnPoint";
    private const string endTag = "End";

    [Header("UI Elements")]
    public GameObject[] hearts;
    public TextMeshProUGUI coinTracker;

    private void Start()
    {
        currentHealth = hearts.Length;

        maxCoins = collectibles.transform.childCount;
        coinTracker.text = collectedCoins + "/" + maxCoins;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case deathTag:
                transform.position = respawnPoint.position;

                if (currentHealth > 0)
                {
                    currentHealth--;
                    hearts[currentHealth].SetActive(false);
                }
                else if (currentHealth == 0)
                {
                    string currentLevel = SceneManager.GetActiveScene().name;
                    SceneManager.LoadScene(currentLevel);
                }
                break;
            case respawnTag:
                respawnPoint = collision.gameObject.transform;
                break;
            case endTag:
                int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentLevelIndex + 1);
                break;
            case coinTag:
                {
                    collectedCoins++;
                    coinTracker.text = collectedCoins + "/" + maxCoins;
                    Destroy(collision.gameObject);
                }
                break;
        }

    }
}
