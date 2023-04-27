using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using ZombieAttack.Managers;

public class BaseWeapon : MonoBehaviour
{
    #region Variables

    [SerializeField] protected GameObject _weapon;
    [SerializeField] protected GameObject prefabVeapon;

    [SerializeField] protected bool isWeaponCloseRange;

    public WeaponData weaponData;

    protected Quaternion firstQuarternion;

    protected float _duration;
    protected float _jumpPower;
    protected float _attackPower;

    #endregion

    public bool IsWeaponCloseRange => isWeaponCloseRange;

    protected void Start()
    {
        firstQuarternion = this.gameObject.transform.rotation;
        _duration = weaponData.duration;
        _jumpPower = weaponData.jumpPower;
    }
    protected void Update()
    {
        if (this.transform.parent == null)
        {
            Destroy(this.gameObject,10);
        }
    }
    protected void RecreateWeapon()
    {
        Quaternion parentRotation = transform.parent.rotation; 
        Quaternion localRotation = Quaternion.Euler(0, 180,0); 
        Quaternion rotation = parentRotation * localRotation; 

        StartCoroutine(WaitCreate());
        IEnumerator WaitCreate()
        {
            yield return new WaitForSeconds(.2f);

            var weapon = Instantiate(prefabVeapon, this.transform.position, rotation);
            weapon.transform.SetParent(gameObject.transform);
            _weapon = weapon;

        }
    }

    public virtual void Throw(Vector3 pos)
    {

        _weapon.transform.SetParent(null);
        _weapon.transform.DOJump(pos + Vector3.up, _jumpPower, 0, _duration);
        _weapon.transform.DORotate(pos, 1);
        _weapon = null;
        RecreateWeapon();
    }

}
