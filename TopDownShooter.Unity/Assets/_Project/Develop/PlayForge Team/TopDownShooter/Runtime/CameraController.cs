using PlayForge_Team.TopDownShooter.Runtime.Players;
using Cinemachine;
using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime
{
    public sealed class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private PlayerAction playerAction;
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float rotationSpeed = 65f;
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private float minZoom = 3f;
        [SerializeField] private float maxZoom = 14f;
        
        private CinemachineFramingTransposer _transposer;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        
        private void LateUpdate()
        {
            RotateCamera();
            ZoomCamera();
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

            var currentEulerAngles = cameraRoot.eulerAngles;
            var cameraEuler = currentEulerAngles;
            cameraEuler.y += direction;
            currentEulerAngles = Vector3.Lerp(currentEulerAngles, cameraEuler, rotationSpeed * Time.deltaTime);
            cameraRoot.eulerAngles = currentEulerAngles;
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
            var currentZoom = direction * zoomSpeed * Time.deltaTime;

            _transposer.m_CameraDistance += currentZoom;
            _transposer.m_CameraDistance = Mathf.Clamp(_transposer.m_CameraDistance, minZoom, maxZoom);
        }
    }
}