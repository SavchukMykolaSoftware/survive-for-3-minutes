using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventOnCollisionWithLayer
{
    public int Layer;
    public UnityEvent OnCollisionWithThisLayer;
}

public class CollisionsHandler : MonoBehaviour
{
    [SerializeField] private List<EventOnCollisionWithLayer> OnCollisionsWithLayers = new();

    private void OnCollisionEnter(Collision collision)
    {
        CheckLayer(collision.gameObject.layer);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckLayer(other.gameObject.layer);
    }

    private void CheckLayer(int layerToBeChecked)
    {
        for(int i = 0; i < OnCollisionsWithLayers.Count; i++)
        {
            if (OnCollisionsWithLayers[i].Layer == layerToBeChecked)
            {
                OnCollisionsWithLayers[i].OnCollisionWithThisLayer.Invoke();
            }
        }
    }
}