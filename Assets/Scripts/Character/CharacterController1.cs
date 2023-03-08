using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController1 : MonoBehaviour
{
    [Header("CharacterMove")]
    [SerializeField] float _speed;
    [SerializeField] float _direction;

    [Space]
    [Header("CharacterJump")]
    [SerializeField] float _jumpForce;
    [SerializeField] float _maxHoldJump = .5f;
    [SerializeField] float _holdingTime = 0f;
    [SerializeField] bool _isExitJumpBtn;
    [SerializeField] Rigidbody2D _rb2D { get => GetComponent<Rigidbody2D>(); }
    [Space]
    [Header("RayCast")]
    [SerializeField] float _rayDistance = .1f;
    [SerializeField] LayerMask _groundCheckLayer;
    bool _isGrounded;
    private void Start()
    {
        _rb2D.freezeRotation = true;
    }

    private void Update()
    {
        CharacterMove();
        CharacterJump();
        Flip();
        WallCheckLeft();
        WallSlide();
    }
    void CharacterMove()
    {
        //Burdaki Bugi duzelt;
            _direction = Input.GetAxisRaw("Horizontal");
            _rb2D.velocity = new Vector2(_direction * _speed, _rb2D.velocity.y);
    }
    void WallJump()
    {
        //Burada WallJump olacaq
    }
    void CharacterJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _holdingTime = _maxHoldJump;
            Jump();
            _isExitJumpBtn = false;
        }
        if (Input.GetButtonUp("Jump"))
        {
            _isExitJumpBtn = true;
        }
        if (Input.GetButton("Jump") && !_isGrounded && _holdingTime > 0 && !_isExitJumpBtn)
        {
            _holdingTime -= Time.deltaTime;
            Jump();
        }
    }
    void Flip()
    {
        if (_direction != 0) transform.localScale = new Vector2(_direction, transform.localScale.y);
    }
    void Jump()
    {
        _rb2D.velocity = new Vector2(_rb2D.velocity.x, _jumpForce);
    }
    void WallSlide()
    {
        if ((WallCheckLeft() || WallCheckRight()) && !_isGrounded)
        {
            _rb2D.velocity += new Vector2(_rb2D.velocity.x, -.1f * Time.deltaTime);
        }
    }
    bool WallCheckLeft()
    {
        Vector3 rayOrigin = transform.position - Vector3.right * (transform.localScale.x / transform.localScale.x / 2);
        RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, Vector2.left, _rayDistance, _groundCheckLayer);
        if (hit2D.collider != null)
        {
            return true;
        }
        return false;

    }
    bool WallCheckRight()
    {
        Vector3 rayOrigin = transform.position - Vector3.left * (transform.localScale.x / transform.localScale.x / 2);
        RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, Vector2.right, _rayDistance, _groundCheckLayer);
        if (hit2D.collider != null)
        {
            return true;
        }
        return false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

}
