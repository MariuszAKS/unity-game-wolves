using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_Controller : MonoBehaviour
{
    [SerializeField] LayerMask layerMask_impassable;
    [SerializeField] int speed;

    BoxCollider2D boxCollider2D;
    
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Vector3 velocity = new Vector3(transform.localScale.x * speed * Time.deltaTime, 0, 0);

        Bounds bounds = boxCollider2D.bounds;
        bounds.Expand(-0.02f);

        RaycastHit2D hit;
        Vector2 origin = new Vector2((transform.localScale.x == 1 ? bounds.max.x : bounds.min.x), bounds.min.y);
        Vector2 direction = new Vector2(transform.localScale.x, 0);
        float distance = Mathf.Abs(velocity.x) + 0.01f;

        for (int i = 0; i < 3; i++) {
            hit = Physics2D.Raycast(origin, direction, distance, layerMask_impassable);
            Debug.DrawRay(origin, direction * distance, Color.red);

            if (hit) {
                velocity.x = (hit.distance - 0.01f) * Mathf.Sign(velocity.x);
            }

            origin += Vector2.up * (bounds.size.y / 2);
        }

        transform.position += velocity;
        
        if (Mathf.Abs(velocity.x) < 0.001) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
