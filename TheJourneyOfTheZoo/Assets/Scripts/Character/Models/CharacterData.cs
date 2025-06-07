using System;
using UnityEngine;

namespace Character.Models
{
    [Serializable]
    public class CharacterData : ICharacterData
    {
        [SerializeField] 
        private StyleNameData styleName;
        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float jumpForce;

        public CharacterData(StyleNameData styleName, float moveSpeed, float jumpForce)
        {
            this.styleName = styleName;
            this.moveSpeed = moveSpeed;
            this.jumpForce = jumpForce;
        }

        public float MoveSpeed => moveSpeed;
        public float JumpForce => jumpForce;
        public StyleNameData StyleName => styleName;
        
        
    }
}