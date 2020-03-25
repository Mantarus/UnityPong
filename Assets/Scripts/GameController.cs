using Carriage;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject ballPrefab;

    public Text leftText;
    public Text rightText;

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

    // Start is called before the first frame update
    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
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
}
