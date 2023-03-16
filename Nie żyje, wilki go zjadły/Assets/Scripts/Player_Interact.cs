using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact : MonoBehaviour
{
    [SerializeField] LayerMask layerMask_collectibles;

    BoxCollider2D boxCollider2D;

    private void Start() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void CheckForInteractable(char compassDirection) {
        Bounds bounds = boxCollider2D.bounds;

        float bounds_expand_direction = -1f;
        float bounds_skin_layers_in_axis = 2f;
        float bounds_expand_multiplier = 0.01f * bounds_expand_direction * bounds_skin_layers_in_axis;
        bounds.Expand(bounds_expand_multiplier);

        Vector2 bounds_topLeft = new Vector2(bounds.min.x, bounds.max.y);
        Vector2 bounds_topRight = new Vector2(bounds.max.x, bounds.max.y);
        Vector2 bounds_bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 bounds_bottomRight = new Vector2(bounds.max.x, bounds.min.y);

        Vector2 ray_origin, ray_direction, nextRay_direction;
        float space_between_rays;

        int rays_amount_per_face = 3;

        switch (compassDirection) {
            case 'N':
                ray_origin = bounds_topLeft;
                ray_direction = Vector2.up;
                nextRay_direction = Vector2.right;
                space_between_rays = bounds.size.x / (rays_amount_per_face - 1);
                break;
            case 'S':
                ray_origin = bounds_bottomLeft;
                ray_direction = Vector2.down;
                nextRay_direction = Vector2.right;
                space_between_rays = bounds.size.x / (rays_amount_per_face - 1);
                break;
            case 'E':
                ray_origin = bounds_bottomRight;
                ray_direction = Vector2.right;
                nextRay_direction = Vector2.up;
                space_between_rays = bounds.size.y / (rays_amount_per_face - 1);
                break;
            default: //case 'W':
                ray_origin = bounds_bottomLeft;
                ray_direction = Vector2.left;
                nextRay_direction = Vector2.up;
                space_between_rays = bounds.size.y / (rays_amount_per_face - 1);
                break;
        }

        for (int ray = 0; ray < rays_amount_per_face; ray++) {
            RaycastHit2D hit = Physics2D.Raycast(ray_origin, ray_direction, 0.5f, layerMask_collectibles);
            Debug.DrawRay(ray_origin, ray_direction * 0.5f, Color.blue);

            ray_origin += nextRay_direction * space_between_rays;

            if (hit) {
                Debug.Log(hit.collider.name);
                Destroy(hit.collider.gameObject);
                break;
            }
        }
    }
}