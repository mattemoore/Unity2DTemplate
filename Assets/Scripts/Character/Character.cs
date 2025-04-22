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

    public enum CharacterAction
    {
        Action1,
        Action2,
        Action3,
        Action4,
        None
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class Character : MonoBehaviour
    {
        // TODO: Change this to a bool and fix animations running opposite for enemy
        public bool IsFacingRight = true;
        public CharacterAttributes Attributes;

        private SpriteRenderer _spriteRenderer;

        public void Move(CharacterMovementDirection moveDirection, float speedMultiplier = 1.0f)
        {
            if (moveDirection != CharacterMovementDirection.Forward && moveDirection != CharacterMovementDirection.Backward)
            {
                Debug.LogError("Invalid move direction.");
                return;
            }
            
            Vector3 direction = moveDirection == CharacterMovementDirection.Forward == IsFacingRight 
                ? Vector3.right 
                : Vector3.left;
            transform.position += Attributes.Speed * speedMultiplier * Time.deltaTime * direction;
        }

        public void Flip()
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Attributes = new KarateMan();
            if (!IsFacingRight)
                Flip();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}
