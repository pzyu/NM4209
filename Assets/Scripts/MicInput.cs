using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MicInput : MonoBehaviour {

    public static float MicLoudness;

    private string _device;

    private AudioClip _clipRecord;
    
    [SerializeField]
    private float maxVolume = 1.0f;

    [SerializeField]
    private float shoutThreshold = 0.03f;

    [SerializeField]
    private TextMeshPro micDebug;

    public static bool isThresholdBroken = false;

    public static bool isVolumeCalibrated = false;
    
    //mic initialization
    void InitMic() {
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
        micDebug.text = _device + "\nLoudness:" + MicLoudness;
    }

    void StopMicrophone() {
        Microphone.End(_device);
    }
    
    private int position = 0;
    private int samplerate = 44100;
    private float frequency = 440;// = AudioClip.Create("Audio Clip", 44100 * 2, 1, 44100, true);//, OnAudioRead, OnAudioSetPosition);
    int _sampleWindow = 512;

    private float lastWindowPeak = 0;

    //get data from microphone into audioclip
    float LevelMax() {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        
        float totalPeak = 0;
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++) {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak) {
                levelMax = wavePeak;
            }

            // Need to figure out which is sustain and which is a shout
            // Check in the last window, is the difference less than threshold?
            totalPeak += wavePeak;
            if (i == (_sampleWindow - 1)) {
                float avgPeak = totalPeak / _sampleWindow;
                float difference = avgPeak - lastWindowPeak;
                if (avgPeak > 0.01f) {
                    //Debug.Log("Average peak: " + avgPeak + " Difference of :" + difference);
                }

                if (difference > shoutThreshold) {
                    Debug.Log("Threshold broken: " + difference);
                    isThresholdBroken = true;
                } else {
                    isThresholdBroken = false;
                }

                lastWindowPeak = avgPeak;
            }
        }


        //levelMax = levelMax <= 0.001 ? 0 : levelMax;

        //levelMax /= 0.2f;

        levelMax = Mathf.Clamp(levelMax, 0, maxVolume);

        return levelMax;
    }

    float fakeVol = 0.0f;
    float FakeMic() {
        if (Input.GetKey(KeyCode.Space)) {
            fakeVol += 0.01f;
            fakeVol = Mathf.Clamp(fakeVol, 0, maxVolume);
        } else {
            if (fakeVol > 0) {
                fakeVol -= 0.01f;
            }
        }
        
        return fakeVol;
    }


    void Update() {
        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // pass the value to a static var so we can access it from anywhere
        MicLoudness = LevelMax();
        //MicLoudness = FakeMic();
        
        if (MicLoudness > 0.01) {
            //Debug.Log(MicLoudness);
            micDebug.text = _device + "\nLoudness:" + MicLoudness;

            if (GameController.gameControllerInstance.IsGameReadyToStart() && !isVolumeCalibrated) {
                isVolumeCalibrated = true;
                shoutThreshold = 0.02f;// MicLoudness * 0.9f;
            }
        }
    }

    bool _isInitialized;
    // start mic when scene starts
    void OnEnable() {
        InitMic();
        _isInitialized = true;
    }

    //stop mic when loading a new level or quit application
    void OnDisable() {
        StopMicrophone();
    }

    void OnDestroy() {
        StopMicrophone();
    }

    private void Start() {
        if (_device == null) {
            _device = Microphone.devices[0];
            _clipRecord = Microphone.Start(_device, true, 999, 44100);
        }

        if (Microphone.devices.Length == 0) {
            GameController.gameControllerInstance.SetInstructionText("MIC NOT FOUND");
        }
    }


    // make sure the mic gets started & stopped when application gets focused
    void OnApplicationFocus(bool focus) {
        /*
        if (focus) {
            Debug.Log("Focus");

            if (!_isInitialized) {
                Debug.Log("Init Mic");
                InitMic();
                _isInitialized = true;
            }
        }
        if (!focus) {
            Debug.Log("Pause");
            StopMicrophone();
            Debug.Log("Stop Mic");
            _isInitialized = false;

        }
        */
    }
}
