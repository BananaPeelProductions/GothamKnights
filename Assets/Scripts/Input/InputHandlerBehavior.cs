using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG
{

    public class InputHandlerBehavior : MonoBehaviour
    {
        public float _horizontal;
        public float _vertical;
        public float _moveAmount;
        public float _mouseX;
        public float _mouseY;

       

        PlayerControls inputActions;
        CameraHandlerBehavior cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            cameraHandler = CameraHandlerBehavior.singleton;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, _mouseX, _mouseY);
            }
        }


        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            
        }

        private void MoveInput(float delta)
        {
            _horizontal = movementInput.x;
            _vertical = movementInput.y;
            _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
            _mouseX = cameraInput.x;
            _mouseY = cameraInput.y;
        }

       
        
    }

    

}
