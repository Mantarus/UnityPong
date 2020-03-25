using UnityEngine;
using Random = UnityEngine.Random;

public class BallMover : MonoBehaviour
{
    public float initialXDirection;
    public float initialSpeed;
    public float increment;
    public float angleMultiplier;

    public AudioSource audioSource;
    public AudioClip bounceCarriageAudio;
    public AudioClip bounceWallAudio;

    private Vector3 _speed;
    private string _winner;

    // Start is called before the first frame update
    private void Start()
    {
        _speed = GetInitialSpeed();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _speed = new Vector3(_speed.x, _speed.y, -_speed.z);
            
            audioSource.clip = bounceWallAudio;
            audioSource.Play();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var angle = (transform.position - other.transform.position).normalized;
            var magnitude = _speed.magnitude;
            var newAngle = angle + _speed.normalized;
            _speed = new Vector3(-_speed.x, _speed.y, 
                _speed.z + angle.z * angleMultiplier * Mathf.Abs(_speed.x)).normalized * (magnitude + increment);
            // _speed = newAngle * (magnitude + increment);

            audioSource.clip = bounceCarriageAudio;
            audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Field")) return;
        var xDistance = (transform.position - other.transform.position).x;
        _winner = xDistance < 0 ? "Right" : "Left";
    }

    private Vector3 Move()
    {
        var position = transform.position;
        return new Vector3(position.x + _speed.x * Time.deltaTime, position.y, position.z + _speed.z * Time.deltaTime);
    }

    private Vector3 GetInitialSpeed()
    {
        return new Vector3(initialXDirection, 0, Random.Range(-0.5f, 0.5f)).normalized * initialSpeed;
    }

    public string GetWinner()
    {
        return _winner;
    }
}
