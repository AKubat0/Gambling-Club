using UnityEngine;

public class DayNightCycleScript : MonoBehaviour
{
    public enum TimeOfDay
    {
        Sunrise,
        Day,
        Sunset,
        Night
    }

    [Header("Time Settings")]
    [SerializeField] private float dayLengthInMinutes = 2f; 
    [SerializeField] private bool isTimeFrozen = false;
    
    [Header("Current Time")]
    [Range(0f, 1f)]
    [Tooltip("0.0 = Midnight, 0.25 = Sunrise, 0.5 = Noon, 0.75 = Sunset")]
    [SerializeField] private float currentTime = 0.25f; 

    [Header("Light References")]
    [SerializeField] private Light sun;
    [SerializeField] private Light moon;

    [Header("Ambient Colors")]
    [SerializeField] private Color dayAmbientColor = new Color(0.2f, 0.2f, 0.25f);
    [SerializeField] private Color nightAmbientColor = new Color(0.05f, 0.05f, 0.1f);

    [Header("Current Time State")]
    [SerializeField] private TimeOfDay currentTimeOfDay;

    void Update()
    {
        if (isTimeFrozen) return;

        currentTime += Time.deltaTime / (dayLengthInMinutes * 60f);
        if (currentTime >= 1f) 
        {
            currentTime = 0f;
        }

        float sunRotationX = (currentTime * 360f) - 90f;
        sun.transform.rotation = Quaternion.Euler(sunRotationX, 0f, 0f);

        if (moon != null)
        {
            moon.transform.rotation = Quaternion.Euler(sunRotationX + 180f, 0f, 0f);
        }

        currentTimeOfDay = CalculateTimeOfDay(currentTime);

        HandleStateBehaviors();
        UpdateEnvironmentLighting();
    }

    TimeOfDay CalculateTimeOfDay(float timePercent)
    {
        if (timePercent >= 0.20f && timePercent < 0.25f) return TimeOfDay.Sunrise;

        if (timePercent >= 0.25f && timePercent < 0.70f) return TimeOfDay.Day;

        if (timePercent >= 0.70f && timePercent < 0.75f) return TimeOfDay.Sunset;

        return TimeOfDay.Night; 
    }

    void HandleStateBehaviors()
    {
        switch (currentTimeOfDay)
        {
            case TimeOfDay.Sunrise:
                // Add sunrise logic here (e.g., turn off streetlights)
                break;
            case TimeOfDay.Day:
                // Add daytime logic here
                break;
            case TimeOfDay.Sunset:
                // Add sunset logic here (e.g., turn on streetlights)
                break;
            case TimeOfDay.Night:
                // Add night logic here (e.g., spawn monsters)
                break;
        }
    }

    void UpdateEnvironmentLighting()
    {
        float dotProduct = Vector3.Dot(sun.transform.forward, Vector3.down);

        if (dotProduct < 0)
        {
            sun.intensity = Mathf.Lerp(sun.intensity, 0f, Time.deltaTime * 2f);
            if (moon != null) moon.intensity = Mathf.Lerp(moon.intensity, 0.3f, Time.deltaTime * 2f);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, nightAmbientColor, Time.deltaTime * 2f);
        }
        else
        {
            sun.intensity = Mathf.Lerp(sun.intensity, 1f, Time.deltaTime * 2f);
            if (moon != null) moon.intensity = Mathf.Lerp(moon.intensity, 0f, Time.deltaTime * 2f);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, dayAmbientColor, Time.deltaTime * 2f);
        }
    }

    public void ChangeDayLength(float newLengthInMinutes)
    {
        dayLengthInMinutes = newLengthInMinutes;
    }
}