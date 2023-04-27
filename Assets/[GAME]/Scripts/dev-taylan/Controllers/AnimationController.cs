using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieAttack.Controllers
{
    public class AnimationController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Animator _animator;
        #endregion
        #region Methods

        public void OnZombieAttackAnimation(bool active)
        {
            _animator.SetBool(AnimationState.IsAttackDistance, active);
        }
        public void OnDeathAnimation(bool active)
        {
            _animator.SetBool(AnimationState.IsDeathAnim, active);
        }
        public void OnCharacterAttackAnimation(bool active)
        {
            _animator.SetBool(AnimationState.IsCharacterAttackAnim, active);

            StartCoroutine(False());
            IEnumerator False()
            {
                yield return new WaitForSeconds(1);
                _animator.SetBool(AnimationState.IsCharacterAttackAnim, false);
            }
        }
        #endregion

    }
}
