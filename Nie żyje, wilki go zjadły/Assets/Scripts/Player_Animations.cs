using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite head_down;
    [SerializeField] private Sprite head_left;
    [SerializeField] private Sprite head_right;
    [SerializeField] private Sprite head_up;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSprite(Direction facingDirection) {
        switch (facingDirection) {
            case Direction.Up: spriteRenderer.sprite = head_up; break;
            case Direction.Right: spriteRenderer.sprite = head_right; break;
            case Direction.Down: spriteRenderer.sprite = head_down; break;
            case Direction.Left: spriteRenderer.sprite = head_left; break;
        }
    }
}
