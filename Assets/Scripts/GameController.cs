using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static GameController gameControllerInstance;

    [SerializeField]
    private GameObject cursor;
    [SerializeField]
    private GameObject ground;

    [SerializeField]
    public float speed;

    [SerializeField]
    public TextMeshProUGUI uiText;

    [SerializeField]
    public TextMeshPro playText;

    [SerializeField]
    public TextMeshPro distText;

    [SerializeField]
    public TextMeshPro instructionsText;

    [SerializeField]
    private ShaderEffect_CorruptedVram shaderEffect;

    private float initSpeed;
    private float micSpeed;

    public float boostSpeed;

    private float initPosition;
    private int currentPosition;

    public bool isGamePaused = true;

    private void Awake() {
        if (gameControllerInstance == null) {
            gameControllerInstance = this;
        } else {
            Destroy(gameObject);
        }

        initSpeed = speed;

        isGamePaused = true;
        playText.text = "PAUSE ||";
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        if (MicInput.MicLoudness >= 0.01f && micSpeed <= 4.0f) {
            micSpeed += MicInput.MicLoudness * 2;
        } else {
            if (micSpeed > 0) {
                micSpeed -= 0.1f;
            }
        }

        if (boostSpeed > 0) {
            boostSpeed -= 0.05f;
        }

        speed = initSpeed + micSpeed + boostSpeed;
        HandleMovement();

        if (isGamePaused && MicInput.MicLoudness >= 0.01f && IsGameReadyToStart()) {
            StartGame();
        }
    }

    public void StartGame() {
        isGamePaused = false;
        playText.DOText("PLAY >", 0.5f, true, ScrambleMode.All);
        instructionsText.DOText("", 0.5f, true, ScrambleMode.All);

        initPosition = cursor.transform.position.x;
    }

    public void PauseGame() {
        isGamePaused = true;
        playText.DOText("PAUSE ||", 0.5f, true, ScrambleMode.All);
        
        instructionsText.DOText("PLAYER CORRUPT", 1.0f, true, ScrambleMode.All);

        instructionsText.DOText("SHOUT TO START", 1.0f, true, ScrambleMode.All).SetDelay(5.0f).OnComplete(()=> {
            DOTween.To(() => shaderEffect.shift, x => shaderEffect.shift = x, 0, 1);
            MicInput.isVolumeCalibrated = false;
        });

        initPosition = cursor.transform.position.x;
        
        DOTween.To(() => shaderEffect.shift, x => shaderEffect.shift = x, 3, 1).SetEase(Ease.Flash, 10);
    }

    private void HandleMovement() {
        cursor.transform.position += transform.right * Time.deltaTime * speed;

        if (!isGamePaused) {
            currentPosition = (int)(cursor.transform.position.x - initPosition);

            uiText.text = currentPosition.ToString() + "m travelled";

            distText.text = "SPD:" + speed.ToString("f1") + " - DST:" + currentPosition.ToString();
        }
    }

    public void SetInstructionText(string text) {
        instructionsText.DOText(text, 0.5f, true, ScrambleMode.All);
    }
    
    public bool IsGameReadyToStart() {
        return isGamePaused && instructionsText.text == "SHOUT TO START";
    }
}
