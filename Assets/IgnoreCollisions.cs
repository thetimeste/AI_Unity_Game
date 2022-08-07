using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<CapsuleCollider>());
    }
}
