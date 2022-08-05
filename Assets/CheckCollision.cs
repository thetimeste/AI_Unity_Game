using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 pos = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
        gameObject.transform.position = pos;
    }
}
