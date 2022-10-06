using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _healthBarFilling;
        [SerializeField] private Player _player;
        [SerializeField] private Camera _camera;
        [SerializeField] private TMP_Text _damageCounter;
        [SerializeField] private Transform _damageCounterTransform;
        

        private void Awake()
        {
            _player.OnHealthChanged += DoOnHealthChanged;
        }

        private void OnDestroy()
        {
            _player.OnHealthChanged -= DoOnHealthChanged;
        }

        private void DoOnHealthChanged(float valueAsPercentage)
        {
            _healthBarFilling.fillAmount = valueAsPercentage;
            _damageCounter.text = $"{_player.GetHealth()}";
        }

        private void LateUpdate()
        {
            var camPosition = _camera.transform.position;
            transform.LookAt(new Vector3(transform.position.x, camPosition.y, camPosition.z));
            _damageCounterTransform.LookAt(new Vector3(_damageCounterTransform.position.x, camPosition.y, camPosition.z));
            transform.Rotate(0, 180, 0);
            _damageCounterTransform.Rotate(0, 180, 0);
        }
    }
}