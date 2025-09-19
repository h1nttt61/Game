using NUnit.Framework.Internal.Filters;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[SelectionBase]//для удобного выделение
public class Hero : MonoBehaviour
{
    // коррутины позволяют выполнять отложенный код(не многопоточка)
    public static Hero Instance { get; private set; }

    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;
    //private чтобы не каждый метод имел к нему доступ, без public меньше и легче связывать
    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    [SerializeField] private float speed = 5f; //директива позволяет появится настройке в unity , но не меняется при вводе в unity
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float damageRecoveryTime = 0.5f;
    [Header("Dash settings")]
    [SerializeField] private int dashSpeed = 4;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float dashCooldown = 0.5f;


    Vector2 _inputVector;

    //Состояние покоя
    private readonly float _minSpeed = 0.1f;
    private bool _isRunning = false;

    private int _currentHealt;
    private bool _canTakeDamage;
    private bool _isAlive;
    private bool _isDashing;
    private float _initialSpeed;

    private Camera _camera;

    /// <summary>
    /// встроенная функция в Unity , запускается до Start
    /// </summary>
    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
        _camera = Camera.main;
        _initialSpeed = speed;
    }

    private void Start()
    {
        _currentHealt = maxHealth;
        _canTakeDamage = true;
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;//Если написать в Awake может быть ошибка
        GameInput.Instance.OnPlayerDash += OnPlayerDashh;
        _isAlive = true;
    }

    private void OnPlayerDashh(object sender, EventArgs e)
    {
        Dash();
    }

    private void Dash()
    {
        if (!_isDashing)
            StartCoroutine(DashRutine());
    }

    private IEnumerator DashRutine()
    {
        _isDashing = true;
        speed *= dashSpeed;
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        speed = _initialSpeed;
        yield return new WaitForSeconds(dashCooldown);
        _isDashing = false;
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= Player_OnPlayerAttack;
    }
    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void Update()
    {
        _inputVector = GameInput.Instance.GetMovementVector();
    }

    /// <summary>
    /// В отличие от Update вызывается не каждый кадр (вызывается каждые 0.02 )
    /// </summary>
    private void FixedUpdate()
    {
        if (_knockBack.IsGettingBack)
            return;
        HandleMovement();
    }

    public bool IsAlive() => _isAlive;

    public bool IsRunning() => _isRunning;

    public Vector3 GetScreenPlayerPosition()
    {
        Vector3 playerScreenPos = _camera.WorldToScreenPoint(transform.position);
        return playerScreenPos;
    }
    public void TakeDamage(Transform damageTransform, int damage)
    {
        if (_canTakeDamage && _isAlive )
        {
            _canTakeDamage = false;
            _currentHealt = Mathf.Max(0, _currentHealt -= damage);
            _knockBack.GetKnockedBack(damageTransform);

            OnFlashBlink?.Invoke(this, EventArgs.Empty);

            StartCoroutine(DamageRecoveryRoutine());
        }

        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealt == 0 && _isAlive)
        {
            _isAlive = false;

            _knockBack.StopKnockBackMovement();

            GameInput.Instance.DisableMovement();

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);

        }
    }

    /// <summary>
    /// Корутин
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }

  
    private void HandleMovement()
    {


        //inputVector = inputVector.normalized;

        _rb.MovePosition(_rb.position + _inputVector * Time.fixedDeltaTime * speed); //фиксим скорость благодаря 0.02

        if (Mathf.Abs(_inputVector.x) > _minSpeed || Mathf.Abs(_inputVector.y) > _minSpeed)
            _isRunning = true;
        else _isRunning = false;

    }
}
/* Vector2 inputVector = new Vector2(0, 0); //сохранение позиции игрока
      if (Input.GetKey(KeyCode.W)) 
      {
          inputVector.y = 1f;
      }
      if (Input.GetKey(KeyCode.S))
      {
          inputVector.y = -1f;
      }
      if (Input.GetKey(KeyCode.A))
      {
          inputVector.x = -1f;
      }
      if (Input.GetKey(KeyCode.D))
      {
          inputVector.x = 1f;
      }

     2*/