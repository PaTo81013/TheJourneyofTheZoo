using System;
using UnityEngine;

namespace Character.Views
{
    public interface ICharacterView
    {
        SpriteRenderer SpriteRenderer { get; }
        Rigidbody2D Rigidbody2D { get; }
        Transform Transform { get; }
        bool FlipSprite { get; set; }
        Vector2 Direction { get; }
        int MoveState { set; }
        event Action OnJumpButtonDown;
    }
}