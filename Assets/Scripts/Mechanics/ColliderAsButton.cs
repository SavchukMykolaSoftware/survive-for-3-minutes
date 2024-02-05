using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ColliderAsButton : MonoBehaviour
{
    [SerializeField] private Camera TheCamera;
    [SerializeField] private UnityEvent OnClick;
    [SerializeField] private MouseButton MouseButtonWhichPressesThisButton;
    [SerializeField] private float MaximalPossibleDistanceFromCameraToButton;

    private Collider ThisCollider;
    private int MouseButtonWhichPressesThisButtonAsInt;
    private Transform CameraTransform;

    private void Awake()
    {
        ThisCollider = GetComponent<Collider>();
        MouseButtonWhichPressesThisButtonAsInt = (int)MouseButtonWhichPressesThisButton;
        CameraTransform = TheCamera.transform;
    }

    private bool CanButtonBePressed()
    {
        return Vector3.Distance(CameraTransform.position, transform.position) <= MaximalPossibleDistanceFromCameraToButton;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(MouseButtonWhichPressesThisButtonAsInt) && CanButtonBePressed())
        {
            Ray RayForCheckingIfTheButtonWasPressed = TheCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] AllRaycastHits = Physics.RaycastAll(RayForCheckingIfTheButtonWasPressed);
            for (int i = 0; i < AllRaycastHits.Length; i++)
            {
                if (AllRaycastHits[i].collider.Equals(ThisCollider))
                {
                    OnClick.Invoke();
                }
            }
        }
    }
}