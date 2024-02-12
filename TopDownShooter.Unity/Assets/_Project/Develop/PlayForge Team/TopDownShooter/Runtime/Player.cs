using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime
{
    public sealed class Player : MonoBehaviour
    {
        private const string MovementHorizontalKey = "Horizontal";
        private const string MovementVerticalKey = "Vertical";
        private const string IsGroundedKey = "IsGrounded";
        
        private static readonly int Horizontal = Animator.StringToHash(MovementHorizontalKey);
        private static readonly int Vertical = Animator.StringToHash(MovementVerticalKey);
        private static readonly int IsGrounded = Animator.StringToHash(IsGroundedKey);
        
        [SerializeField] private float gravityMultiplier = 2f;
        [SerializeField] private float movementSpeed = 6f;
        [SerializeField] private float jumpSpeed = 30f;
        [SerializeField] private float jumpDuration = 1f;
        [SerializeField] private float groundCheckDistance = 0.2f;
        [SerializeField] private float groundCheckExtraUp = 0.2f;
        [SerializeField] private float aimingSpeed = 10f;
        
        private Animator _animator;
        private CharacterController _characterController;
        private Camera _mainCamera;
        private Vector3 _groundCheckBox;
        private bool _isGrounded;
        private bool _isJumping;
        private float _jumpTimer;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _animator = GetComponentInChildren<Animator>();
            _characterController = GetComponent<CharacterController>();
            _mainCamera = Camera.main;
            var radius = _characterController.radius;
            _groundCheckBox = new Vector3(radius, 0.0001f, radius);
        }
        
        private void FixedUpdate()
        {
            Gravity();
            Movement();
            Jumping();
            Aiming();
        }
        
        private void Gravity()
        {
            var gravity = Physics.gravity;
            gravity *= gravityMultiplier * Time.fixedDeltaTime;
            _characterController.Move(gravity);
        }
        
        private void Movement()
        {
            var movement = Vector3.zero;
            movement.x = Input.GetAxis(MovementHorizontalKey);
            movement.z = Input.GetAxis(MovementVerticalKey);
            movement = GetMovementByCamera(movement);
            movement *= movementSpeed * Time.fixedDeltaTime;
            _characterController.Move(movement);
            AnimateMovement(movement);
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
            
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !_isJumping)
            {
                SetIsGrounded(false);
                _isJumping = true;
                _jumpTimer = 0;
            }
            
            if (_isJumping)
            {
                _jumpTimer += Time.fixedDeltaTime;
                var motion = Vector3.up * (jumpSpeed * (1 - _jumpTimer / jumpDuration) * Time.fixedDeltaTime);
                _characterController.Move(motion);
                
                if (_jumpTimer >= jumpDuration || _isGrounded)
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
            return Physics.BoxCast(startCheckPosition, _groundCheckBox, Vector3.down, tr.rotation, checkDistance);
        }

        private void SetIsGrounded(bool value)
        {
            if (value != _isGrounded)
            {
                _animator.SetBool(IsGrounded, value);
            }
            _isGrounded = value;
        }
        
        private void Aiming()
        {
            var mouseScreenPosition = Input.mousePosition;
            var findTargetRay = _mainCamera.ScreenPointToRay(mouseScreenPosition);
    
            if (Physics.Raycast(findTargetRay, out var hitInfo))
            {
                var lookDirection = (hitInfo.point - transform.position).normalized;
                lookDirection.y = 0;
                var newRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.fixedDeltaTime * aimingSpeed);
            }
        }
    }
}