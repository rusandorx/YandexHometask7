using UnityEngine;

// public enum State
// {
//     Idle,
//     Observe
// }

public class Enemy : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float defeatDistance = 3f;

    [SerializeField] private AudioClip _ambient;

    [SerializeField] private AudioClip _scream;
    // [SerializeField] private float _minDistance = 5f;
    // [SerializeField] private float _maxDistance = 20f;

    public float _runSpeed = 10f;

    private GameManager _gameManager;
    private AudioSource _audioSource;

    private Animation _animation;
    // private bool isMoving = false;
    // private State _state = State.Idle;

    public delegate void OnGameOver();

    public OnGameOver onGameOver;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = FindObjectOfType<GameManager>();
        onGameOver += (() => _audioSource.PlayOneShot(_scream));
        _audioSource.clip = _ambient;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    private void Update()
    {
        if (!_gameManager.Playing) return;
        
        var playerPosition = _player.transform.position;
        var toTarget = (playerPosition - transform.position);
        transform.LookAt(new Vector3(playerPosition.x, 0, playerPosition.z));
        if (toTarget.magnitude < defeatDistance)
            onGameOver();
        else
            Run(toTarget);
    }

    private void Run(Vector3 direction)
    {
        _animation.Play("Run");
        Vector3 to = direction.normalized * Time.deltaTime * _runSpeed;
        transform.Translate(new Vector3(to.x, 0, to.z));
    }
}