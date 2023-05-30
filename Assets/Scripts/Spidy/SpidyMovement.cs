using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

enum State
{
    idle,
    hang,
    fly,
    fall
}

public class SpidyMovement : MonoBehaviour
{
    [SerializeField] private Transform _spidy;
    [SerializeField] private float _spidyXSpeed = 10f;
    [SerializeField] private Vector2 _xMovementRange = new Vector2(-2, 2);
    [SerializeField] private SpidyJump _spidyJump;
    [SerializeField] private float _maxRelativeJumpForce;
    [SerializeField] private Transform _netCheckRayUp;
    [SerializeField] private Transform _netCheckRayDown;
    [SerializeField] private TMP_Text _jumoForceText;
    private float _startMouseX;
    private float _startMouseY;
    private float _startSpidyX;
    private Camera _cam;

    private State _state;
    private bool _isChecking;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _state == State.idle)
        {
            _state = State.hang;

            var mP = _cam.ScreenToWorldPoint(Input.mousePosition);
            _startMouseX = mP.x;
            _startMouseY = mP.y;
            _startSpidyX = _spidy.position.x;
        }

        if (Input.GetMouseButtonUp(0) && _state == State.hang)
        {
            _state = State.fly;

            var _endMouseY = _cam.ScreenToWorldPoint(Input.mousePosition).y;
            var relativeJumpForce = Mathf.Clamp(_startMouseY - _endMouseY, 0, _maxRelativeJumpForce);
            _jumoForceText.text = relativeJumpForce.ToString();
            _spidyJump.Jump(relativeJumpForce);
        }

        if (Input.GetMouseButton(0))
        {
            var pos = _spidy.position;
            var touchPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            var delta = touchPos.x - _startMouseX;
            var lerpX = Mathf.Lerp(pos.x, _startSpidyX + delta, Time.deltaTime * _spidyXSpeed);
            pos = new Vector3(Mathf.Clamp(lerpX, _xMovementRange.x, _xMovementRange.y), pos.y, pos.z);
            _spidy.position = pos;
        }

        if (_state == State.fly && _spidyJump.GetRB().velocity.y < 0)
        {
            _state = State.fall;
        }
    }

    private void FixedUpdate()
    {
        if (_state == State.fall && !_isChecking)
        {
            _isChecking = true;
            CheckDeath();
        }
    }

    private void CheckDeath()
    {
        Physics.Raycast(_netCheckRayDown.position, _netCheckRayDown.forward, out RaycastHit hit, 10f);
        Physics.Raycast(_netCheckRayUp.position, _netCheckRayUp.forward, out RaycastHit hit1, 10f);
        if ((hit.collider && hit.collider.CompareTag("net")) ||  ( hit1.collider && hit1.collider.CompareTag("net")))
        {
            _state = State.idle;
            _spidyJump.HangOn();
        }
        else
        {
            _spidyJump.FallForward();
            StartCoroutine(FooCo());
        }
        _isChecking = false;
    }

    private IEnumerator FooCo()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene("Game");
    }
}