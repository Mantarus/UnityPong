﻿using Carriage;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject ballPrefab;

    public Text leftText;
    public Text rightText;
    public Text middleText;
    public Text topText;
    public GameObject dimPlane;

    public AudioSource audioSource;
    public AudioClip goalAudio;
    
    public float carriageXPosition = 18;
    public float carriageLimit = 12.5f;
    public float carriageSpeed = 30;

    public float ballInitialSpeed = 10;
    public float ballSpeedIncrement = 2;
    public float ballAngleMultiplier = 1;

    private GameObject _leftPlayer;
    private GameObject _rightPlayer;
    private GameObject _ball;
    private BallMover _ballMover;
    private int _leftPlayerWins = 0;
    private int _rightPlayerWins = 0;
    private string _previousWinner = "Right";

    private bool _paused = true;

    // Start is called before the first frame update
    private void Start()
    {
        InitUi();
        StartGame();
    }

    private void Update()
    {
        Pause();
        if (_ballMover.GetWinner() != null)
        {
            audioSource.clip = goalAudio;
            audioSource.Play();
            
            NewRound();
        }
    }

    private void NewRound()
    {
        UpdateWins();
        UpdateScore();
        ResetObjects();
        StartGame();
    }

    private void StartGame()
    {
        _ball = Instantiate(ballPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        _ballMover = _ball.GetComponent<BallMover>();
        _ballMover.initialSpeed = ballInitialSpeed;
        _ballMover.increment = ballSpeedIncrement;
        _ballMover.angleMultiplier = ballAngleMultiplier;
        _ballMover.initialXDirection = _previousWinner == "Left" ? 1f: -1f;
        
        _leftPlayer = Instantiate(playerPrefab, new Vector3(-carriageXPosition, 1, 0), Quaternion.identity);
        _leftPlayer.GetComponent<CarriageAiMover>().enabled = false;
        var leftPlayerMover = _leftPlayer.GetComponent<CarriagePlayerMover>();
        leftPlayerMover.limit = carriageLimit;
        leftPlayerMover.speed = carriageSpeed;
        
        _rightPlayer = Instantiate(playerPrefab, new Vector3(carriageXPosition, 1, 0), Quaternion.identity);
        _rightPlayer.GetComponent<CarriagePlayerMover>().enabled = false;
        var rightPlayerMover = _rightPlayer.GetComponent<CarriageAiMover>();
        rightPlayerMover.limit = carriageLimit;
        rightPlayerMover.speed = carriageSpeed;
        rightPlayerMover.ball = _ball;
    }
    
    private void UpdateWins()
    {
        if (_ballMover.GetWinner() == "Left")
        {
            _leftPlayerWins++;
        }
        if (_ballMover.GetWinner() == "Right")
        {
            _rightPlayerWins++;
        }
        _previousWinner = _ballMover.GetWinner();
    }

    private void UpdateScore()
    {
        leftText.text = _leftPlayerWins.ToString();
        rightText.text = _rightPlayerWins.ToString();
    }

    private void ResetObjects()
    {
        Destroy(_leftPlayer.gameObject);
        Destroy(_rightPlayer.gameObject);
        Destroy(_ball.gameObject);
    }

    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _paused = !_paused;
            if (_paused)
            {
                Time.timeScale = 0;
                middleText.text = "PAUSED";
                Screen.brightness = 0.5f;
                dimPlane.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                middleText.text = "";
                Screen.brightness = 1f;
                dimPlane.SetActive(false);
            }
        }
    }

    private void InitUi()
    {
        dimPlane.SetActive(false);
        topText.text = "";
        leftText.text = "0";
        rightText.text = "0";
        middleText.text = "";
    }
}
