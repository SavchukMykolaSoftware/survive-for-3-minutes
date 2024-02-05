using UnityEngine;
using UnityEngine.UI;

public class ObjectRotatableWithMouse : MonoBehaviour
{
    [SerializeField] private float Sensitivity = 9.0f;
    [SerializeField] private float MinimumVerticalAngle = -89.0f;
    [SerializeField] private float MaximumVerticalAngle = 89.0f;
    [SerializeField] private MouseButton MouseButtonWhichRotatesObject;
    [SerializeField] private Slider SliderWhichSetsSensitivity;
    
    private float RotationX = 0;
    private int MouseButtonWhichRotatesObjectAsInt;

    private void Awake()
    {
        MouseButtonWhichRotatesObjectAsInt = (int)MouseButtonWhichRotatesObject;
        if(PlayerPrefs.HasKey(PlayerPrefsKeys.Sensitivity))
        {
            Sensitivity = PlayerPrefs.GetFloat(PlayerPrefsKeys.Sensitivity);
        }
        SliderWhichSetsSensitivity.onValueChanged.AddListener(newSensitivity =>
        {
            if (newSensitivity != 0)
            {
                Sensitivity = newSensitivity;
                PlayerPrefs.SetFloat(PlayerPrefsKeys.Sensitivity, newSensitivity);
            }
        });
        SliderWhichSetsSensitivity.value = Sensitivity;
    }

    private void Update()
    {
        if (Input.GetMouseButton(MouseButtonWhichRotatesObjectAsInt))
        {
            RotationX -= Input.GetAxis("Mouse Y") * Sensitivity;
            RotationX = Mathf.Clamp(RotationX, MinimumVerticalAngle, MaximumVerticalAngle);
            float Delta = Input.GetAxis("Mouse X") * Sensitivity;
            float RotationY = transform.localEulerAngles.y + Delta;
            transform.localEulerAngles = new Vector3(RotationX, RotationY, 0);
        }
    }
}