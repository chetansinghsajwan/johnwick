using UnityEngine;

abstract class WeaponAsset : ScriptableObject
{
    [SerializeField]
    string _weaponName;

    [SerializeField]
    GameObject _prefab;

    [SerializeField]
    WeaponCategory _category;

    public string WeaponName => _weaponName;
    public GameObject prefab => _prefab;
    public WeaponCategory category => _category;
}
