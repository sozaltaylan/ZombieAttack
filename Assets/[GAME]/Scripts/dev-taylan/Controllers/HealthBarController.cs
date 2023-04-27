using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    private float _fillAmount;
    private Canvas _canvas;
    private Quaternion _originalRotation;
    

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
    }
    private void Start()
    {
        _originalRotation = transform.rotation;
    }
    private void LateUpdate()
    {
        _canvas.gameObject.transform.rotation = _originalRotation;
        _canvas.transform.LookAt(CameraManager.Instance.transform.position);
    }
    public void UpdateHealthBar(float health, float maxHealth)
    {
        gameObject.SetActive(true);
        _fillAmount = health / maxHealth;
        _healthBar.fillAmount = _fillAmount;

    }
}
