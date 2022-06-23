using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG
{
    public class AnimatorHandlerBehavior : MonoBehaviour
    {
        PlayerManagerBehavior playerManager;
        public Animator _anim;
        InputHandlerBehavior inputHandler;
        PlayerMovementBehvaior playerMovement;
        int _vertical;
        int _horizontal;
        public bool _canRotate;

        public void Initialized()
        {
            playerManager = GetComponentInParent<PlayerManagerBehavior>();
            _anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandlerBehavior>();
            playerMovement = GetComponentInParent<PlayerMovementBehvaior>();
            _vertical = Animator.StringToHash("Vertical");
            _horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if ( verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            _anim.SetFloat(_vertical, v, 0.1f, Time.deltaTime);
            _anim.SetFloat(_horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnim,bool isInteracting)
        {
            _anim.applyRootMotion = isInteracting;
            _anim.SetBool("isInteracting", isInteracting);
            _anim.CrossFade(targetAnim, 0.2f);
        }

        public void CanRotate()
        {
            _canRotate = true;
        }

        public void StopRotation()
        {
            _canRotate = false;
        }

        private void OnAnimatorMove()
        {
            if (playerManager._isInteracting == false)
                return;

            float delta = Time.deltaTime;
            playerMovement.rigidbody.drag = 0;
            Vector3 deltaPosition = _anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerMovement.rigidbody.velocity = velocity;
            

            
        }

    }
}


