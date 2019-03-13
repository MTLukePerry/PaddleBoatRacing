using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
