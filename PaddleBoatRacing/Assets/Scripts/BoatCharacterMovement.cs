using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCharacterMovement : MonoBehaviour
{
    public static bool OneTapMode = false;
    public static bool OneTapAllowHold = false;

    [SerializeField] private Player _player = Player.None;
    [SerializeField] private bool _isRightSide;
    [SerializeField] private GameObject _forceFrom;

    private List<KeyCode> _characterKeys = new List<KeyCode>();
    private KeyCode _expectedNextMovement;

    private Rigidbody2D _rb;

    [SerializeField] private float _movementSpeed = 80000;
    [SerializeField] private float _manualForcedDeceleration = 0.9989f;

    [Header("One Tap Vars")]
    [SerializeField] private float _movementSpeedDecreaseOnHoldMode = 50000f;

    private bool _leftFootNext = true;
    private bool _oneTapAwaitingStillness = false;

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
            else if (_player == Player.Player3)
            {
                _characterKeys = new List<KeyCode>() { KeyCode.LeftArrow, KeyCode.DownArrow };
            }
            else if (_player == Player.Player4)
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
            else if (_player == Player.Player3)
            {
                _characterKeys = new List<KeyCode>() { KeyCode.Keypad2, KeyCode.Keypad3 };
            }
            else if (_player == Player.Player4)
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
            if (!OneTapMode)
            {
                _expectedNextMovement = _characterKeys.Find((k) => { return k != _expectedNextMovement; });
            }
            MoveAndRotateBoat();
        }

        if (!OneTapMode)
        {
            float triggerUsed = Input.GetAxis("FeetP" + GetPlayerNumber());
            string controllerButton = GetControllerButtonExpected();
            if (Input.GetButtonDown(controllerButton))
            {
                _leftFootNext = !_leftFootNext;
                MoveAndRotateBoat();
            }
            else if (triggerUsed != 0)
            {
                if (triggerUsed > 0 && _leftFootNext || triggerUsed < 0 && !_leftFootNext)
                {
                    _leftFootNext = !_leftFootNext;
                    MoveAndRotateBoat();
                }
            }
        }
        else
        {
            if (!_isRightSide)
            {
                float triggerUsed = Input.GetAxis("LeftPaddleP" + GetPlayerNumberOneTap());
                if (triggerUsed > 0.2)
                {
                    if (!_oneTapAwaitingStillness || OneTapAllowHold)
                    {
                        _oneTapAwaitingStillness = true;
                        MoveAndRotateBoat();
                    }
                }
                else
                {
                    _oneTapAwaitingStillness = false;
                }
            }
            else
            {
                float triggerUsed = Input.GetAxis("RightPaddleP" + GetPlayerNumberOneTap());
                if (triggerUsed > 0.2)
                {
                    if (!_oneTapAwaitingStillness || OneTapAllowHold)
                    {
                        _oneTapAwaitingStillness = true;
                        MoveAndRotateBoat();
                    }
                }
                else
                {
                    _oneTapAwaitingStillness = false;
                }
            }
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
        string playerNumber = GetPlayerNumber();

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

    private string GetPlayerNumber()
    {
        string playerNumber = "0";
        if (_player == Player.Player4)
        {
            playerNumber = _isRightSide ? "7" : "8";
        }
        else if (_player == Player.Player3)
        {
            playerNumber = _isRightSide ? "5" : "6";
        }
        else if (_player == Player.Player2)
        {
            playerNumber = _isRightSide ? "4" : "3";
        }
        else
        {
            playerNumber = _isRightSide ? "2" : "1";
        }
        return playerNumber;
    }

    private string GetPlayerNumberOneTap()
    {
        string playerNumber = "0";
        if (_player == Player.Player4)
        {
            playerNumber = "4";
        }
        else if (_player == Player.Player3)
        {
            playerNumber = "3";
        }
        else if (_player == Player.Player2)
        {
            playerNumber = "2";
        }
        else
        {
            playerNumber = "1";
        }
        return playerNumber;
    }

    void MoveAndRotateBoat()
    {
        if (_rb != null)
        {
            var moveDirection = transform.right;
            float movementPower = _movementSpeed;
            if (OneTapAllowHold)
            {
                movementPower -= _movementSpeedDecreaseOnHoldMode;
            }

            _rb.AddForceAtPosition(moveDirection * movementPower * Time.deltaTime, _forceFrom.transform.position);
        }
    }
}

public enum Player
{
    None,
    Player1,
    Player2,
    Player3,
    Player4
}