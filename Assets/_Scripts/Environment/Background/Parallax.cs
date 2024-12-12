using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cameraTransform;

    public float parallaxEffect;

    private Vector3 lastCameraPosition;

    #region Unity Callback Functions
    private void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    private void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(deltaMovement.x * parallaxEffect, 0, 0);

        lastCameraPosition = cameraTransform.position;
    }
    #endregion
}
