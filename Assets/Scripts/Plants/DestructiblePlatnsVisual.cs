using UnityEngine;

public class DestructiblePlatnsVisual : MonoBehaviour
{
    [SerializeField] private DestructiblePlatns destructiblePlatns;
    [SerializeField] private GameObject bushDeathVFXPrefab;

    private void Start()
    {
        destructiblePlatns.OnDestructibleTakeDemage += _destructiblePlatns_OnDestructibleTakeDemage;
    }

    private void _destructiblePlatns_OnDestructibleTakeDemage(object sender, System.EventArgs e)
    {
        ShowDeathVFX();
    }

    private void ShowDeathVFX()
    {
        Instantiate(bushDeathVFXPrefab, destructiblePlatns.transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        destructiblePlatns.OnDestructibleTakeDemage -= _destructiblePlatns_OnDestructibleTakeDemage;
    }
}
