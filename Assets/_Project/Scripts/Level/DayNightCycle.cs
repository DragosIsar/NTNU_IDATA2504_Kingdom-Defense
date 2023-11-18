using UnityEngine;

namespace _Project.Scripts
{
    public class DayNightCycle : MonoBehaviour
    {
        private GameObject directionalLight;

        private Vector3 _rot = Vector3.zero;
        public float rotationSpeedInDeg = 6;

        [SerializeField] private bool isDay = true;
        public bool IsDay => isDay;

        private void Start()
        {
            directionalLight = GameObject.Find("Directional Light");
        }

        private void Update()
        {
            Cycle();
        }

        private void Cycle()
        {
            _rot.x = rotationSpeedInDeg * Time.deltaTime;
            directionalLight.transform.Rotate(_rot);

            isDay = !(directionalLight.transform.rotation.eulerAngles.x > 90);
        }
    }
}