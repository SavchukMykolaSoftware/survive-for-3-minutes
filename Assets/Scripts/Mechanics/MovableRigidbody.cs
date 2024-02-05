using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovableRigidbody : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private GameObject GameObjectWhichDeterminesDirection;
    
    [Header("Controls")]
    [SerializeField] private string ForwardButton;
    [SerializeField] private string BackButton;
    [SerializeField] private string LeftButton;
    [SerializeField] private string RightButton;

    private Rigidbody Rigidbody;
    private Vector3 Velocity;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        float AngleToRotate = GameObjectWhichDeterminesDirection.transform.eulerAngles.y;
        Vector3 Move = Quaternion.AngleAxis(AngleToRotate, Vector3.up) * transform.forward;

        Velocity = Vector3.zero;

        void MoveWithDirection(string ButtonWhichShouldBePressedForMoving, float AngleToRotate)
        {
            if (Input.GetButton(ButtonWhichShouldBePressedForMoving))
            {
                Velocity += (Quaternion.AngleAxis(AngleToRotate, Vector3.up) * Move);
            }
        }

        MoveWithDirection(ForwardButton, -180);
        MoveWithDirection(LeftButton, -270);
        MoveWithDirection(BackButton, 0);
        MoveWithDirection(RightButton, -90);

        Velocity = Velocity.normalized * Speed;

        Rigidbody.velocity = Velocity;
    }
}