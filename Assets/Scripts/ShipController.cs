using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    None,
    Basic,
    Triple,
    Quin,
}

public class ShipController : MonoBehaviour
{
	[Header("Stats")]
    public float maxHealth;
    protected float currentHealth;
    public float maxShield;
    protected float currentShield;
    public float shieldRechargeRate = 0.1f;
    public float shieldRechargeDelay = 0.1f;
    protected bool takenDamage = false;
    // Attack parameters
    public AttackType CurrentAttackType = AttackType.Basic;
    public float shotReloadTime = 0.1f;

	[Header("Turret Objects")]
    public GameObject BasicTurret;
    public bool bShotActive = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    protected virtual void Death()
    {
        gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
    }

    virtual public IEnumerator Shoot()
    {
        while (bShotActive)
        {
            TrySpawnBullet(BasicTurret);

            yield return new WaitForSeconds(shotReloadTime);
        }
    }

    // Spawns a bullet at the referenced gameobjects transform, tries to dip into object pool
    virtual public bool TrySpawnBullet(GameObject gameObject)
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = gameObject.transform.position;
            bullet.transform.rotation = gameObject.transform.rotation;
            bullet.SetActive(true);
            

            return true;
        }
        else
        {
            ObjectPooler.SharedInstance.AddToPool();
            TrySpawnBullet(gameObject);
        }
        return false;
    }
}
