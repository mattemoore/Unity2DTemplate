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
        public BoxCollider2D CollisionCollider;
        public BoxCollider2D HitBox;
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
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
            // TODO: Press '1' to show hitbox/hurtbox etc....
            DrawColliders();
        }

        private void DrawColliders()
        {
            if (CollisionCollider != null)
            {
                DrawBoxCollider2D(CollisionCollider, Color.green);
            }
            if (HitBox != null)
            {
                DrawBoxCollider2D(HitBox, Color.blue);
            }
        }

        void DrawBoxCollider2D(BoxCollider2D collider, Color color)
        {
            Vector2 center = collider.bounds.center;
            Vector2 size = collider.bounds.size;
            float angle = collider.transform.eulerAngles.z;

            Vector2 right = Quaternion.Euler(0, 0, angle) * Vector2.right * size.x * 0.5f;
            Vector2 up = Quaternion.Euler(0, 0, angle) * Vector2.up * size.y * 0.5f;

            Vector3[] corners = new Vector3[5];
            corners[0] = center - right - up;
            corners[1] = center - right + up;
            corners[2] = center + right + up;
            corners[3] = center + right - up;
            corners[4] = corners[0]; // close the loop

            LineRenderer lineRenderer = collider.GetComponent<LineRenderer>();
            if (lineRenderer == null)
                throw new System.Exception("LineRenderer component is missing on the collider GameObject.");
            lineRenderer.positionCount = 5;
            lineRenderer.SetPositions(corners);
        }
    }
}
