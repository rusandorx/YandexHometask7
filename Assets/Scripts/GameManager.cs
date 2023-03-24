using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Enemy _enemy;

    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private HorizontalLayoutGroup _flashlightBatteryUi;
    [SerializeField] private int _oneBatteryLifeTime = 50;

    [SerializeField] private Image _defeatScreen;
    [SerializeField] private Image _winScreen;


    private List<Transform> _batteries;
    private int _radioCollected = 0;
    private int _radioNeeded = 5;
    public bool Playing { get; private set; } = false;

    private void RenderRadio() => countText.text = $"{_radioCollected}\\{_radioNeeded}";

    private void Start() => RenderRadio();

    private void Awake()
    {
        _batteries = _flashlightBatteryUi.GetComponentsInChildren<Transform>()
            .Where(child => child.gameObject != _flashlightBatteryUi.gameObject).ToList();
        _enemy.onGameOver += GameOver;
    }

    private IEnumerator ReduceBatteryCharge()
    {
        while (_batteries.Count > 0)
        {
            yield return new WaitForSeconds(_oneBatteryLifeTime);
            _batteries[0].gameObject.SetActive(false);
            _batteries.Remove(_batteries[0]);
        }

        _player._flashlight.gameObject.SetActive(false);
        _enemy._runSpeed += 4f;
    }

    public void CollectRadio()
    {
        _radioCollected++;
        RenderRadio();
        if (_radioCollected >= _radioNeeded)
            Win();
    }

    private void Win()
    {
        _winScreen.gameObject.SetActive(true);
        Destroy(_enemy);
    }

    private void GameOver()
    {
        _defeatScreen.gameObject.SetActive(true);
        Playing = false;
    }

    public void StartPlaying()
    {
        Playing = true;
        StartCoroutine(nameof(ReduceBatteryCharge));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}