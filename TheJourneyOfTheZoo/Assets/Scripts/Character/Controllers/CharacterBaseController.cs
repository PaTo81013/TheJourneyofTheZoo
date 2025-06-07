using System.Threading;
using Character.Models;
using Character.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Controllers
{
    public class CharacterBaseController : ICharacterBaseController
    {
        private readonly ICharacterView characterView;
        private ICharacterData characterData;
        private readonly CancellationTokenRegistration cancellationTokenRegistration;
        private readonly float jumpForce;
        private readonly float moveSpeed;
        private readonly Rigidbody2D rb2d;
        private readonly Transform transform;

        private static readonly Vector2 JumpDirection = Vector2.up;
        private const ForceMode2D ForceMode = ForceMode2D.Impulse;

        public CharacterBaseController(ICharacterView characterView, ICharacterData characterData, CancellationToken gameToken)
        {
            this.characterView = characterView;
            this.characterData = characterData;

            characterView.OnJumpButtonDown += Jump;

            jumpForce = characterData.JumpForce;
            moveSpeed = characterData.MoveSpeed;
            
            rb2d = characterView.Rigidbody2D;
            transform = characterView.Transform;
            
            cancellationTokenRegistration = gameToken.Register(Dispose);
            
            MovementCycleTask(gameToken).Forget();
        }

        private async UniTask MovementCycleTask(CancellationToken gameToken)
        {
            while (!gameToken.IsCancellationRequested)
            {
                var direction = characterView.Direction;
                var horizontal = direction.x;

                var flipX = characterView.FlipSprite;
                flipX = horizontal < 0 || !(horizontal > 0) && flipX;
                characterView.FlipSprite = flipX;
                characterView.MoveState = Mathf.Abs((int)horizontal);
                
                transform.Translate(direction * moveSpeed * Time.deltaTime);
                await UniTask.NextFrame();
            }
        }

        private void Jump()
        {
            rb2d.AddForce(JumpDirection * jumpForce, ForceMode);
        }

        public void Dispose()
        {
            characterView.OnJumpButtonDown -= Jump;
            cancellationTokenRegistration.Dispose();
        }
    }
}