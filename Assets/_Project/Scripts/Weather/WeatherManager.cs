using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;

public class WeatherManager : MonoBehaviour
{
    [Header("Light settings")]
    [SerializeField] private GameObject sunPivot;
    [SerializeField] private float sunRotationAngle_Cloudy = 15f;
    [SerializeField] private float sunRotationAngle_Sunny = 15f;
    [SerializeField] private Light mainLight;
    [SerializeField] private Light horrorLight;
    [SerializeField] private float minAngle = 0f;
    [SerializeField] private float maxAngle = 90f;
    [SerializeField] private float rotationDuration = 20f;

    [Header("Skybox settings")]
    [SerializeField] private Material nightSkybox;
    [SerializeField] private Material cloudySkybox;
    [SerializeField] private Material sunnySkybox;

    [Header("Light intensity")]
    [SerializeField] private float minIntensity = 0.2f;
    [SerializeField] private float maxIntensity = 1.0f;

    [Header("BoatsSettings")]
    [SerializeField] private GameObject normalBoat;
    [SerializeField] private GameObject horrorBoat;
    [SerializeField] private float horrorLightsDuration = 0.5f;
    [SerializeField][Tooltip("The higher the number, the less chances of it happening")][Range(2f, 100f)]
    private int randomHorrorChance = 11; 

    private Material _currentSkybox;
    private void Start()
    {
        StartCoroutine(SolarCycle());
        StartCoroutine(RandomHorror());
    }

    private IEnumerator SolarCycle()
    {
        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.PingPong(elapsed / rotationDuration, 1f); // va avanti e indietro tra 0 e 1

            // Calcola angolo e intensità
            float angle = Mathf.Lerp(minAngle, maxAngle, t);
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            // Applica rotazione e intensità
            mainLight.transform.rotation = Quaternion.Euler(angle, 0f, 0f);
            mainLight.intensity = intensity;

            // Aggiorna skybox in base all’angolo
            if (angle < 30f)
                SetSkybox(nightSkybox);
            else if (angle < 60f)
                SetSkybox(cloudySkybox);
            else
                SetSkybox(sunnySkybox);

            GenerateLight(); // forza aggiornamento GI

            yield return null;
        }
    }

    private void SetSkybox(Material newSkybox)
    {
        if (_currentSkybox != newSkybox)
        {
            RenderSettings.skybox = newSkybox;
            _currentSkybox = newSkybox;            

            switch (newSkybox)
            {
                case Material m when m == nightSkybox:
                    if (sunPivot.activeSelf) sunPivot.SetActive(false);
                    RenderSettings.ambientIntensity = 0.2f;
                    break;
                case Material m when m == cloudySkybox:
                    if (!sunPivot.activeSelf) sunPivot.SetActive(true);
                    sunPivot.transform.Rotate(Vector3.right, sunRotationAngle_Cloudy); // slight adjustment for cloudy
                    RenderSettings.ambientIntensity = 0.5f;
                    break;
                case Material m when m == sunnySkybox:
                    if (!sunPivot.activeSelf) sunPivot.SetActive(true);
                    sunPivot.transform.Rotate(Vector3.left, sunRotationAngle_Sunny); //this should correct the angle for sunny
                    RenderSettings.ambientIntensity = 1.0f;
                    break;
            }

            GenerateLight();
        }
    }
    private void GenerateLight()
    {
        DynamicGI.UpdateEnvironment();
    }

    private void ToggleHorrorLights()
    {
        normalBoat.SetActive(!normalBoat.activeSelf);
        horrorBoat.SetActive(!normalBoat.activeSelf);

        mainLight.gameObject.SetActive(normalBoat.activeSelf);
        horrorBoat.gameObject.SetActive(horrorBoat.activeSelf);
    }

    public IEnumerator RandomHorror()
    {
        while (true) 
        {
            int randomValue = Random.Range(0, randomHorrorChance);
            if (randomValue == 2)
            {
                ToggleHorrorLights();
                yield return new WaitForSeconds(horrorLightsDuration);
                ToggleHorrorLights();
            }
            else yield return new WaitForSeconds(randomValue);
        }
    }
}