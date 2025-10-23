using System.Collections;
using DependencyInjection;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class TimeCycle : MonoBehaviour, IDependencyProvider
{
    [Inject] EntityControls controls;
    [Provide] TimeCycle Provide() => this;
    public static TimeCycle instance;
    public Light dayLight;
    public float nightIntensity = 0f;
    float initialIntensity;

    public UnityEvent OnDayStart;
    public UnityEvent OnNightStart;

    private void Awake()
    {
        instance = this;
        initialIntensity = dayLight.intensity;
    }


    public float currentTime;

    [Title("Fade Settings")]
    [Range(0f, 100f)] public float dayFadeStartPercent = 75f;
    [Range(0f, 100f)] public float nightFadeStartPercent = 75f;

    [Title("Cycle Lengths (Minutes)")]
    public float dayLengthInMinutes = 5f * 60f;
    public float nightLengthInMinutes = 3f * 60f;

    [Title("States")]
    [SerializeField] bool isDay = true;
    public bool timeFrozen = false;
    public bool transitioning = false;

    private void Start()
    {
        isDay = true;
    }

    public void Update()
    {
        if (timeFrozen || transitioning) return;
        currentTime += Time.deltaTime;

        if (isDay) CheckDay();
        if (!isDay) CheckNight();
    }

    void CheckDay()
    {
        FadeDayLightToNight();
        if (currentTime > dayLengthInMinutes * 60)
            Transition();
    }

    void CheckNight()
    {
        FadeNightLightToDay(); 

        if (currentTime > nightLengthInMinutes * 60)
            Transition();
    }

    void Transition()
    {
        FadeScreen fade = controls.TryGet<FadeScreen>();

        fade.FadeInAndOutCallback((isDay) ? SetToNight : SetToDay);
        isDay = !isDay;
        currentTime = 0;
    }


    void SetToNight()
    {
        dayLight.intensity = nightIntensity;
        OnNightStart?.Invoke();
    }

    void SetToDay()
    {
        dayLight.intensity = initialIntensity;
        OnDayStart?.Invoke();
    }

    void FadeDayLightToNight()
    {
        float fadeStart = (dayFadeStartPercent / 100f) * (dayLengthInMinutes * 60f);
        float fadeEnd = dayLengthInMinutes * 60f;

        if (currentTime >= fadeStart)
        {
            float t = Mathf.InverseLerp(fadeStart, fadeEnd, currentTime);
            dayLight.intensity = Mathf.Lerp(initialIntensity, nightIntensity, t);
        }
    }

    void FadeNightLightToDay()
    {
        float fadeStart = (nightFadeStartPercent / 100f) * (nightLengthInMinutes * 60f);
        float fadeEnd = nightLengthInMinutes * 60f;

        if (currentTime >= fadeStart)
        {
            float t = Mathf.InverseLerp(fadeStart, fadeEnd, currentTime);
            dayLight.intensity = Mathf.Lerp(nightIntensity, initialIntensity, t);
        }
    }


}
