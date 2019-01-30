using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;

    [SerializeField] private TextMesh _timeText;

    private bool _countTimer = true;
    private float _timer;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

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
