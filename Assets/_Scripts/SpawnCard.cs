
using UnityEngine;

public class SpawnCard : MonoBehaviour
{
    public Transform networkedOwner;
    // The Transform object that this tail is following
    public Transform followTransform;

    [SerializeField, Tooltip("Represents the time delay between the GameObject and it's target")] private float delayTime = 0.1f;
    [SerializeField, Tooltip("The distance the GameObject should keep from it's target")] private float distance = 15f;
    [SerializeField, Tooltip("Movement lerp speed")] private float moveStep = 20f;

    private Vector3 _targetPosition;

    /// <summary>
    /// Move the tail towards the target with a delay.
    /// </summary>
    private void Update()
    {
        _targetPosition = followTransform.position - followTransform.forward * distance;
        _targetPosition += (transform.position - _targetPosition) * delayTime;
        _targetPosition.z = 0f;
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * moveStep);
    }
}