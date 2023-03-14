using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Player_Input))]

[RequireComponent(typeof(BoxCollider2D))]

public class Player_Controller : MonoBehaviour
{
    Player_Input input;

    //private float localScale_x_facing;

    [SerializeField] private float speed_sneak;
    [SerializeField] private float speed_walk;
    [SerializeField] private float speed_run;

    BoxCollider2D boxCollider2D;
    [SerializeField] LayerMask layerMask_impassable;

    //[SerializeField] Text info_text;

    // Start is called before the first frame update
    private void Start() {
        input = GetComponent<Player_Input>();

        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Move(Vector3 velocity) {
        velocity *= Time.deltaTime;

        string Information = $"{transform.position.x} + ({velocity.x} => ";

        // Establish bounds from which the raycasts will be cast
        Bounds bounds = boxCollider2D.bounds;

        float bounds_skin_width = 0.01f;
        float bounds_expand_direction = -1f;
        float bounds_skin_layers_in_axis = 2f;
        float bounds_expand_multiplier = bounds_skin_width * bounds_expand_direction * bounds_skin_layers_in_axis;
        
        bounds.Expand(bounds_expand_multiplier);

        Vector2 bounds_topLeft = new Vector2(bounds.min.x, bounds.max.y);
        Vector2 bounds_topRight = new Vector2(bounds.max.x, bounds.max.y);
        Vector2 bounds_bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 bounds_bottomRight = new Vector2(bounds.max.x, bounds.min.y);

        // Movement
        int rays_amount_per_face = 3;
        float space_between_rays = bounds.size.x / (rays_amount_per_face - 1);

        // Horizontal
        for (int i = 0; i < rays_amount_per_face; i++) {
            Vector2 origin = (Mathf.Sign(velocity.x) == 1 ? bounds_topRight : bounds_topLeft) + i * space_between_rays * Vector2.down;
            Vector2 direction = Vector2.right * Mathf.Sign(velocity.x);
            float distance = Mathf.Abs(velocity.x) + bounds_skin_width;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask_impassable);
            Debug.DrawRay(origin, direction * distance, Color.red);

            if (hit) {
                velocity.x = (hit.distance == 0) ? 0 : (hit.distance - bounds_skin_width) * Mathf.Sign(velocity.x);
            }
        }
        transform.position += new Vector3(velocity.x, 0, 0);

        // Update bounds (player moved)
        bounds = boxCollider2D.bounds;
        bounds.Expand(bounds_expand_multiplier);

        bounds_topLeft = new Vector2(bounds.min.x, bounds.max.y);
        bounds_topRight = new Vector2(bounds.max.x, bounds.max.y);
        bounds_bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        bounds_bottomRight = new Vector2(bounds.max.x, bounds.min.y);

        // Vertical
        for (int i = 0; i < rays_amount_per_face; i++) {
            Vector2 origin = (Mathf.Sign(velocity.y) == 1 ? bounds_topLeft : bounds_bottomLeft) + i * space_between_rays * Vector2.right;
            Vector2 direction = Vector2.up * Mathf.Sign(velocity.y);
            float distance = Mathf.Abs(velocity.y) + bounds_skin_width;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask_impassable);
            Debug.DrawRay(origin, direction * distance, Color.red);

            if (hit) {
                velocity.y = (hit.distance == 0) ? 0 : (hit.distance - bounds_skin_width) * Mathf.Sign(velocity.y);
            }
        }

        transform.position += new Vector3(0, velocity.y, 0);
    }

    // Update is called once per frame
    void Update() {
        input.Check_Input();

        int direction_x = input.Left ? -1 : input.Right ? 1 : 0;
        int direction_y = input.Down ? -1 : input.Up ? 1 : 0;

        // if (direction_x != 0) {
        //     transform.localScale = new Vector3(direction_x, transform.localScale.y, transform.localScale.z);
        // }

        float speed = input.Run ? speed_run : input.Sneak ? speed_sneak : speed_walk;
        float velocity_x = direction_x * speed;
        float velocity_y = direction_y * speed;

        Vector3 velocity = new Vector3(velocity_x, velocity_y, 0);

        Move(velocity);
    }
}
