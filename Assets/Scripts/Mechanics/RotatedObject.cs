using UnityEngine;

public class RotatedObject : MonoBehaviour
{
    [SerializeField] private Axis AxisForRotation;
    [SerializeField] private bool ShouldRotateClockwise = true;
    [SerializeField] private float Speed;

    private Vector3 AxisForRotationAsVector3;
    private int RotationDirection;

    private void Awake()
    {
        RotationDirection = ShouldRotateClockwise ? 1 : -1;
        AxisForRotationAsVector3 = RepresentAxisAsVector3(AxisForRotation);
    }

    private Vector3 RepresentAxisAsVector3(Axis axis)
    {
        Vector3 Result = Vector3.zero;
        Result[(int)axis] = 1;
        return Result;
    }

    private void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(RotationDirection * Speed * Time.deltaTime, AxisForRotationAsVector3);
    }
}