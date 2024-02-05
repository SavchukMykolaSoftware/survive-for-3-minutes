using UnityEngine;

public class DestroyingWithDelay : MonoBehaviour
{
    public void DestroyWithDelay(float delay) => Destroy(gameObject, delay);
}