using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    public SpriteRenderer _parentRenderer;
    public float _attack;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(_parentRenderer.flipX? -1 : 1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collison)
    {
        if(collison.gameObject.CompareTag("Player"))
        {
            collison.gameObject.GetComponent<PlayerController>().Damage(_attack);
        }
    }

    private void OnTriggerStay2D(Collider2D collison)
    {
        if(collison.gameObject.CompareTag("Player"))
        {
            collison.gameObject.GetComponent<PlayerController>().Damage(_attack);
        }
    }
}
