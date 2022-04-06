using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballController : MonoBehaviour
{
    public float _speed;
    Rigidbody2D _baseballRigidBody;
    public float _damage;

    private void Awake()
    {
        _baseballRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _baseballRigidBody.AddForce(transform.up * _speed);
        //Invoke("Disable", 4f);
    }

    void Disable()
    {
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collison)
    {
        if(collison.gameObject.CompareTag("Player"))
        {
            collison.GetComponent<PlayerController>().Damage(_damage);
            Invoke("Disable", 0.001f);
        }

        if(collison.gameObject.CompareTag("Wall"))
        {
            Invoke("Disable", 0.001f);
        }
    }
    /*private void OnDisable()
    {
        CancelInvoke();
    }*/
}
