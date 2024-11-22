using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform targetToFollow;
    [SerializeField] float smoothing = 0.6f;
    [SerializeField] Vector3 offset = Vector3.zero;

    Vector3 velocity = Vector3.zero;

    void Start()
    {
        transform.position = new Vector3(
            targetToFollow.position.x,
            targetToFollow.position.y,
            transform.position.z) + offset;
    }

   
    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(
            targetToFollow.position.x,
            targetToFollow.position.y,
            transform.position.z) + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothing);
    }
}
