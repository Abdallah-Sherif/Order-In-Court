using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URC.Core;
using URC.Utility;


namespace URC.Movement
{
    public class Dash : Module
    {
        // Start is called before the first frame update
        [Header("Dash Properties")]
        public float DashSpeed;
        [Tooltip("Key to start dashing")]
        public KeyCode m_dashKey;
        [Tooltip("Dash Cooldown")]
        public float dashCooldown = 1f;
        private Vector3 MoveDirection;
        private bool canDash = true;
        private void Update()
        {
            MoveDirection = getMoveDirection();
            if(Input.GetKey(m_dashKey) && canDash)
            {
                StartCoroutine(ExecuteDash());
            }
        }

        IEnumerator ExecuteDash()
        {
            StartDash();
            canDash = false;
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(DashCooldown());
            EndDash();
        }
        IEnumerator DashCooldown()
        {
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
        void StartDash()
        {
            Motor.AddImpulse(MoveDirection * DashSpeed);
            InputHelper.DisableMovement(true);
        }
        void EndDash()
        {
            InputHelper.DisableMovement(false);
            Motor.ResetVelocity();
        }
        
        private Vector3 getMoveDirection()
        {
            Vector3 worldDir = InputHelper.DesiredDirection().normalized;
            return transform.TransformDirection(worldDir);
        }
    }
}
