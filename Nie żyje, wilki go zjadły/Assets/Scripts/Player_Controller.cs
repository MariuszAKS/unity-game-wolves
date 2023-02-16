using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_Input))]

[RequireComponent(typeof(BoxCollider2D))]

public class Player_Controller : MonoBehaviour
{
    Player_Input input;

    private float localScale_x_facing;

    [SerializeField] private float speed_sneak;
    [SerializeField] private float speed_walk;
    [SerializeField] private float speed_run;

    BoxCollider2D boxCollider2D;
    [SerializeField] LayerMask layerMask_impassable;

    // Start is called before the first frame update
    private void Start() {
        input = GetComponent<Player_Input>();

        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        input.Check_Input();

        int direction_x = input.Left ? -1 : input.Right ? 1 : 0;
        int direction_y = input.Down ? -1 : input.Up ? 1 : 0;

        if (direction_x != 0) {
            transform.localScale = new Vector3(direction_x, transform.localScale.y, transform.localScale.z);
        }

        float speed = input.Run ? speed_run : input.Sneak ? speed_sneak : speed_walk;
        float velocity_x = direction_x * speed;
        float velocity_y = direction_y * speed;



        float bounds_skin_width = 0.01f;
        float bounds_expand_direction = -1f;
        float bounds_skin_layers_in_axis = 2f;
        float bounds_expand_multiplier = bounds_skin_width * bounds_expand_direction * bounds_skin_layers_in_axis;

        Bounds bounds = boxCollider2D.bounds;
        bounds.Expand(bounds_expand_multiplier);

        Vector2 bounds_topLeft = new Vector2(bounds.min.x, bounds.max.y);
        Vector2 bounds_topRight = new Vector2(bounds.max.x, bounds.max.y);
        Vector2 bounds_bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 bounds_bottomRight = new Vector2(bounds.max.x, bounds.min.y);

        int rays_amount_per_face = 3;
        float space_between_rays = bounds.size.x / (rays_amount_per_face - 1);

        for (int i = 0; i < rays_amount_per_face; i++) {
            RaycastHit2D hit = Physics2D.Raycast(
                (direction_x == 1 ? bounds_topRight : bounds_topLeft) + i * space_between_rays * Vector2.down,
                Vector2.right * direction_x,
                velocity_x * Time.deltaTime + bounds_skin_width,
                layerMask_impassable
            );

            if (hit) {
                velocity_x = (hit.distance - bounds_skin_width) * direction_x;
            }
        }

        transform.position += new Vector3(velocity_x, 0, 0) * Time.deltaTime;



        bounds = boxCollider2D.bounds;
        bounds.Expand(bounds_expand_multiplier);

        bounds_topLeft = new Vector2(bounds.min.x, bounds.max.y);
        bounds_topRight = new Vector2(bounds.max.x, bounds.max.y);
        bounds_bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        bounds_bottomRight = new Vector2(bounds.max.x, bounds.min.y);

        for (int i = 0; i < rays_amount_per_face; i++) {
            RaycastHit2D hit = Physics2D.Raycast(
                (direction_y == 1 ? bounds_topRight : bounds_bottomRight) + i * space_between_rays * Vector2.left,
                Vector2.up * direction_y,
                velocity_y * Time.deltaTime + bounds_skin_width,
                layerMask_impassable
            );

            if (hit) {
                velocity_y = (hit.distance - bounds_skin_width) * direction_y;
            }
        }

        transform.position += new Vector3(0, velocity_y, 0) * Time.deltaTime;
    }
}
