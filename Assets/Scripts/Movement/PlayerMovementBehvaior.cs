using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG
{
    public class PlayerMovementBehvaior : MonoBehaviour
    {
        PlayerManagerBehavior playerManager;
        Transform _cameraObject;
        InputHandlerBehavior inputHandler;
        Vector3 _moveDirection;

        [HideInInspector]
        public Transform _myTransform;
        [HideInInspector]
        public AnimatorHandlerBehavior animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject _normalCamera;

        [Header("Movement Stats")]
        [SerializeField]
        float _movementSpeed = 5;
        [SerializeField]
        float _sprintSpeed = 7;
        [SerializeField]
        float _rotationSpeed = 10;

        
        

        // Start is called before the first frame update
        void Start()
        {
            playerManager = GetComponent<PlayerManagerBehavior>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandlerBehavior>();
            animatorHandler = GetComponentInChildren<AnimatorHandlerBehavior>();
            _cameraObject = Camera.main.transform;
            _myTransform = transform;
            animatorHandler.Initialized();
        }

        

        #region Movement
        Vector3 _normalVector;
        Vector3 _targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler._moveAmount;

            targetDir = _cameraObject.forward * inputHandler._vertical;
            targetDir += _cameraObject.right * inputHandler._horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = _myTransform.forward;

            float rs = _rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(_myTransform.rotation, tr, rs * delta);

            _myTransform.rotation = targetRotation;
        }

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)
                return;

            _moveDirection = _cameraObject.forward * inputHandler._vertical;
            _moveDirection += _cameraObject.right * inputHandler._horizontal;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            float speed = _movementSpeed;

            if(inputHandler.sprintFlag)
            {
                speed = _sprintSpeed;
                playerManager._isSprinting = true;
                _moveDirection *= speed;
            }
            else
            {
                _moveDirection *= speed;
            }
            

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
            rigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler._moveAmount, 0, playerManager._isSprinting);

            if (animatorHandler._canRotate)
            {
                HandleRotation(delta);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (animatorHandler._anim.GetBool("isInteracting"))
                return;

            if(inputHandler.rollFlag)
            {
                _moveDirection = _cameraObject.forward * inputHandler._vertical;
                _moveDirection += _cameraObject.right * inputHandler._horizontal;

                if (inputHandler._moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Dodge", true);
                    _moveDirection.y = 0;
                    Quaternion dodgeRoation = Quaternion.LookRotation(_moveDirection);
                    _myTransform.rotation = dodgeRoation;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Backstep", true);
                }
            }
        }
            

        #endregion

    }
}


