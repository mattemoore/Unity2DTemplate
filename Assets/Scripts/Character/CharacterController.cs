using EditorAttributes;
using UnityEngine;

namespace Assets.Scripts
{
    public enum CharacterMovementDirection
    {
        Forward,
        Backward,
        Jump,
        Duck,
        None
    }

    public enum CharacterFacingDirection
    {
        Left,
        Right
    }

    public class CharacterController : MonoBehaviour
    {
        [SerializeField, Clamp(0.1f, 10.0f), Tooltip("The speed of the player.")]
        private float _moveSpeed = 2.0f;

        public CharacterFacingDirection FacingDirection { get; private set; }

        public void Move(CharacterMovementDirection moveDirection, float speedMultiplier = 1.0f)
        {
            if (moveDirection != CharacterMovementDirection.Forward && moveDirection != CharacterMovementDirection.Backward)
            {
                Debug.LogError("Invalid move direction.");
                return;
            }

            Vector3 direction = (moveDirection == CharacterMovementDirection.Forward) 
                ? (FacingDirection == CharacterFacingDirection.Right ? Vector3.right : Vector3.left) 
                : (FacingDirection == CharacterFacingDirection.Right ? Vector3.left : Vector3.right);
            transform.position += _moveSpeed * speedMultiplier * Time.deltaTime * direction;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            FacingDirection = CharacterFacingDirection.Right;
        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}
