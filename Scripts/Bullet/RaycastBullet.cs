using UnityEngine;

public class RaycastBullet : MonoBehaviour
{
    [SerializeField] private float speed = 150f;
    [SerializeField] private float maxDistance = 500f;
    [SerializeField] private float lifetime = 3f;

    private Vector3 _direction;
    private bool _hasHit;

    public void Fire(Vector3 startPos, Vector3 direction, float damage)
    {
        transform.position = startPos;
        _direction = direction.normalized;
        _hasHit = false;

        // Raycast immediately to detect hit
        Ray ray = new Ray(startPos, _direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.Log("Bullet hit: " + hit.collider.name);

            ZombieHealth zh = hit.collider.GetComponentInParent<ZombieHealth>();
            if (zh != null)
            {
                float finalDamage = damage;

                // Headshot check
                if (hit.collider.CompareTag("Head"))
                {
                    finalDamage = damage * 3f;
                    Debug.Log("HEADSHOT!");
                }

                zh.TakeDamage(finalDamage);
            }

            _hasHit = true;
        }

        // Auto-destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move bullet forward
        transform.position += _direction * speed * Time.deltaTime;
    }
}
