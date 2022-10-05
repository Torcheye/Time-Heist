using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public TMP_Text text;
    public bool forward { get; private set; }
    public Volume volume;
    
    private ColorAdjustments _adjustments;
    private float _time;
    private TimeActor[] _timeActors;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        volume.sharedProfile.TryGet(out _adjustments);
        _adjustments.hueShift.value = 0;
        _timeActors = FindObjectsOfType<TimeActor>();
    }
    
    private void Start()
    {
        _time = 0;
        forward = true;
    }

    private void Update()
    {
        _time += forward ? Time.deltaTime : -Time.deltaTime;
        if (_time >= 10)
        {
            forward = false;
            DOTween.To(() => _adjustments.hueShift.value, x => _adjustments.hueShift.value = x,
                180, .5f);
            GameManager.Instance.TryReversePlay(true);
            
            foreach (var t in _timeActors)
                t.OnChangeTimeDirection(false);
        }

        if (_time <= 0)
        {
            forward = true;
            DOTween.To(() => _adjustments.hueShift.value, x => _adjustments.hueShift.value = x,
                0, .5f);
            GameManager.Instance.TryReversePlay(false);
            
            foreach (var t in _timeActors)
                t.OnChangeTimeDirection(true);
        }
        
        int intPart = Mathf.FloorToInt(_time);
        int decimalPart = Mathf.FloorToInt((_time - intPart) * 100);

        text.text = $"{intPart:00}:{decimalPart:00}";
    }
}
