using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

[RequireComponent (typeof(PolygonCollider2D))] // "хороший код"
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    //[SerializeField] private int _maxHealth; не нужно из-за Scriptable Objects

    private int _currentHealth;
    
    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        _currentHealth = _enemySO.enemyHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Hero hero))
        {
            hero.TakeDamage(transform, _enemySO.enemyDamageAmount);
        }
    }

    public void PolugonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    public void PolugonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnTakeHit.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;

            _enemyAI.SetDeathState();

            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    
}
