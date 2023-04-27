using System.Collections;
using UnityEngine;
using ZombieAttack.Managers;
using ZombieAttack.Controllers;
using DG.Tweening;

public class BaseCharacter : MonoBehaviour
{
    #region Variables
    [SerializeField] protected GameObject _closestZombie;
    [SerializeField] protected AnimationController _animationController;
    [SerializeField] protected CharacterData characterData;
    [SerializeField] protected BaseWeapon _baseWeapon;
    [SerializeField] protected HealthBarController _healthBarController;

    [SerializeField] protected int _characterLevel;

    protected float _characterHealth;
    protected float _closestZombieDist = float.MaxValue;

    protected bool isAttack;

    #region Properties
    public int CharacterLevel => _characterLevel;

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
        _characterHealth = characterData.maxHealth;
    }
    protected virtual void HandleUpdate()
    {
        SetTarget();


    }
    protected virtual void SetAttack()
    {
        StartCoroutine(WaitAtttack());
        IEnumerator WaitAtttack()
        {
            yield return new WaitForSeconds(characterData.attackTime);

          var zombie =  _closestZombie.GetComponent<BaseZombie>();
            if (zombie.IsGameOver) {isAttack = false; yield break; }
            if (_characterLevel <= 2)
            {
                _animationController.OnCharacterAttackAnimation(true);
                yield return new WaitForSeconds(.2f);
                if (zombie.IsGameOver) { isAttack = false; yield break; }
                _baseWeapon.Throw(_closestZombie.transform.position);
            }
            else if (_characterLevel == 3 && _closestZombieDist <= characterData.closeRangeAttackDistance)
            {

                _animationController.OnCharacterAttackAnimation(true);
            }
            isAttack = false;
            _closestZombie = null;
        }
    }

    protected virtual void SetTarget()
    {
        if (!ZombieManager.Instance.IsCalculateZombie() || isAttack) return;

        var selectedZombie = ZombieManager.Instance.GetZombie(this.transform);
        float distance;

        var targetPos = selectedZombie.transform.position;
        targetPos.y = transform.position.y;

        distance = Vector3.Distance(transform.position, selectedZombie.transform.position);
       
        if (distance <= characterData.attackDistance)
        {
           
            _closestZombie = selectedZombie.gameObject;
            _closestZombieDist = distance;
            gameObject.transform.DOLookAt(_closestZombie.transform.position, 0.5f);
            isAttack = true;
        }

        if (isAttack && _closestZombie != null)
        {
            SetAttack();
        }

    }
    protected virtual void SetDead()
    {
        _animationController.OnDeathAnimation(true);
        CharacterManager.Instance.RemoveCharacter(this);
        Destroy(this.gameObject, 3);
    }

    public virtual void SetHealth(float damage)
    {
        _characterHealth -= damage;
        _healthBarController.UpdateHealthBar(_characterHealth, characterData.maxHealth);
        if (_characterHealth == 0) SetDead();
    }



    #endregion
}