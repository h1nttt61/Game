using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }

    [SerializeField] private Sword sword;
    [SerializeField] private SpriteRenderer swordSpriteRenderer;

    private void Awake()
    {
        Instance = this;  
        
    }

    private void Update()
    {
        if (Hero.Instance.IsAlive())
            FollowMousePosition();
    }

    public Sword GetActiveWeapon()
    { 
        return sword; 
    }

    private void FollowMousePosition()
    {
        //If you need a flip from the cursor
        /*Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Hero.Instance.GetScreenPlayerPosition();

        if (mousePos.x < playerPosition.x)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);*/
        Vector2 movement = GameInput.Instance.GetMovementVector();

        // Поворачиваем меч в сторону движения по X
        if (movement.x < -0.1f) // Движение влево (A)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            if (swordSpriteRenderer != null) swordSpriteRenderer.flipY = true; // Дополнительный flip если нужно
        }
        else if (movement.x > 0.1f) // Движение вправо (D)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            if (swordSpriteRenderer != null) swordSpriteRenderer.flipY = false;
        }
    }
}
