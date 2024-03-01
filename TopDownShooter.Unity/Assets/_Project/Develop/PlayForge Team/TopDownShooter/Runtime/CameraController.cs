using PlayForge_Team.TopDownShooter.Runtime.Players;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime
{
    public sealed class CameraController : MonoBehaviour
    {
        [SerializeField] private PlayerAction playerAction;
        [SerializeField] private Transform target;
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Vector3 positionOffset = Vector3.up;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 65f;
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private float minZoom = 3f;
        [SerializeField] private float maxZoom = 14f;
        private float _currentZoom;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _currentZoom = (target.position - cameraTransform.position).magnitude;
        }
        
        private void LateUpdate()
        {
            MoveCamera();
            RotateCamera();
            ZoomCamera();
        }
        
        private void MoveCamera()
        {
            if (!target || !cameraRoot)
            {
                return;
            }
            var targetPosition = target.position + positionOffset;
            cameraRoot.transform.position =
                Vector3.Lerp(cameraRoot.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        
        private void RotateCamera()
        {
            if (!cameraRoot)
            {
                return;
            }
            var direction = playerAction.RotateCameraDirection;
            
            
            if (Mathf.Approximately(direction, 0))
            {
                return;
            }
            
            var cameraEuler = cameraRoot.eulerAngles;
            cameraEuler.y += direction * rotationSpeed * Time.deltaTime;
            cameraRoot.eulerAngles = cameraEuler;
        }
        
        private void ZoomCamera()
        {
            if (!cameraTransform)
            {
                return;
            }
            
            var direction = playerAction.ZoomCamera;
            
            if (Mathf.Approximately(direction, 0))
            {
                return;
            }
            _currentZoom += direction * zoomSpeed * Time.deltaTime;

            _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);

            cameraTransform.position = cameraRoot.position - cameraTransform.forward * _currentZoom;
        }
    }
}