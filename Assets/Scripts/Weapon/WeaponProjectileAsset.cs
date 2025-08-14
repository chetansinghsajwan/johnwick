using UnityEngine;

class WeaponProjectileAsset : ScriptableObject
{
    [SerializeField]
    string _name;

    [SerializeField]
    WeaponProjectileType _projectileType;

    [SerializeField]
    uint _mass;

    [SerializeField]
    uint _penetrationPower;

    public string Name => _name;
    public uint Mass => _mass;
    public WeaponProjectileType ProjectileType => _projectileType;
    public uint PenetrationPower => _penetrationPower;
}
