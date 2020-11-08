using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class CharacterCameraShakeController : MonoBehaviour
{
    public CinemachineImpulseSource dashImpulseSource;
    public CinemachineImpulseSource landImpulseSource;


    private CharacterController m_CharacterController;
    private Rigidbody2D m_Rigidbody2D;
    private float lastEventTime = 0f;
    private bool previousGrounded = true;

    private void Awake()
    {
        m_CharacterController = this.GetComponent<CharacterController>();
        m_Rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        previousGrounded = m_CharacterController.IsGrounded;
    }

    private void Update()
    {
        if (m_CharacterController.GetDash())
        {
            if (m_CharacterController.hasGrapplingHook)
            {
                float now = Time.time;
                float eventLength = dashImpulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime + dashImpulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime;
                Vector3 distance = m_CharacterController.ally.transform.position - transform.position + new Vector3(0, 0.5f, 0);
                if (distance.magnitude > 1 && now - lastEventTime > eventLength)
                {
                    dashImpulseSource.m_ImpulseDefinition.CreateEvent(transform.position, Vector3.down);
                    lastEventTime = now;
                }
            }
            else
            {
                dashImpulseSource.GenerateImpulse();
            }
        }
    }

    private void LateUpdate()
    {
        // We have hit the ground this frame
        if (m_CharacterController.IsGrounded == true && previousGrounded == false)
        {
            landImpulseSource.GenerateImpulse();
        }
        previousGrounded = m_CharacterController.IsGrounded;
    }

}
