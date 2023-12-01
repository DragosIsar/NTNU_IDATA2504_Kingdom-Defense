using UnityEngine;

namespace _Project.Scripts
{
    public class DayNightCycle : MonoBehaviour
    {
        private Vector3 _rot = Vector3.zero;
        public float rotationSpeedInDeg = 6;

        [SerializeField] private bool isDay = true;
        private Light _light;
        public bool IsDay => isDay;

        private void Start()
        {
            _light = GetComponent<Light>();
        }

        private void Update()
        {
            Cycle();
        }

        private void Cycle()
        {
            _rot.x = rotationSpeedInDeg * Time.deltaTime;
            Transform t;
            (t = transform).Rotate(_rot);

            Quaternion rotation = t.rotation;
            isDay = !(rotation.eulerAngles.x > 90);

            _light.colorTemperature = Mathf.Lerp(1000, 10000, rotation.eulerAngles.x / 135);

            float lightIntensity = !isDay ? 0 : Mathf.Lerp(0, 1, Mathf.Clamp01(rotation.eulerAngles.x / 90));
            _light.intensity = lightIntensity;
        }
    }
}