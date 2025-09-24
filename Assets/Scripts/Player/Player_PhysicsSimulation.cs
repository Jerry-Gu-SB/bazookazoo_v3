using Global_Utils;
using UnityEngine;

namespace Player
{
    public class PlayerPhysicsSimulation : MonoBehaviour
    {
        [Header("Physics parameters")]
        public float moveForce = 30f;
        public float maxControlSpeed = 10f;
        public float controlInAirMultiplier = 0.75f;
        public float fallingMultiplier = 15f;
        
        [Header("Terrain Layer")]
        public LayerMask terrainLayer;
        
        private Rigidbody2D _rigidbody2D;
        
        public void AddForce(Vector2 force)
        {
            _rigidbody2D.AddForce(force, ForceMode2D.Force);
        }

        public void AddImpulse(Vector2 impulse)
        {
            _rigidbody2D.AddForce(impulse, ForceMode2D.Impulse);
        }

        public void HandleInput(PlayerInput.InputPayLoad playerInput)
        {
            Vector2 input =  playerInput.InputVector;
            bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, .6f, terrainLayer);
            float controlMultiplier = isGrounded ? 1f : controlInAirMultiplier;  // Decrease air horizontal control

            if (_rigidbody2D.linearVelocityY < 0 && !isGrounded)
            {
                _rigidbody2D.linearVelocityY -= fallingMultiplier * GlobalTick.Instance.minTimeBetweenTicks / (1f / Time.deltaTime);
            }
            
            // Only add force if we're under max control speed in input direction
            if (Mathf.Approximately(Mathf.Sign(input.x), Mathf.Sign(_rigidbody2D.linearVelocityX)) &&
                !(Mathf.Abs(_rigidbody2D.linearVelocityX) < maxControlSpeed)) return;
            
            float velocityInInputDir = _rigidbody2D.linearVelocityX * input.x;
            float forceScale = Mathf.Clamp01(1f - (velocityInInputDir / maxControlSpeed));
            
            // I won't lie gpt cooked this weird equation, but it feels good so we move.
            AddForce(Vector2.right * input * (moveForce * forceScale * controlMultiplier));
        }
    }
}
