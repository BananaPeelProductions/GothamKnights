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
    private Vector3 _cameraFollowVelocity = Vector3.zero;

    public static CameraHandlerBehavior singleton;

    [SerializeField]
    public float _lookSpeed = 0.1f;
    [SerializeField]
    public float _followSpeed = 0.1f;
    [SerializeField]
    public float _pivotSpeed = 0.1f;

    private float _targetPosition;
    private float _defaultPosition;
    private float _lookAngle;
    private float _pivotAngle;
    [SerializeField]
    public float _minimunPivot = -35;
    [SerializeField]
    public float _maximumPivot = 35;

    public float _cameraSphereRadius = 0.2f;
    public float _cameraCollisionOffSet = 0.2f;
    public float _minimumCollisionOffSet = 0.2f;

    private void Awake()
    {
        singleton = this;
        _playerTransform = transform;
        _defaultPosition = _cameraTransform.localPosition.z;
        _ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(_playerTransform.position, _targetTransform.position, ref _cameraFollowVelocity, delta / _followSpeed);
        _playerTransform.position = targetPosition;

        HandleCameraCollision(delta);
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

    private void HandleCameraCollision(float delta)
    {
        _targetPosition = _defaultPosition;
        RaycastHit hit;
        Vector3 direction = _cameraTransform.position - _cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(_cameraPivotTransform.position, _cameraSphereRadius, direction, out hit, Mathf.Abs(_targetPosition), _ignoreLayers))
        {
            float dis = Vector3.Distance(_cameraPivotTransform.position, hit.point);
            _targetPosition = -(dis - _cameraCollisionOffSet);
        }

        if (Mathf.Abs(_targetPosition) < _minimumCollisionOffSet)
        {
            _targetPosition = -_minimumCollisionOffSet;
        }

        _cameraTransformPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, _targetPosition, delta / 0.2f);
        _cameraTransform.localPosition = _cameraTransformPosition;
    }
}
