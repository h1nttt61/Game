using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour damagbleObject;
    [SerializeField] private Material blinkMaterial;
    [SerializeField] private float blinkDuration = 0.2f;

    private float _blinkTimer;
    private Material _defaultMaterial;
    private SpriteRenderer _spriteRenderer;
    private bool _isBlinking;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;

        _isBlinking = true;

    }

    private void Start()
    {
        if (damagbleObject is Hero hero)
        {
            hero.OnFlashBlink += FlashBlink_OnFlashBlink;
        }
    }

    private void FlashBlink_OnFlashBlink(object sender, System.EventArgs e)
    {
        SetBlinkingMaterial();
    }

    private void Update()
    {
        if (_isBlinking)
        {
            _blinkTimer -= Time.deltaTime;
            if (_blinkTimer < 0)
            {
                SetDefaultMaterial();
            }
        }
    }

    private void SetBlinkingMaterial()
    {
        _blinkTimer = blinkDuration;
        _spriteRenderer.material = blinkMaterial;
    }

    private void SetDefaultMaterial()
    {
        _spriteRenderer.material = _defaultMaterial;
    }

    public void StopBlinkig()
    {
        SetDefaultMaterial();
        _isBlinking = false;
    }

    private void OnDestroy()
    {
        if (damagbleObject is Hero hero)
        {
            hero.OnFlashBlink -= FlashBlink_OnFlashBlink;
        }
    }
}
