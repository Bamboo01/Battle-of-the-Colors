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
        [SerializeField] CinemachineFreeLook cinemachineFreeLook;

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
            cinemachineFreeLook.LookAt = lookat;
            cinemachineFreeLook.Follow = follow;
        }

        public Transform GetCameraTransform()
        {
            return mainCamera.transform;
        }
    }
}
