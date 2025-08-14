using UnityEngine;
using System.Linq;

public enum WeaponFireMode
{
    Single,
    Burst,
    Auto,
    Charge,
}

class FiringWeapon : Weapon
{
    FiringWeaponAsset _asset;

    [SerializeField]
    GameObject _muzzleGo;

    GameObject _bulletPrefab;

    WeaponFireMode _fireMode;
    WeaponFireMode[] _availableFireModes;

    // Fire interval in seconds
    float _fireInterval = .2f;

    // The force to fire the bullet with.
    // Larger force means faster bullet and longer range.
    float _fireForce;

    // The time when the last shot was fired.
    float _lastShootTime = 0;

    // Is bullet clocked in the chamber.
    WeaponProjectile[] _projectiles;

    // Is the trigger pressed
    bool _isTriggerDown = false;

    // Number of shots fired since the last trigger press.
    // This is reset once the trigger is up.
    uint _fireCount = 0;

    // Number of shots to fire in burst mode
    uint _burstFireCount = 3;

    uint _fireCountAtOnce = 3;

    // The mag insert into the gun.
    WeaponAmmo _mag;

    AudioClip _fireAudio;

    bool _wasTriggerDown = false;

    bool _needsClocking = true;

    public FiringWeaponAsset WeaponAsset => _asset;

    public void Init(FiringWeaponAsset asset)
    {
        Debug.Assert(asset != null, "Firing weapon asset is null.");
        _asset = asset;

        _bulletPrefab = _asset.BulletPrefab;
        _fireMode = _asset.InitialFireMode;
        _availableFireModes = _asset.AvailableFireModes;
        _fireInterval = 1f / _asset.FireRate;
        _fireForce = _asset.FireForce;
        _burstFireCount = _asset.BurstFireCount;
        _fireAudio = _asset.FireAudio;

        Debug.Assert(_bulletPrefab != null, "Bullet prefab is null.");
        Debug.Assert(_muzzleGo != null, "Muzzle game object is null.");
        Debug.Assert(_fireAudio != null, "Fire audio clip is null.");

        _fireCountAtOnce = 1;
    }

    void Awake()
    {
        // Initialize if not already initialized.
        if (_asset == null)
        {
            var initializer = GetComponent<WeaponInitializer>();
            var asset = initializer.weaponAsset as FiringWeaponAsset;

            Init(asset);

            // Consume the initializer
            Destroy(initializer);
        }
    }

    void Update()
    {
        if (CanFire())
            _Fire();

        if (!_wasTriggerDown && _isTriggerDown)
        {
            _wasTriggerDown = true;
            OnTriggerDown();
        }
        else if (_wasTriggerDown && !_isTriggerDown)
        {
            _wasTriggerDown = false;
            OnTriggerUp();
        }

        // We do this so the gun controller has to keep pressing the trigger,
        // the alternative event system of OnDown() and OnUp() introduces the risk
        // of not calling the OnUp() event and dropping the weapon.
        _isTriggerDown = false;
    }

    void OnTriggerDown()
    {
    }

    void OnTriggerUp()
    {
        _fireCount = 0;
    }

    public void ReleaseCharge()
    {
    }

    public void OnScopeIn()
    {
    }

    public void OnScopeOut()
    {
    }

    public virtual void PressTrigger()
    {
        _isTriggerDown = true;
    }

    public bool HasMag()
    {
        return _mag != null;
    }

    public void OnProjectileIn(WeaponProjectile projectile)
    {
    }

    public void OnMagIn(WeaponAmmo mag)
    {
        Debug.Assert(mag != null);

        _mag = mag;
    }

    public void OnMagOut()
    {
        _mag = null;
    }

    public bool IsClocked()
    {
        return !_needsClocking || _projectiles[0] != null;
    }

    public void OnClock()
    {
        _isClocked = true;
    }

    public void SwitchMode(WeaponFireMode mode)
    {
        if (_fireMode == mode)
            return;

        if (!_availableFireModes.Contains(mode))
        {
            Debug.LogWarning($"Cannot switch to {mode} mode, not available.");
            return;
        }

        switch (mode)
        {
            case WeaponFireMode.Single:
                _fireCountAtOnce = 1;
                break;

            case WeaponFireMode.Burst:
                _fireCountAtOnce = _burstFireCount;
                break;

            case WeaponFireMode.Auto:
                _fireCountAtOnce = _mag.capacity;
                break;
        }

        _fireMode = mode;
    }

    public bool CanFire()
    {
        // Checks the fire interval
        if (_lastShootTime + _fireInterval >= Time.time)
            return false;

        if (!_projectile)
            return false;

        if (_fireCount >= _fireCountAtOnce)
            return false;

        if (!_isTriggerDown)
            return false;

        return true;
    }

    void _Fire()
    {
        Debug.Log("Firing...");

        // Record the last fire time to handle fire rate.
        _lastShootTime = Time.time;

        if (_mag != null)
        {
            if (_mag.bulletCount == 0)
                _projectile = false;
            else _mag.bulletCount -= 1;
        }
        else
        {
            _projectile = false;
        }

        _fireCount += 1;

        // Consume the projectile
        _projectiles[0] = null;

        var bulletGo = Instantiate(_bulletPrefab);
        var bullet = bulletGo.GetComponent<WeaponProjectile>();
        bullet.transform.position = _muzzleGo.transform.position;
        bullet.transform.rotation = _muzzleGo.transform.rotation;

        bullet.Launch(Owner, _muzzleGo.transform.forward, _fireForce);
    }
}
