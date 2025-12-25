using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] private float speed = 150f;           // units per second
    [SerializeField] private float maxDistance = 500f;     // max range before destroy

    private Vector3 _direction;
    private float _distanceTraveled;

    public void Fire(Vector3 startPos, Vector3 direction)
    {
        transform.position = startPos;
        _direction = direction.normalized;
        _distanceTraveled = 0f;

        // Orient bullet toward direction (optional, mostly for looks)
        if (_direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_direction);
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position += _direction * step;
        _distanceTraveled += step;

        if (_distanceTraveled >= maxDistance)
            Destroy(gameObject);
    }

    public Vector3 GetDirection() => _direction;
}
