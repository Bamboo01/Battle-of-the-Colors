using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Bamboo.Utility;

namespace CSZZGame.Refactor
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] Camera mainCamera;
        [SerializeField] CinemachineVirtualCamera cinemachineVC;
        Transform _lookat;

        protected override void OnAwake()
        {
            _persistent = false;
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void AssignTargets(Transform follow, Transform lookat)
        {
            cinemachineVC.LookAt = lookat;
            cinemachineVC.Follow = follow;
            _lookat = lookat;
        }

        private void Update()
        {
            if (_lookat)
            _lookat.rotation = cinemachineVC.VirtualCameraGameObject.transform.rotation;
        }
        public Transform GetCameraTransform()
        {
            return mainCamera.transform;
        }
    }
}
