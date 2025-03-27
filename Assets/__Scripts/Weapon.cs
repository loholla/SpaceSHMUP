using UnityEngine;

/// <summary>
/// This is an enum of the various possible weapon types.
/// It also includes a "shield" type to allow a shield PowerUp.
/// Items marked [NI] below are Not Implemented
/// </summary>
public enum eWeaponType {
    none,
    blaster,
    spread,
    phaser, //[NI]
    missile, //[NI]
    laser, //[NI]
    shieldLevel
}

/// <summary>
/// The WeaponDefinition class allows you to set the properties of a specific weapon in the Inspector.
/// The Main class has an array of WeaponDefinitions that makes this possible.
/// </summary>
[System.Serializable]
public class WeaponDefinition {
    public eWeaponType type = eWeaponType.none;
    [Tooltip("Letter to show on the PowerUp Cube")]
    public string       letter;
    [Tooltip("Color of PowerUp Cube")]
    public Color        powerUpColor = Color.white;
    [Tooltip("Prefab of Weapon model that is attached to the Player Ship")]
    public GameObject   weaponModelPrefab;
    [Tooltip("Prefab of projectile that is fired")]
    public GameObject   projectilePrefab;
    [Tooltip("Color of the Projectile that is fired")]
    public Color        projectileColor = Color.white;
    [Tooltip("Damage caused when a single Projectile hits an Enemy")]
    public float        damageOnHit = 0;
    [Tooltip("Damage caused per second by the Laser [NI]")]
    public float        damagePerSecond = 0;
    [Tooltip("Seconds to delay between shots")]
    public float        delayBetweenShots = 0;
    [Tooltip("Velocity of individual Projectiles")]
    public float        velocity = 50;
}

public class Weapon : MonoBehaviour
{

}