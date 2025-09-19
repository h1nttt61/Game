using UnityEngine;
using System;

public class DestructiblePlatns : MonoBehaviour
{
    public event EventHandler OnDestructibleTakeDemage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Sword>())
        {
            OnDestructibleTakeDemage?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
            NavMeshSurfaceManagment.Instance.RebakeNavMeshSurface();
        }
    }
}
