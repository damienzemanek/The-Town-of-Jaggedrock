using DependencyInjection;
using Unity.VisualScripting;
using UnityEngine;

public class TimeCycle : MonoBehaviour
{
    [Inject] EntityControls controls;
    public static TimeCycle instance;

    private void Awake()
    {
        instance = this;
    }


    public float currentTime;

    public float dayLengthInMinutes = (60f * 5);
    public float nightLenghtInMinutes = (60f * 3);

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
        if (currentTime > dayLengthInMinutes * 60)
            Transition();
    }

    void CheckNight()
    {
        if (currentTime > nightLenghtInMinutes * 60)
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

    }

    void SetToDay()
    {

    }

}
