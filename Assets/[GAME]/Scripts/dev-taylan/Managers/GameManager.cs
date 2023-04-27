using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieAttack.Exceptions;
using ZombieAttack.Managers;

namespace ZombieAttack.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Variables
        public float zombieMoney;
        public float totalMoney;
        public float zombieMoneyMultiplier;

        public int level = 1;
        
        public bool isGameOver;

        public ZombieData zombieData;

        #endregion

        #region Methods
        private void Start()
        {
            zombieMoney = zombieData.zombieMoney;
        }

        public void IncreaseZombieMoney()
        {
            zombieMoney *= zombieMoneyMultiplier;
        }
        public void IncreaseTotalMoney(float amount)
        {
            totalMoney += amount;
            UIManager.Instance.UpdateMoneyText(totalMoney);
        }



        #endregion
    }

}

