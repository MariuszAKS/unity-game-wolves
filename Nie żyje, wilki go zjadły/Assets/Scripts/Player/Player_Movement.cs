using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]

public class Player_Movement : MonoBehaviour
{
    [SerializeField] LayerMask layerMask_impassable;
    [SerializeField] LayerMask layerMask_passage;

    const float bounds_skin_width = 0.01f;
    const int rays_amount_per_face = 3;

    BoxCollider2D boxCollider2D;
    Bounds bounds;
    Vector2 bounds_topLeft, bounds_topRight, bounds_bottomLeft, bounds_bottomRight;
    
    bool passage_hit;
    string passage_name;
    
    private void Start() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Move(Vector2 direction, float speed) {
        if (direction.x == 0 && direction.y == 0)
            return;

        Vector3 velocity = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        passage_hit = false;
        passage_name = "";

        if (velocity.x != 0) {
            UpdateBounds();
            RaycastHorizontal(ref velocity);
            transform.position += new Vector3(velocity.x, 0, 0);
        }

        if (velocity.y != 0) {
            UpdateBounds();
            RaycastVertical(ref velocity);
            transform.position += new Vector3(0, velocity.y, 0);
        }

        if (passage_hit) {
            if (passage_name == "Military_Building") {
                SceneManager.LoadScene(2);
                return;
            }

            if (passage_name == "Outside") {
                SceneManager.LoadScene(1);
                return;
            }
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
        float distance = Mathf.Abs(velocity.x) + bounds_skin_width;

        for (int i = 0; i < rays_amount_per_face; i++) {
            Vector2 origin = (Mathf.Sign(velocity.x) == 1 ? bounds_bottomRight : bounds_bottomLeft) + i * space_between_rays * Vector2.up;
            Vector2 direction = Vector2.right * Mathf.Sign(velocity.x);

            RaycastHit2D hit_impassable = Physics2D.Raycast(origin, direction, distance, layerMask_impassable);
            RaycastHit2D hit_passage = Physics2D.Raycast(origin, direction, distance, layerMask_passage);
            Debug.DrawRay(origin, direction * distance, Color.red);

            if (hit_impassable) {
                passage_hit = false;
                velocity.x = (hit_impassable.distance - bounds_skin_width) * Mathf.Sign(velocity.x);
                distance = hit_impassable.distance;
            }

            if (hit_passage) {
                passage_hit = true;
                passage_name = hit_passage.collider.name;
            }
        }
    }

    void RaycastVertical(ref Vector3 velocity) {
        float space_between_rays = bounds.size.y / (rays_amount_per_face - 1);
        float distance = Mathf.Abs(velocity.y) + bounds_skin_width;

        for (int i = 0; i < rays_amount_per_face; i++) {
            Vector2 origin = (Mathf.Sign(velocity.y) == 1 ? bounds_topLeft : bounds_bottomLeft) + i * space_between_rays * Vector2.right;
            Vector2 direction = Vector2.up * Mathf.Sign(velocity.y);

            RaycastHit2D hit_impassable = Physics2D.Raycast(origin, direction, distance, layerMask_impassable);
            RaycastHit2D hit_passage = Physics2D.Raycast(origin, direction, distance, layerMask_passage);
            Debug.DrawRay(origin, direction * distance, Color.red);

            if (hit_impassable) {
                passage_hit = false;
                velocity.y = (hit_impassable.distance - bounds_skin_width) * Mathf.Sign(velocity.y);
                distance = hit_impassable.distance;
            }

            if (hit_passage) {
                passage_hit = true;
                passage_name = hit_passage.collider.name;
            }
        }
    }
}
