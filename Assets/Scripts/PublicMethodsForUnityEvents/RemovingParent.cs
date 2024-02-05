using UnityEngine;

public class RemovingParent : MonoBehaviour
{
    public void RemoveParent() => transform.parent = null;
}