using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up, Right, Down, Left };

[RequireComponent(typeof(Player_Animations))]
[RequireComponent(typeof(Player_Input))]
[RequireComponent(typeof(Player_Interaction))]
[RequireComponent(typeof(Player_Movement))]
[RequireComponent(typeof(Player_UI))]

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private float speed_sneak;
    [SerializeField] private float speed_walk;
    [SerializeField] private float speed_run;

    Player_Animations animations;
    Player_Input input;
    Player_Interaction interact;
    Player_Movement movement;
    Player_UI ui;

    Direction facingDirection;

    private void Start() {
        animations = GetComponent<Player_Animations>();
        input = GetComponent<Player_Input>();
        interact = GetComponent<Player_Interaction>();
        movement = GetComponent<Player_Movement>();
        ui = GetComponent<Player_UI>();
    }

    void Update() {
        input.Check_Input();

        Vector2 direction = new Vector2(
            input.Left ? -1 : input.Right ? 1 : 0,
            input.Down ? -1 : input.Up ? 1 : 0
        );

        float speed = input.Run ? speed_run : input.Sneak ? speed_sneak : speed_walk;

        if (direction.x != 0 || direction.y != 0) {
            if (direction.y == 1)       facingDirection = Direction.Up;
            else if (direction.y == -1) facingDirection = Direction.Down;
            else if (direction.x == 1)  facingDirection = Direction.Right;
            else if (direction.x == -1) facingDirection = Direction.Left;

            animations.UpdateSprite(facingDirection);
        }

        movement.Move(direction, speed);

        if (input.Interact) {
            interact.Interact(facingDirection);
        }

        if (input.Skip) {
            ui.CloseDialogBox();
        }
    }
}
