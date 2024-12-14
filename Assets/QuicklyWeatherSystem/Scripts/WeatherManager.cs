using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

namespace QuicklyWeatherSystem
{
    public class WeatherManager : MonoBehaviour
    {
        private GameObject Weather2D;
        private ParticleSystem rain2DParticleSystem;
        private ParticleSystem storm2DParticleSystem;
        private ParticleSystem snow2DParticleSystem;

        private GameObject Weather3D;
        private ParticleSystem rain3DParticleSystem;
        private ParticleSystem storm3DParticleSystem;
        private ParticleSystem snow3DParticleSystem;
        private Light directionalLight;

        public enum ParticleSystemType { _2D, _3D }
        public ParticleSystemType particleSystemType;

        [Header("Weather API Settings")]
        [SerializeField] private string apiKey = "your open weather api key";
        [SerializeField] private string city = "HongKong";
        [SerializeField] private float updateInterval = 3600f;

        private Camera m_MainCamera;
        private void Start()
        {
            Weather2D = transform.Find("Weather2D").gameObject;
            Weather3D = transform.Find("Weather3D").gameObject;

            rain2DParticleSystem = Weather2D.transform.Find("Rain").GetComponent<ParticleSystem>();
            storm2DParticleSystem = Weather2D.transform.Find("Storm").GetComponent<ParticleSystem>();
            snow2DParticleSystem = Weather2D.transform.Find("Snow").GetComponent<ParticleSystem>();

            rain3DParticleSystem = Weather3D.transform.Find("Rain").GetComponent<ParticleSystem>();
            storm3DParticleSystem = Weather3D.transform.Find("Storm").GetComponent<ParticleSystem>();
            snow3DParticleSystem = Weather3D.transform.Find("Snow").GetComponent<ParticleSystem>();

            directionalLight = transform.Find("Directional Light").GetComponent<Light>();

            StartCoroutine(UpdateWeather());
            if (particleSystemType == ParticleSystemType._2D)
            {
                rain3DParticleSystem.Stop();
                storm3DParticleSystem.Stop();
                snow3DParticleSystem.Stop();
            }
            else
            {
                rain2DParticleSystem.Stop();
                storm2DParticleSystem.Stop();
                snow2DParticleSystem.Stop();
            }
            m_MainCamera = Camera.main;

            // set particle system transform equal to camera transform
            Weather2D.transform.position = m_MainCamera.transform.position;
            Weather2D.transform.rotation = m_MainCamera.transform.rotation;

            Weather3D.transform.rotation = m_MainCamera.transform.rotation;
            directionalLight.transform.SetParent(m_MainCamera.transform);
        }

        private void Update()
        {
            followCamera();
        }

        private IEnumerator UpdateWeather()
        {
            while (true)
            {
                yield return StartCoroutine(GetWeather());
                yield return new WaitForSeconds(updateInterval); // 等待指定時間
            }
        }

        private IEnumerator GetWeather()
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&APPID={apiKey}";
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error retrieving weather data: " + webRequest.error);
                    SetWeather("Clear");
                }
                else
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    ParseWeatherData(jsonResponse);
                }
            }
        }

        private void ParseWeatherData(string jsonResponse)
        {
            Weather weather = JsonUtility.FromJson<Weather>(jsonResponse);
            SetWeather(weather.main);
        }

        private void SetWeather(string weatherType)
        {
            rain3DParticleSystem.Stop();
            storm3DParticleSystem.Stop();
            snow3DParticleSystem.Stop();

            rain2DParticleSystem.Stop();
            storm2DParticleSystem.Stop();
            snow2DParticleSystem.Stop();

            switch (weatherType)
            {
                case "Clear":
                    ResetLighting();
                    break;
                case "Clouds":
                    AdjustLightingForClouds();
                    break;
                case "Rain":
                    if (particleSystemType == ParticleSystemType._2D)
                    {
                        rain2DParticleSystem.Play();
                    }
                    else
                    {
                        rain3DParticleSystem.Play();
                    }
                    AdjustLightingForRain();
                    break;
                case "Thunderstorm":
                    if (particleSystemType == ParticleSystemType._2D)
                    {
                        storm2DParticleSystem.Play();
                        rain2DParticleSystem.Play();
                    }
                    else
                    {
                        storm3DParticleSystem.Play();
                        rain3DParticleSystem.Play();
                    }
                    AdjustLightingForstorm();
                    break;
                case "Snow":
                    if (particleSystemType == ParticleSystemType._2D)
                    {
                        snow2DParticleSystem.Play();
                    }
                    else
                    {
                        snow3DParticleSystem.Play();
                    }
                    AdjustLightingForSnow();
                    break;
                case "Atmosphere":
                    AdjustLightingForAtmosphere();
                    break;
                default:
                    ResetLighting();
                    break;
            }
        }

        private void AdjustLightingForRain()
        {
            directionalLight.intensity = 0.5f;
        }

        private void AdjustLightingForstorm()
        {
            directionalLight.intensity = 0.3f;
        }

        private void AdjustLightingForSnow()
        {
            directionalLight.intensity = 0.7f;
        }

        private void AdjustLightingForClouds()
        {
            directionalLight.intensity = 0.6f;
        }

        private void AdjustLightingForAtmosphere()
        {
            directionalLight.intensity = 0.5f;
        }

        private void ResetLighting()
        {
            directionalLight.intensity = 1.0f;
        }

        private void followCamera()
        {
            var shape = rain3DParticleSystem.shape;
            shape.position = m_MainCamera.transform.position;
            shape = storm3DParticleSystem.shape;
            shape.position = m_MainCamera.transform.position;
            shape = snow3DParticleSystem.shape;
            shape.position = m_MainCamera.transform.position;

            Weather2D.transform.position = m_MainCamera.transform.position;
        }
    }

    [System.Serializable]
    public class Weather
    {
        public string main;
    }
}