using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ZombieAttack.Exceptions;
using UnityEngine.UI;
using ZombieAttack.Managers;

namespace ZombieAttack.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        #region Variables
        public TextMeshProUGUI _totalMoneyText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _incomeMoneyCostText;
        [SerializeField] private TextMeshProUGUI _mergeMoneyCostText;
        [SerializeField] private TextMeshProUGUI _addSoldierCostText;

        [SerializeField] private Slider _levelBar;

        [SerializeField] private float _incomeMoneyCost;
        [SerializeField] private float _mergeMoneyCost;
        [SerializeField] private float _addSoldierButtonMoneyCost;
        [SerializeField] private float _costIncrease;

        [SerializeField] private ZombieData _zombieData;

        [SerializeField] private ParticleSystem _confetti;

        #region Properties
        public float AddSoldierButtonMoneyCost => _addSoldierButtonMoneyCost;
        public float MergeMoneyCost => _mergeMoneyCost;

        #endregion
        #endregion

        private void Start()
        {
            _totalMoneyText.text = GameManager.Instance.totalMoney.ToString();
        }

        public void UpdateMoneyText(float money)
        {
            _totalMoneyText.text = money.ToString();
            _totalMoneyText.transform.parent.DOKill(true);
            _totalMoneyText.transform.parent.DOShakeScale(.1f, .1f, 1, .1f);


        }
        public void SetMaxValue(int maxValue)
        {
            _levelBar.maxValue = maxValue;
        }
        public void UpdateLevelSlider()
        {
            float fillAmount = (1f / ZombieManager.Instance._numEnemies);
            _levelBar.value += fillAmount;

            if (ZombieManager.Instance._zombiesKilled == ZombieManager.Instance._numEnemies)
            {
                ZombieManager.Instance._zombiesKilled = 0;
                _levelBar.value = 0;
                GameManager.Instance.level++;
                _levelText.text = "LEVEL " + GameManager.Instance.level;
                SetLevelUpConfetti();
            }
        }
        private void SetLevelUpConfetti()
        {
            StartCoroutine(ConfettiOpenClose());

            IEnumerator ConfettiOpenClose()
            {
                _confetti.gameObject.SetActive(true);
                yield return new WaitForSeconds(3);
                _confetti.gameObject.SetActive(false);
            }
        }
        public void SetIncomeMoneyButton()
        {
            if (GameManager.Instance.totalMoney < _incomeMoneyCost) return;

            GameManager.Instance.totalMoney -= _incomeMoneyCost;
            _incomeMoneyCost += _costIncrease;
            _incomeMoneyCostText.text = _incomeMoneyCost.ToString();
            GameManager.Instance.IncreaseZombieMoney();
            UpdateMoneyText(GameManager.Instance.totalMoney);


        }

        public void SetMergeButton()
        {

            if (GameManager.Instance.totalMoney < _mergeMoneyCost || CharacterManager.Instance.IsCanMerge == false) return;

            GameManager.Instance.totalMoney -= _mergeMoneyCost;
            _mergeMoneyCost += _costIncrease;
            _mergeMoneyCostText.text = _mergeMoneyCost.ToString();
            UpdateMoneyText(GameManager.Instance.totalMoney);
        }

        public void SetAddSoldierButton()
        {

            if (GameManager.Instance.totalMoney < _addSoldierButtonMoneyCost) return;


            GameManager.Instance.totalMoney -= _addSoldierButtonMoneyCost;
            _addSoldierButtonMoneyCost += _costIncrease;
            _addSoldierCostText.text = _addSoldierButtonMoneyCost.ToString();
            UpdateMoneyText(GameManager.Instance.totalMoney);

        }
    }

}
