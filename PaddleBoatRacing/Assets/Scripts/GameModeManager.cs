using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;

    [SerializeField] private TextMesh _timeText;

    [SerializeField] private bool _oneTapEnabled;
    [SerializeField] private bool _oneTapHoldModeEnabled;

    private bool _countTimer = true;
    private float _timer;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BoatCharacterMovement.OneTapMode = _oneTapEnabled;
        BoatCharacterMovement.OneTapAllowHold = _oneTapHoldModeEnabled;
    }

    void Update ()
    {
        if (_countTimer)
        {
            _timer += Time.deltaTime;
        }
        _timeText.text = _timer.ToString();
    }

    public void StopTimer()
    {
        _countTimer = false;
    }

    private void RestartTimer()
    {
        _timer = 0;
    }
}
