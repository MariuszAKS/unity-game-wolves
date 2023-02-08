using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    float localScale_x_facing;

    // Start is called before the first frame update
    private void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        int direction_x = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
        int direction_y = Input.GetKey(KeyCode.DownArrow) ? -1 : Input.GetKey(KeyCode.UpArrow) ? 1 : 0;

        if (direction_x != 0)
        {
            localScale_x_facing = Mathf.Abs(transform.localScale.x) * direction_x;
            transform.localScale = new Vector3(localScale_x_facing, transform.localScale.y, transform.localScale.z);
        }

        transform.position += new Vector3(direction_x, direction_y, 0) * Time.deltaTime;
    }
}
