using UnityEngine;
using UnityEngine.UI;

public class HealthBarHUD : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private Image heart1;
    private Image heart2;
    private Image heart3;

    private void Start()
    {
        // Автоматически находим сердца по именам
        heart1 = FindChildImage("Heart1");
        heart2 = FindChildImage("Heart2");
        heart3 = FindChildImage("Heart3");

        InvokeRepeating("UpdateHearts", 0f, 0.1f);
    }

    private Image FindChildImage(string childName)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {
            return child.GetComponent<Image>();
        }
        return null;
    }

    private void UpdateHearts()
    {
        if (Hero.Instance == null) return;

        int health = Hero.Instance.GetCurrentHealth();

        if (heart1 != null) heart1.sprite = health >= 1 ? fullHeart : emptyHeart;
        if (heart2 != null) heart2.sprite = health >= 2 ? fullHeart : emptyHeart;
        if (heart3 != null) heart3.sprite = health >= 3 ? fullHeart : emptyHeart;
    }

    private void OnDestroy()
    {
        CancelInvoke("UpdateHearts");
    }
}