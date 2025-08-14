using UnityEngine;

enum WeaponProjectileType
{
    Bullet,
    Grenade,
    Rocket,
}

class WeaponProjectile : MonoBehaviour
{
    WeaponProjectileAsset _asset;
    Controller _owner;
    Rigidbody _rb;

    public WeaponProjectileAsset Asset => _asset;
    public Controller Owner => _owner;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Launch(Controller owner, Vector3 direction, float force)
    {
        _owner = owner;

        GameObject.Destroy(this, 4);
        _rb.AddForce(direction * force, ForceMode.Impulse);
    }
}
