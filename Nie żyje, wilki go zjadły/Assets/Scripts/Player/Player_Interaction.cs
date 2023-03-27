using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    [SerializeField] LayerMask layerMask_interact;

    Player_UI ui;

    int layer_interactText;
    int layer_interactAction;
    int layer_interactCollect;

    BoxCollider2D boxCollider2D;
    Bounds bounds;
    Vector2 bounds_topLeft, bounds_topRight, bounds_bottomLeft, bounds_bottomRight, bounds_center;

    Vector2 origin, direction, next_ray_direction;
    float length, space_between_rays;
    const byte rays_per_side = 5;

    bool collected_leftArm, collected_rightArm;
    bool collected_leftLeg, collected_rightLeg;
    bool collected_torso;

    private void Start() {
        ui = GetComponent<Player_UI>();

        layer_interactText = LayerMask.NameToLayer("Interact Text");
        layer_interactAction = LayerMask.NameToLayer("Interact Action");
        layer_interactCollect = LayerMask.NameToLayer("Interact Collect");

        boxCollider2D = GetComponent<BoxCollider2D>();

        collected_leftArm = collected_rightArm = false;
        collected_leftLeg = collected_rightLeg = false;
        collected_torso = false;
    }

    public void Interact(Direction facingDirection) {
        GetBounds();

        GetRaycastParameters(facingDirection);

        for (int ray = 0; ray < rays_per_side; ray++) {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, length, layerMask_interact);
            Debug.DrawRay(origin, direction * length, Color.blue);

            origin += next_ray_direction * space_between_rays;

            if (hit) {
                HandleInteraction(hit);
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

    void GetRaycastParameters(Direction facingDirection) {
        switch (facingDirection) {
            case Direction.Up:
                origin = bounds_bottomLeft;
                direction = Vector2.up;
                next_ray_direction = Vector2.right;
                length = bounds.size.y + 0.25f;
                space_between_rays = bounds.size.x / (rays_per_side - 1);
                break;
            case Direction.Right:
                origin = bounds_topLeft;
                direction = Vector2.right;
                next_ray_direction = Vector2.down;
                length = bounds.size.x + 0.25f;
                space_between_rays = bounds.size.y / (rays_per_side - 1);
                break;
            case Direction.Down:
                origin = bounds_topRight;
                direction = Vector2.down;
                next_ray_direction = Vector2.left;
                length = bounds.size.y + 0.25f;
                space_between_rays = bounds.size.x / (rays_per_side - 1);
                break;
            case Direction.Left:
                origin = bounds_bottomRight;
                direction = Vector2.left;
                next_ray_direction = Vector2.up;
                length = bounds.size.x + 0.25f;
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
    }

    void HandleInteraction(RaycastHit2D hit) {
        string hitColliderName = hit.collider.name;
        int layer = hit.collider.gameObject.layer;

        if (layer == layer_interactText) {
            Interact_Text interactText = hit.collider.GetComponent<Interact_Text>();

            if (interactText == null) {
                ui.UpdateUIdialogBox("Text Script on Object not found");
                return;
            }

            ui.UpdateUIdialogBox(interactText.Text);

            return;
        }
        
        if (layer == layer_interactAction) {
            return;
        }

        if (layer == layer_interactCollect) {
            switch(hitColliderName) {
                case "Left_Arm": collected_leftArm = true; break;
                case "Right_Arm": collected_rightArm = true; break;
                case "Left_Leg": collected_leftLeg = true; break;
                case "Right_Leg": collected_rightLeg = true; break;
                case "Torso": collected_torso = true; break;
            }

            Destroy(hit.collider.gameObject);

            ui.UpdateUIinfo(collected_leftArm, collected_rightArm, collected_leftLeg, collected_rightLeg, collected_torso);

            if (collected_leftArm && collected_rightArm && collected_leftLeg && collected_rightLeg && collected_torso) {
                Debug.Log("Winning condition fulfilled!");
            }

            return;
        }
    }
}