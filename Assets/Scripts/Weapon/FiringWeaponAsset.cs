using UnityEngine;

[CreateAssetMenu(fileName = "Firing Weapon Asset", menuName = "Weapon/Firing Weapon Asset", order = 1)]
class FiringWeaponAsset : WeaponAsset
{
    [SerializeField]
    WeaponFireMode _initialFireMode;

    [SerializeField]
    WeaponFireMode[] _availableFireModes;

    [SerializeField]
    AudioClip _fireAudio;

    [SerializeField]
    GameObject _bulletPrefab;

    [SerializeField]
    WeaponFireMode _fireMode;

    [SerializeField]
    uint _fireRate;

    [SerializeField]
    uint _fireForce;

    [SerializeField]
    uint _burstFireCount;

    public WeaponFireMode InitialFireMode => _initialFireMode;
    public WeaponFireMode[] AvailableFireModes => _availableFireModes;
    public AudioClip FireAudio => _fireAudio;
    public GameObject BulletPrefab => _bulletPrefab;
    public WeaponFireMode FireMode => _fireMode;
    public uint FireRate => _fireRate;
    public uint FireForce => _fireForce;
    public uint BurstFireCount => _burstFireCount;
}
