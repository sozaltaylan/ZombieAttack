using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using ZombieAttack.Managers;
using ZombieAttack.Controllers;

public class BaseZombie : MonoBehaviour
{
    #region Variables
    [SerializeField] protected List<BaseCharacter> _charachterList;

    [SerializeField] protected NavMeshAgent _navMeshAgent;
    [SerializeField] protected AnimationController _animationController;
    [SerializeField] protected EarnedMoneyController _earnedMoneyController;
    [SerializeField] protected HealthBarController _healthBarController;
    [SerializeField] protected ZombieData _zombieData;
    [SerializeField] protected BoxCollider _collider;

    protected float _zombieHealth;
    protected float _distance;
    protected float _nearestDistance = float.MaxValue;
    protected float _zombieAttackPower;

    [SerializeField] protected GameObject _nearestCharacter;

    [SerializeField] protected TextMeshProUGUI _damageText;

    protected bool isAttack;


    [SerializeField] protected bool isGameOver;

    [SerializeField] protected GameObject hitBloodParticle;
    [SerializeField] protected GameObject deadBloodParticle;

    public bool IsGameOver => isGameOver;

    #region Properties

    public float ZombieAttackPower => _zombieAttackPower;
    #endregion
    #endregion

    #region Methods 
    protected void Start()
    {
        HandleStart();
    }
    protected void Update()
    {
        HandleUpdate();
    }

    protected virtual void HandleStart()
    {
        SetZombieArise(transform);
        _zombieHealth = _zombieData.zombieMaxHealth;
        _zombieAttackPower = _zombieData.zombieAttackPower;
    }
    protected virtual void HandleUpdate()
    {
        SetTarget(transform, _navMeshAgent.speed);
    }
    protected virtual void SetTarget(Transform transform, float zombieSpeed)
    {
        _charachterList = CharacterManager.Instance.GetCharacterList();

        if (!_nearestCharacter)
        {
            float nearDist = Mathf.Infinity;

            for (int i = 0; i < _charachterList.Count; i++)
            {
                var targetPos = _charachterList[i].transform.position;
                targetPos.y = transform.position.y;

                var distCh = Vector3.Distance(transform.position, targetPos);

                if (distCh < nearDist)
                {
                    nearDist = distCh;
                    _nearestCharacter = _charachterList[i].gameObject;
                    gameObject.transform.DOLookAt(_nearestCharacter.transform.position, 0.5f);
                }
            }

            return;
        }


        if (!isAttack)
        {
            _navMeshAgent.speed = zombieSpeed;
            _navMeshAgent.destination = _nearestCharacter.transform.position;

            var pos = _nearestCharacter.transform.position;
            pos.y = transform.position.y;

            var dist = Vector3.Distance(transform.position, pos);

            if (dist < _zombieData.zombieAttackDistance)
            {
                isAttack = true;
                _navMeshAgent.isStopped = true;
            }

        }
        else
        {
            SetAttack();
        }

    }
    protected virtual void SetAttack()
    {
        _animationController.OnZombieAttackAnimation(true);
    }

    protected virtual void SetZombieArise(Transform transform)
    {
        transform.DOMoveY(1, 0.2f, false);
    }
    protected virtual void SetDead()
    {
        
        _collider.enabled = false;
        ZombieManager.Instance.IncreaseKilledZombie();
        UIManager.Instance.UpdateLevelSlider();
        _earnedMoneyController.SetEarnedMoneyText(true, GameManager.Instance.zombieMoney);
        GameManager.Instance.IncreaseTotalMoney(GameManager.Instance.zombieMoney);
        _navMeshAgent.isStopped = true;
        _animationController.OnDeathAnimation(true);
        ZombieManager.Instance.RemoveZombie(this);
        Destroy(this.gameObject,3);
        isGameOver = true;
        Instantiate(deadBloodParticle, this.transform.position, Quaternion.identity);
    }

    protected virtual void SetHealth(float damage)
    {
        _zombieHealth -= damage;
        _healthBarController.UpdateHealthBar(_zombieHealth,_zombieData.zombieMaxHealth);
        if (_zombieHealth <= 0) SetDead();
    }

    protected virtual void CreateDamageText(float damage, Vector3 pos)
    {
        var damagePrefab = Instantiate(_damageText,pos, Quaternion.identity);
        _damageText.gameObject.SetActive(true);
        _damageText.text ="-"+damage.ToString();
        _damageText.gameObject.transform.DOLocalMoveY(1, 1, false);
        _damageText.DOFade(0, 1);
        Destroy(damagePrefab, 1);
    }

    protected virtual void CreateBloodParticle(Vector3 pos)
    {
        Instantiate(hitBloodParticle, pos, Quaternion.identity);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BaseWeapon weapon))
        {
            SetHealth(weapon.weaponData.attackPower);
            CreateDamageText(weapon.weaponData.attackPower, weapon.transform.position);
            CreateBloodParticle(weapon.transform.position);
            if (!weapon.IsWeaponCloseRange)
            {
                weapon.gameObject.SetActive(false);
            }

        }
        if (other.gameObject.TryGetComponent(out BaseCharacter character))
        {
            
            character.SetHealth(_zombieAttackPower);
        }
    }
    
    #endregion
}