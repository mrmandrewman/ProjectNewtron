using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    AttackPowerUp,
    AttackSpeedPowerUp
}

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;

    private void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * 3;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(GetComponent<Rigidbody2D>().velocity, new Vector2(0, -3), 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.SendMessage("PowerUp", powerUpType);
            Destroy(gameObject);
        }
    }
}
