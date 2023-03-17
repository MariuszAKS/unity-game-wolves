using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    const byte rays_per_side = 5;

    [SerializeField] LayerMask layerMask_collectibles;

    BoxCollider2D boxCollider2D;
    Bounds bounds;
    Vector2 bounds_topLeft, bounds_topRight, bounds_bottomLeft, bounds_bottomRight, bounds_center;

    bool collected_leftArm, collected_rightArm;
    bool collected_leftLeg, collected_rightLeg;
    bool collected_torso;

    public bool LeftArm {get { return collected_leftArm; } }
    public bool RightArm {get { return collected_rightArm; } }
    public bool LeftLeg {get { return collected_leftLeg; } }
    public bool RightLeg {get { return collected_rightLeg; } }
    public bool Torso {get { return collected_torso; } }

    private void Start() {
        boxCollider2D = GetComponent<BoxCollider2D>();

        collected_leftArm = collected_rightArm = false;
        collected_leftLeg = collected_rightLeg = false;
        collected_torso = false;
    }

    public void Interact(Direction facingDirection) {
        GetBounds();

        Vector2 origin, direction, next_ray_direction;
        float length, space_between_rays;

        switch (facingDirection) {
            case Direction.Up:
                origin = bounds_topLeft;
                direction = Vector2.up;
                next_ray_direction = Vector2.right;
                length = bounds.size.y + 0.5f;
                space_between_rays = bounds.size.x / (rays_per_side - 1);
                break;
            case Direction.Right:
                origin = bounds_topRight;
                direction = Vector2.right;
                next_ray_direction = Vector2.down;
                length = bounds.size.x + 0.5f;
                space_between_rays = bounds.size.y / (rays_per_side - 1);
                break;
            case Direction.Down:
                origin = bounds_bottomRight;
                direction = Vector2.down;
                next_ray_direction = Vector2.left;
                length = bounds.size.y + 0.5f;
                space_between_rays = bounds.size.x / (rays_per_side - 1);
                break;
            case Direction.Left:
                origin = bounds_bottomLeft;
                direction = Vector2.left;
                next_ray_direction = Vector2.up;
                length = bounds.size.x + 0.5f;
                space_between_rays = bounds.size.y / (rays_per_side - 1);
                break;
            default:
                origin = bounds_center;
                direction = Vector2.zero;
                next_ray_direction = Vector2.zero;
                length = 0f;
                space_between_rays = 0f;
                break;
        }

        RaycastHit2D hit;

        for (int ray = 0; ray < rays_per_side; ray++) {
            hit = Physics2D.Raycast(origin, direction, length, layerMask_collectibles);
            Debug.DrawRay(origin, direction * length, Color.blue);

            origin += next_ray_direction * space_between_rays;

            if (hit) {
                string hitColliderName = hit.collider.name;

                Destroy(hit.collider.gameObject);

                switch(hitColliderName) {
                    case "Left_Arm": collected_leftArm = true; break;
                    case "Right_Arm": collected_rightArm = true; break;
                    case "Left_Leg": collected_leftLeg = true; break;
                    case "Right_Leg": collected_rightLeg = true; break;
                    case "Torso": collected_torso = true; break;
                }

                if (collected_leftArm && collected_rightArm && collected_leftLeg && collected_rightLeg && collected_torso) {
                    Debug.Log("Winning condition fulfilled!");
                }

                break;
            }
        }
    }

    void GetBounds() {
        bounds = boxCollider2D.bounds;

        bounds_topLeft = new Vector2(bounds.min.x, bounds.max.y);
        bounds_topRight = new Vector2(bounds.max.x, bounds.max.y);
        bounds_bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        bounds_bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        bounds_center = new Vector2(bounds.min.x + (bounds.size.x / 2), bounds.min.y + (bounds.size.y / 2));
    }
}