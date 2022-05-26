using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandlerBehavior : MonoBehaviour
{
    public Transform _targetTransform;
    public Transform _cameraTransform;
    public Transform _cameraPivotTransform;
    private Transform _playerTransform;
    private Vector3 _cameraTransformPosition;
    private LayerMask _ignoreLayers;

    public static CameraHandlerBehavior singleton;

    [SerializeField]
    public float _lookSpeed = 0.1f;
    [SerializeField]
    public float _followSpeed = 0.1f;
    [SerializeField]
    public float _pivotSpeed = 0.1f;

    private float _defaultPosition;
    private float _lookAngle;
    private float _pivotAngle;
    [SerializeField]
    public float _minimunPivot = -35;
    [SerializeField]
    public float _maximumPivot = 35;

    private void Awake()
    {
        singleton = this;
        _playerTransform = transform;
        _defaultPosition = _cameraTransform.localPosition.z;
        _ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.Lerp(_playerTransform.position, _targetTransform.position, delta / _followSpeed);
        _playerTransform.position = targetPosition;
    }

    public void HandleCameraRotation(float delta, float mouseXinput, float mouseYInput)
    {
        _lookAngle += (mouseXinput * _lookSpeed) / delta;
        _pivotAngle -= (mouseYInput * _pivotSpeed) / delta;
        _pivotAngle = Mathf.Clamp(_pivotAngle, _minimunPivot, _maximumPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = _lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        _playerTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = _pivotAngle;

        targetRotation = Quaternion.Euler(rotation);
        _cameraPivotTransform.localRotation = targetRotation;
    }
}
