using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG
{
    public class PlayerManagerBehavior : MonoBehaviour
    {
        CameraHandlerBehavior cameraHandler;
        InputHandlerBehavior inputHandler;
        Animator _anim;
        PlayerMovementBehvaior playerMovement;

        public bool _isInteracting;
        [Header("Player Flags")]
        public bool _isSprinting;

        private void Awake()
        {
            cameraHandler = CameraHandlerBehavior.singleton;
        }

        void Start()
        {
            inputHandler = GetComponent<InputHandlerBehavior>();
            _anim = GetComponentInChildren<Animator>();
            playerMovement = GetComponent<PlayerMovementBehvaior>();
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;

            _isInteracting = _anim.GetBool("isInteracting");
            inputHandler.TickInput(delta);
            playerMovement.HandleMovement(delta);
            playerMovement.HandleRollingAndSprinting(delta);
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler._mouseX, inputHandler._mouseY);
            }
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            _isSprinting = inputHandler.b_Input;
        }
    }
}

