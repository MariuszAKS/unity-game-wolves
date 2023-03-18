using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Input : MonoBehaviour {
    private bool up, down, left, right;
    private bool run;
    private bool sneak;

    private bool interact;
    private bool skip;

    public bool Up { get { return up; } }
    public bool Down { get { return down; } }
    public bool Left { get { return left; } }
    public bool Right { get { return right; } }
    
    public bool Run { get { return run; } }
    public bool Sneak { get { return sneak; } }

    public bool Interact { get { return interact; } }
    public bool Skip { get { return skip; } }

    public void Check_Input() {
        up = Input.GetKey(KeyCode.UpArrow);
        down = Input.GetKey(KeyCode.DownArrow);
        left = Input.GetKey(KeyCode.LeftArrow);
        right = Input.GetKey(KeyCode.RightArrow);

        run = Input.GetKey(KeyCode.LeftShift);
        sneak = Input.GetKey(KeyCode.LeftControl);

        interact = Input.GetKeyDown(KeyCode.E);
        skip = Input.GetKeyDown(KeyCode.X);
    }
}