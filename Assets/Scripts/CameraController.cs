using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Vector2 _xBounds = Vector2.zero;
	[SerializeField] private Vector2 _zBounds = Vector2.zero;
	[SerializeField] private Vector2 _yBounds = Vector2.zero;
	[SerializeField] private float _dragSpeed = 0;
	[SerializeField] private float _panningSpeed = 0;

	[SerializeField] private PlayerController _player = null;

	private bool _draggingCamera = false;
	private bool _panningCamera = false;
	private Vector3 _previousPosition;
	private Camera _camera;
	private Transform _cameraPivot;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
		_cameraPivot = transform.parent.GetComponent<Transform>();
	}

	private void Update()
	{
		if (_player.BuildingSelected)
		{
			_panningCamera = false;
			_draggingCamera = false;
			return;
		}

		if (!_panningCamera)
			DragCamera();

		if (!_draggingCamera)
			PannCamera();

		ZoomCamera();
	}

	private void PannCamera()
	{
		if (Input.GetMouseButtonDown(2))
		{
			_panningCamera = true;
			_previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
		}
		else if (Input.GetMouseButtonUp(2))
		{
			_panningCamera = false;
		}
	}

	private void LateUpdate()
	{
		if (_player.BuildingSelected)
			return;

		if (_draggingCamera)
		{
			Vector3 mouseDelta = _camera.ScreenToViewportPoint(Input.mousePosition) - _previousPosition;

			var forward = _cameraPivot.forward;
			var right = _cameraPivot.right;

			forward.y = 0f;
			right.y = 0f;
			forward.Normalize();
			right.Normalize();

			Vector3 newPos = _cameraPivot.position - (forward * mouseDelta.y + right * mouseDelta.x) * _dragSpeed * Time.deltaTime;
			newPos.x = Mathf.Clamp(newPos.x, _xBounds.x, _xBounds.y);
			newPos.z = Mathf.Clamp(newPos.z, _zBounds.x, _zBounds.y);
			_cameraPivot.position = newPos;

			_previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
		}

		if (_panningCamera)
		{
			Vector3 mouseDelta = _camera.ScreenToViewportPoint(Input.mousePosition) - _previousPosition;

			_cameraPivot.Rotate(Vector3.up, mouseDelta.x * _panningSpeed * Time.deltaTime);

			_previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
		}
	}

	private void ZoomCamera()
	{
		if (Input.mouseScrollDelta.y == 0)
			return;
		else if (Input.mouseScrollDelta.y > 0 && _camera.transform.position.y < _yBounds.x)
			return;
		else if (Input.mouseScrollDelta.y < 0 && _camera.transform.position.y > _yBounds.y)
			return;

		_camera.transform.position += _camera.transform.forward * Input.mouseScrollDelta.y;
	}

	private void DragCamera()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			_draggingCamera = true;
			_previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
		}
		else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
		{
			_draggingCamera = false;
		}
	}

    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(_xBounds.x, _yBounds.x, _zBounds.x), new Vector3(_xBounds.y, _yBounds.x, _zBounds.x));
		Gizmos.DrawLine(new Vector3(_xBounds.x, _yBounds.x, _zBounds.y), new Vector3(_xBounds.y, _yBounds.x, _zBounds.y));
		Gizmos.DrawLine(new Vector3(_xBounds.x, _yBounds.x, _zBounds.x), new Vector3(_xBounds.x, _yBounds.x, _zBounds.y));
		Gizmos.DrawLine(new Vector3(_xBounds.y, _yBounds.x, _zBounds.x), new Vector3(_xBounds.y, _yBounds.x, _zBounds.y));

		Gizmos.DrawLine(new Vector3(_xBounds.x, _yBounds.y, _zBounds.x), new Vector3(_xBounds.y, _yBounds.y, _zBounds.x));
		Gizmos.DrawLine(new Vector3(_xBounds.x, _yBounds.y, _zBounds.y), new Vector3(_xBounds.y, _yBounds.y, _zBounds.y));
		Gizmos.DrawLine(new Vector3(_xBounds.x, _yBounds.y, _zBounds.x), new Vector3(_xBounds.x, _yBounds.y, _zBounds.y));
		Gizmos.DrawLine(new Vector3(_xBounds.y, _yBounds.y, _zBounds.x), new Vector3(_xBounds.y, _yBounds.y, _zBounds.y));

		Gizmos.DrawLine(new Vector3(_xBounds.x, _yBounds.x, _zBounds.x), new Vector3(_xBounds.x, _yBounds.y, _zBounds.x));
		Gizmos.DrawLine(new Vector3(_xBounds.x, _yBounds.x, _zBounds.y), new Vector3(_xBounds.x, _yBounds.y, _zBounds.y));
		Gizmos.DrawLine(new Vector3(_xBounds.y, _yBounds.x, _zBounds.x), new Vector3(_xBounds.y, _yBounds.y, _zBounds.x));
		Gizmos.DrawLine(new Vector3(_xBounds.y, _yBounds.x, _zBounds.y), new Vector3(_xBounds.y, _yBounds.y, _zBounds.y));
	}
}
