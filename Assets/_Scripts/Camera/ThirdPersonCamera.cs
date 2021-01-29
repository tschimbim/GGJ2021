using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    #region Unity References
    [SerializeField, Range(0.0f, 1.0f)] private float myLerpFactor = 0.5f;

    [SerializeField] private Transform myTarget = default;

    [SerializeField] private float myDesiredDistToTarget = 5.0f;
    [SerializeField] private float myMinDistToTarget = 1.0f;
    [SerializeField] private float myMaxDistToTarget = 8.0f;
    #endregion

    #region Unity Callbacks
    private void Update()
    {
        transform.LookAt(myTarget);

        Vector3 desiredPoint = myTarget.position + (transform.position - myTarget.position).normalized * myDesiredDistToTarget;

        transform.position = Vector3.Lerp(transform.position, desiredPoint, myLerpFactor);
    }
    #endregion
}
