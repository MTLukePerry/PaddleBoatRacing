using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCharacterMovement : MonoBehaviour
{
    [SerializeField] private Player _player = Player.None;
    [SerializeField] private bool _isRightSide;
    [SerializeField] private GameObject _forceFrom;

    private List<KeyCode> _characterKeys = new List<KeyCode>();
    private KeyCode _expectedNextMovement;

    private Rigidbody2D _rb;

    [SerializeField] private float _movementSpeed = 80000;
    [SerializeField] private float _manualForcedDeceleration = 0.9989f;

    private bool _leftFootNext = true;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        if (!_isRightSide)
        {
            if (_player == Player.None)
            {
                _characterKeys = new List<KeyCode>() { KeyCode.A, KeyCode.S };
            }
            else if (_player == Player.Player1)
            {
                _characterKeys = new List<KeyCode>() { KeyCode.Z, KeyCode.X };
            }
            else if (_player == Player.Player2)
            {
                _characterKeys = new List<KeyCode>() { KeyCode.LeftArrow, KeyCode.DownArrow };
            }
        }
        else
        {
            if (_player == Player.None)
            {
                _characterKeys = new List<KeyCode>() { KeyCode.K, KeyCode.L };
            }
            else if (_player == Player.Player1)
            {
                _characterKeys = new List<KeyCode>() { KeyCode.N, KeyCode.M };
            }
            else if (_player == Player.Player2)
            {
                _characterKeys = new List<KeyCode>() { KeyCode.Keypad2, KeyCode.Keypad3 };
            }
        }

        _expectedNextMovement = _characterKeys[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(_expectedNextMovement))
        {
            _expectedNextMovement = _characterKeys.Find((k) => { return k != _expectedNextMovement; });
            MoveAndRotateBoat();
        }

        string controllerButton = GetControllerButtonExpected();
        if (Input.GetButtonDown(controllerButton))
        {
            _leftFootNext = !_leftFootNext;
            MoveAndRotateBoat();
        }

        //else if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    _rb.AddForce(-transform.right * (_movementSpeed * 10) * Time.deltaTime);
        //}

        //if (_rb.velocity != new Vector2(0,0))
        //{
        //    _rb.AddForce(-transform.right * (_movementSpeed / 10) * Time.deltaTime);
        //}

        _rb.velocity *= _manualForcedDeceleration;
    }

    private string GetControllerButtonExpected()
    {
        string playerNumber = "";
        if (_player == Player.Player2)
        {
            playerNumber = _isRightSide ? "4" : "3";
        }
        else
        {
            playerNumber = _isRightSide ? "2" : "1";
        }

        if (_leftFootNext)
        {
            //Debug.Log("I am boat " + _player + " and right side is " + _isRightSide + ". My final string I'm waiting for is " + ("LeftFootP" + playerNumber));
            return "LeftFootP" + playerNumber;
        }
        else
        {
            //Debug.Log("I am boat " + _player + " and right side is " + _isRightSide + ". My final string I'm waiting for is " + ("RightFootP" + playerNumber));
            return "RightFootP" + playerNumber;
        }
    }

    void MoveAndRotateBoat()
    {
        if (_rb != null)
        {
            //if (!_isRightSide)
            //{
            //    //_rb.AddTorque(_torqueSpeed * Time.deltaTime);
            //    transform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), 2);
            //}
            //else
            //{
            //    //_rb.AddTorque(-_torqueSpeed * Time.deltaTime);
            //    transform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), -2);
            //}

            //var angle = Vector2.Angle(transform.up, transform.right) / 9;
            //angle = _isRightSide ? -angle : angle;
            var moveDirection = transform.right;
            //transform.position += transform.right * 10;
            _rb.AddForceAtPosition(moveDirection * _movementSpeed * Time.deltaTime, _forceFrom.transform.position);
        }
    }
}

public enum Player
{
    None,
    Player1,
    Player2
}