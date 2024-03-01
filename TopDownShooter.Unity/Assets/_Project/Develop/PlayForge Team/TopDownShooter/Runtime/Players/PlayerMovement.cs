using PlayForge_Team.TopDownShooter.Runtime.Characters;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Players
{
    public sealed class PlayerMovement : CharacterMovement
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        
        [SerializeField] private float gravityMultiplier = 2f;
        [SerializeField] private float movementSpeed = 6f;
        [SerializeField] private float jumpSpeed = 30f;
        [SerializeField] private float jumpDuration = 1f;
        [SerializeField] private float groundCheckDistance = 0.2f;
        [SerializeField] private float groundCheckExtraUp = 0.2f;

        private CharacterController _characterController;
        private PlayerAction _playerAction;
        private Animator _animator;
        private Camera _mainCamera;
        private Vector3 _groundCheckBox;
        private bool _isGrounded;
        private bool _isJumping;
        private float _jumpTimer;
        private float _animationBlend;
        private float _movementSpeed;
        private float _targetRotation;

        protected override void OnInit()
        {
            _playerAction = GetComponent<PlayerAction>();
            _animator = GetComponentInChildren<Animator>();
            _characterController = GetComponent<CharacterController>();
            _mainCamera = Camera.main;
            _playerAction.JumpEvent += OnPlayerActionOnJumpEvent;

            var radius = _characterController.radius;
            _groundCheckBox = new Vector3(radius, 0.0001f, radius);
        }

        private void OnDestroy()
        {
            _playerAction.JumpEvent -= OnPlayerActionOnJumpEvent;
        }

        private void FixedUpdate()
        {
            Gravity();
            if (!IsActive)
            {
                return;
            }

            Move();
            Jumping();
        }

        private void Gravity()
        {
            var gravity = Physics.gravity;
            gravity *= gravityMultiplier * Time.fixedDeltaTime;
            _characterController.Move(gravity);
        }

        private void Move()
        {
            var targetSpeed = movementSpeed;
            var inputMove = _playerAction.MoveDirection;
            
            if (inputMove == Vector2.zero) targetSpeed = 0.0f;
            
            var currentInputDirection = new Vector3(inputMove.x, 0.0f, inputMove.y).normalized;
            currentInputDirection = GetMovementByCamera(currentInputDirection);
            
            _characterController.Move(currentInputDirection * (targetSpeed * Time.fixedDeltaTime));
            AnimateMovement(currentInputDirection);
        }

        private Vector3 GetMovementByCamera(Vector3 input)
        {
            var cameraTransform = _mainCamera.transform;
            var cameraForward = cameraTransform.forward;
            var cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            var movement = cameraForward * input.z + cameraRight * input.x;
            return movement;
        }

        private void AnimateMovement(Vector3 movement)
        {
            var relatedX = Vector3.Dot(movement.normalized, transform.right);
            var relatedY = Vector3.Dot(movement.normalized, transform.forward);

            _animator.SetFloat(Horizontal, relatedX);
            _animator.SetFloat(Vertical, relatedY);

        }

        private void Jumping()
        {
            RefreshIsGrounded();
            
            if (_isJumping)
            {
                _jumpTimer += Time.fixedDeltaTime;
                var motion = Vector3.up * (jumpSpeed * (1 - _jumpTimer / jumpDuration) * Time.fixedDeltaTime);
                _characterController.Move(motion);
                if (_jumpTimer >= jumpDuration)
                {
                    _isJumping = false;
                }
            }
        }

        private void RefreshIsGrounded()
        {
            SetIsGrounded(GroundCheck());
        }

        private bool GroundCheck()
        {
            var tr = transform;
            var startCheckPosition = tr.position + Vector3.up * groundCheckExtraUp;
            var checkDistance = groundCheckDistance + groundCheckExtraUp;
            return Physics.BoxCast(startCheckPosition, _groundCheckBox, Vector3.down, tr.rotation,
                checkDistance);
        }

        private void SetIsGrounded(bool value)
        {
            if (value != _isGrounded)
            {
                _animator.SetBool(IsGrounded, value);
            }
            _isGrounded = value;
        }
        
        private void OnPlayerActionOnJumpEvent()
        {
            if (_isGrounded && !_isJumping)
            {
                SetIsGrounded(false);
                _isJumping = true;
                _jumpTimer = 0;
            }
        }
    }
}