using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _camStartMoveOffset = 0.5f;
    [SerializeField] private float _camSpeed = 1;

    private Camera _cam;

    private float _offsetPoint;
    private float _camRelativeSpeed;

    private void Start()
    {
        _cam = Camera.main;
        _offsetPoint = Screen.height * _camStartMoveOffset;
    }

    private void FixedUpdate()
    {
        var targetYPos = _cam.WorldToScreenPoint(_target.position).y;

        if (targetYPos > _offsetPoint)
            MoveUp(targetYPos);
    }

    private void MoveUp(float targetYPos)
    {
        var diff = targetYPos - _offsetPoint;
       _camRelativeSpeed = diff / (Screen.height - _offsetPoint);
        transform.position += _camRelativeSpeed * _camSpeed * Time.fixedDeltaTime * new Vector3(0, 1, 0);
    }
}