using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private FlashBlink _flashBlink;

    private const string IS_RUNNING = "IsRunning";

    private const string IS_DIE = "IsDie";

    private void Start()
    {
        Hero.Instance.OnPlayerDeath += Instance_OnPlayerDeath;
    }

    private void OnDestroy()
    {
        Hero.Instance.OnPlayerDeath -= Instance_OnPlayerDeath;
    }

    private void Instance_OnPlayerDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
        _flashBlink.StopBlinkig();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _flashBlink = GetComponent<FlashBlink>(); 
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, Hero.Instance.IsRunning());
        if (Hero.Instance.IsAlive())
            AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Hero.Instance.GetScreenPlayerPosition();

        if (mousePos.x < playerPosition.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

    }
}
