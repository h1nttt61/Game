using UnityEngine;
using UnityEngine.SceneManagement;
public class CursorManager : MonoBehaviour
{
    [Header("Scenes name")]
    [SerializeField] private string gameScenceName = "Game";
    [SerializeField] private string menuScenceName = "Menu";

    [Header("Cursor Settings")]
    [SerializeField] private bool hideCursorInGame = true;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnScenceChanged;

        UpdateCursorForCurrentScence();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnScenceChanged;
    }

    private void OnScenceChanged(Scene scene, Scene newScene)
    {
        UpdateCursorForCurrentScence();
    }

    private void UpdateCursorForCurrentScence()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == gameScenceName)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
