using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Player_Movement : MonoBehaviour
{
    [SerializeField] LayerMask layerMask_impassable;

    BoxCollider2D boxCollider2D;
    Bounds bounds;
    Vector2 bounds_topLeft, bounds_topRight, bounds_bottomLeft, bounds_bottomRight;
    
    const float bounds_skin_width = 0.01f;
    const int rays_amount_per_face = 3;

    private void Start() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Move(Vector3 velocity) {
        velocity *= Time.deltaTime;

        UpdateBounds();

        if (velocity.x != 0) {
            RaycastHorizontal(ref velocity);
            transform.position += new Vector3(velocity.x, 0, 0);
        }

        UpdateBounds();

        if (velocity.y != 0) {
            RaycastVertical(ref velocity);
            transform.position += new Vector3(0, velocity.y, 0);
        }
    }

    void UpdateBounds() {
        bounds = boxCollider2D.bounds;

        float bounds_expand_direction = -1f;
        float bounds_skin_layers_in_axis = 2f;
        float bounds_expand_multiplier = bounds_skin_width * bounds_expand_direction * bounds_skin_layers_in_axis;
        bounds.Expand(bounds_expand_multiplier);

        bounds_topLeft = new Vector2(bounds.min.x, bounds.max.y);
        bounds_topRight = new Vector2(bounds.max.x, bounds.max.y);
        bounds_bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        bounds_bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    }

    void RaycastHorizontal(ref Vector3 velocity) {
        float space_between_rays = bounds.size.x / (rays_amount_per_face - 1);

        for (int i = 0; i < rays_amount_per_face; i++) {
            Vector2 origin = (Mathf.Sign(velocity.x) == 1 ? bounds_bottomRight : bounds_bottomLeft) + i * space_between_rays * Vector2.up;
            Vector2 direction = Vector2.right * Mathf.Sign(velocity.x);
            float distance = Mathf.Abs(velocity.x) + bounds_skin_width;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask_impassable);
            Debug.DrawRay(origin, direction * distance, Color.red);

            if (hit) {
                velocity.x = (hit.distance - bounds_skin_width) * Mathf.Sign(velocity.x);
            }
        }
    }

    void RaycastVertical(ref Vector3 velocity) {
        float space_between_rays = bounds.size.y / (rays_amount_per_face - 1);

        for (int i = 0; i < rays_amount_per_face; i++) {
            Vector2 origin = (Mathf.Sign(velocity.y) == 1 ? bounds_topLeft : bounds_bottomLeft) + i * space_between_rays * Vector2.right;
            Vector2 direction = Vector2.up * Mathf.Sign(velocity.y);
            float distance = Mathf.Abs(velocity.y) + bounds_skin_width;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask_impassable);
            Debug.DrawRay(origin, direction * distance, Color.red);

            if (hit) {
                velocity.y = (hit.distance - bounds_skin_width) * Mathf.Sign(velocity.y);
            }
        }
    }
}
