using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D _playerRigidbody;
    private float _moveHorizontal;
    private float _moveVertical;
    public float health = 100f;
    public float maxHealth = 100f;

    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (health <= 0)
        {
            Debug.Log("PLAYER DIED");
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        _moveHorizontal = Input.GetAxis("Horizontal");
        _moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(_moveHorizontal, _moveVertical);
        _playerRigidbody.AddForce(movement * speed);
    }
}





