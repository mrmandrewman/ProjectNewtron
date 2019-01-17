using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState SharedInstance;
    

    public float maxHealth = 10;
    private float currentHealth;
    public float maxShield = 5;
    private float currentShield;

    public GameObject player;
    public int attackPowerUpsCollected;
    public int attackSpeedPowerUpsCollected;
    public int attackPowerLevel = 1;
    public AttackType attackType = AttackType.Basic;

    private void Awake()
    {
        if (SharedInstance == null)
        {
            DontDestroyOnLoad(gameObject);
            SharedInstance = this;
        }
        else if (SharedInstance != this)
        {
            Destroy(gameObject);
        }
    }

}
