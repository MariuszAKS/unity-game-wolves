using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Player_Animations))]
[RequireComponent(typeof(Player_Input))]
[RequireComponent(typeof(Player_Interact))]
[RequireComponent(typeof(Player_Movement))]

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private float speed_sneak;
    [SerializeField] private float speed_walk;
    [SerializeField] private float speed_run;

    Player_Animations animations;
    Player_Input input;
    Player_Interact interact;
    Player_Movement movement;

    char compassDirection;

    private void Start() {
        animations = GetComponent<Player_Animations>();
        input = GetComponent<Player_Input>();
        interact = GetComponent<Player_Interact>();
        movement = GetComponent<Player_Movement>();
    }

    void Update() {
        input.Check_Input();

        int direction_x = input.Left ? -1 : input.Right ? 1 : 0;
        int direction_y = input.Down ? -1 : input.Up ? 1 : 0;

        float speed = input.Run ? speed_run : input.Sneak ? speed_sneak : speed_walk;

        if (direction_x != 0 || direction_y != 0) {
            if (direction_y == 1)       compassDirection = 'N';
            else if (direction_y == -1) compassDirection = 'S';
            else if (direction_x == 1)  compassDirection = 'E';
            else if (direction_x == -1) compassDirection = 'W';

            animations.UpdateSprite(compassDirection);
        }

        Vector3 velocity = new Vector3(direction_x, direction_y, 0) * speed;

        movement.Move(velocity);

        if (input.Interact) {
            interact.CheckForInteractable(compassDirection);
        }
    }
}
