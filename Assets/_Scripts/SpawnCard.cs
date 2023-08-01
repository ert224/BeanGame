using System.Collections;
using UnityEngine;

public class SpawnCard : MonoBehaviour
{
    public Transform networkedOwner;
    // The Transform object that this tail is following
    public Transform followTransform;

    [SerializeField, Tooltip("Represents the time delay between the GameObject and it's target")] private float delayTime = 0.1f;
    [SerializeField, Tooltip("The distance the GameObject should keep from it's target")] private float distance = 50f;
    [SerializeField, Tooltip("Movement lerp speed")] private float moveStep = 20f;
    [SerializeField, Tooltip("Duration of the lerp")] private float lerpDuration = 1f; // Adjust this value

    private Vector3 _targetPosition;

    // Call this function after instantiation
    public void BeginMoveToParent()
    {
        StartCoroutine(MoveToParent());
    }

    private IEnumerator MoveToParent()
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            _targetPosition = followTransform.position - followTransform.forward * distance;
            _targetPosition.z = 0f;

            transform.position = Vector3.Lerp(transform.position, _targetPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        // Snap to the final position
        transform.position = _targetPosition;
    }
}
