using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Transform heartsContainer;
    public GameObject heartPrefab;

    private Image[] heartsImages;

    private void Start()
    {
        int maxLives = LifeManager.Instance != null ? LifeManager.Instance.startingLives : 1;
        heartsImages = new Image[maxLives];

        for (int i = 0; i < maxLives; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsContainer);
            heartsImages[i] = heart.GetComponent<Image>();
        }

    }

    public void SetLives(int currentLives)
    {
        for (int i = 0; i < heartsImages.Length; i++)
        {
            if (heartsImages[i] != null)
                heartsImages[i].sprite = i < currentLives ? fullHeart : emptyHeart;
        }
    }
}
