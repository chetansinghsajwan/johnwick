using UnityEngine;

public enum WeaponCategory
{
    Meele,
    Rifle,
    Pistol,
    Misc,
}

abstract class Weapon : MonoBehaviour
{
    Controller _owner;

    public Controller Owner => _owner;

    public virtual void OnEquip(Controller owner)
    {
        _owner = owner;
    }

    public virtual void OnUnequip() { }
}
