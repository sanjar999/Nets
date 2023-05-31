using System;
using DG.Tweening;
using UnityEngine;

public class SpidyJump : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _fallForwardForce = 3;
    [SerializeField] private float _maxYSpeed = 20;
    private Rigidbody _rb;

    public Rigidbody GetRB() => _rb;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Jump(float relativeForce)
    {
        _rb.AddForce(Vector3.up * _jumpForce * relativeForce, ForceMode.Impulse);
        Physics.gravity = new Vector3(0,-4f,0);

    }

    public void HangOn()
    {
        var fallSpeed = _rb.velocity.y;
        DOTween.To(()=> fallSpeed, x=>  _rb.velocity = new Vector3(0, x, 0), 0, .25f);
        Physics.gravity = Vector3.zero;
    }

    private void Update()
    {
        var clampedY = Mathf.Clamp(_rb.velocity.y, -100f, _maxYSpeed);
        _rb.velocity = new Vector3(0, clampedY, 0);
    }

    public void FallForward()
    {
        GetComponent<Collider>().isTrigger = true;
        _rb.velocity = Vector3.zero;
        _rb.AddForce(new Vector3(0,-1,1)  * _jumpForce * _fallForwardForce, ForceMode.Impulse);
    }
}