using UnityEngine.SceneManagement;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;

    public int startingLives = 3;
    private int currentLives;

    private LifeUI lifeUI;
    private Player player;
    private LevelTransfer levelTransfer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentLives = startingLives;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Prevent duplicate
        }
    }

    private void Start()
    {
        // Delay UI hookup so it works even if UI loads a bit later
        Invoke(nameof(InitUI), 0.1f);
        if (currentLives <= 0) // This prevents resetting lives on scene load
            currentLives = startingLives;

        lifeUI = FindObjectOfType<LifeUI>();
        player = FindObjectOfType<Player>();

        if (lifeUI != null)
            lifeUI.SetLives(currentLives);
    }

    private void InitUI()
    {
        lifeUI = FindObjectOfType<LifeUI>();
        player = FindObjectOfType<Player>();
        if (lifeUI != null)
            lifeUI.SetLives(currentLives);
    }

    public void LoseLife(AudioClip deathClip)
    {
        currentLives--;
        Debug.Log("Life lost, currentLives: " + currentLives);
        if (lifeUI != null)
            lifeUI.SetLives(currentLives);

        if (currentLives <= 0)
            GameOver();
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");

        if (levelTransfer != null)
        {
            levelTransfer.sceneToLoad = "EndScreen";
            levelTransfer.TriggerTransfer();
        }
        else
        {
            Debug.LogWarning("LevelTransfer not found — fallback to direct load.");
            SceneManager.LoadScene("EndScreen");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedSceneInit());
    }

    private System.Collections.IEnumerator DelayedSceneInit()
    {
        yield return null; // wait 1 frame

        levelTransfer = FindObjectOfType<LevelTransfer>();
        lifeUI = FindObjectOfType<LifeUI>();
        player = FindObjectOfType<Player>();

        if (lifeUI != null)
            lifeUI.SetLives(currentLives);
    }

}
