using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScripts : MonoBehaviour
{
    /// <summary>
    /// The muzzle is the end of the weapon, from where the projectile will be shot
    /// </summary>
    public Transform muzzle;

    /// <summary>
    /// The amount of damage the projectile deals to the target
    /// </summary>
    public float gunDamage;

    /// <summary>
    /// How fast the weapon shoots projectiles
    /// </summary>
    private int _fireRate;

    /// <summary>
    /// The entity the script wants to shoot at
    /// </summary>
    private GameObject _target;

    /// <summary>
    /// The prefab of the bullet that will be spawned when the weapon is fired.
    /// </summary>
    public GameObject ProjectilePrefab;



}