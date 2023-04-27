using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using ZombieAttack.Exceptions;
using ZombieAttack.Managers;


namespace ZombieAttack.Managers
{
    public class CharacterManager : MonoSingleton<CharacterManager>
    {
        #region Variables
        [SerializeField] private List<CharacterPrefabState> characterPrefabStates = new List<CharacterPrefabState>();
        [SerializeField] private List<BaseCharacter> onSceneCharacter = new List<BaseCharacter>();
        private List<Vector3> spawnList = new List<Vector3>();
        private List<Vector3> sameLevelPositions = new List<Vector3>();

        [SerializeField] private ParticleSystem _particleSystem;

        [SerializeField] private int _spawnNum;
        [SerializeField] private int _spawnPoint;

        [SerializeField] private float _radius;

        private bool isCanMerge;

        #region Properties
        public bool IsCanMerge => isCanMerge;

        #endregion
        #endregion
        #region Methods
        private void Start()
        {
            CreateSpawnPoints(spawnList);
        }
        public List<BaseCharacter> GetCharacterList()
        {
            return onSceneCharacter;
        }
        private void AddCharacter(BaseCharacter character)
        {
            if (!onSceneCharacter.Contains(character))
            {
                onSceneCharacter.Add(character);
            }

        }

        private bool CanAddSoldier()
        {
            return GameManager.Instance.totalMoney <= UIManager.Instance.AddSoldierButtonMoneyCost ? true : false;
        }
        public void AddSoldier()
        {
            if (CanAddSoldier() || onSceneCharacter.Count == _spawnNum) return;
            var pos = AvaibleSpawnPoint(spawnList);
            CreateCharacter(1, pos);
        }
        private void CreateSpawnPoints(List<Vector3> list)
        {
            float deltaTheta = (2f * Mathf.PI) / _spawnNum;
            float theta = 1;
            for (int i = 0; i < _spawnNum; i++)
            {
                Vector3 pos = new Vector3(Mathf.Cos(theta) * _radius, .78f, Mathf.Sin(theta) * _radius);
                list.Add(pos);
                theta -= deltaTheta;
            }
        }



        private Vector3 AvaibleSpawnPoint(List<Vector3> spawnPoints)
        {
            List<Vector3> occupiedSpawnPoints = new List<Vector3>();

            foreach (var item in onSceneCharacter)
            {
                occupiedSpawnPoints.Add(item.transform.position);
            }

            Vector3 spawnPoint = onSceneCharacter[0].transform.position;
            float shortestDistance = Mathf.Infinity;

            for (int i = 0; i < spawnPoints.Count; i++)
            {

                var point = spawnPoints[i];

                if (occupiedSpawnPoints.Contains(point))
                {
                    continue;
                }
                var nextIndex = (i + 1) % spawnPoints.Count;

                float distance = Vector3.Distance(point, spawnPoints[nextIndex]);

                if (distance < 2)
                {
                    occupiedSpawnPoints.Add(point);
                }

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    spawnPoint = point;
                }
            }
            return spawnPoint;
        }

        private CharacterPrefabState GetCharacterPrefab(int level)
        {
            CharacterPrefabState character = default;

            for (int i = 0; i < characterPrefabStates.Count; i++)
            {
                if (characterPrefabStates[i].characterLevel == level)
                {
                    character = characterPrefabStates[i];
                }
            }
            return character;
        }
        private void CreateCharacter(int level, Vector3 pos)
        {
            var characterPrefab = GetCharacterPrefab(level).characterPrefab.gameObject;
            var newCharacter = Instantiate(characterPrefab, pos, Quaternion.identity);
            var baseCh = newCharacter.GetComponent<BaseCharacter>();
            AddCharacter(baseCh);
        }
        private void CreateParticle(Vector3 pos)
        {
            Instantiate(_particleSystem, pos, Quaternion.identity);
        }
        private void ConvertCharacters(List<BaseCharacter> sameLevelCharacters)
        {

            List<BaseCharacter> newList = new List<BaseCharacter>();
            newList.AddRange(sameLevelCharacters);

            StartCoroutine(Delay());
            IEnumerator Delay()
            {
                for (int i = 0; i < sameLevelCharacters.Count; i++)
                {
                    sameLevelCharacters[i].transform.DOMove(sameLevelCharacters[1].transform.position, 1);
                    RemoveCharacter(sameLevelCharacters[i]);
                    sameLevelPositions.Add(sameLevelCharacters[i].transform.position);
                }
                yield return new WaitForSeconds(.9f);

                foreach (var item in newList)
                {
                    Destroy(item.gameObject);
                }
            }


        }

        public void RemoveCharacter(BaseCharacter character)
        {
            if (onSceneCharacter.Contains(character))
            {
                onSceneCharacter.Remove(character);
            }
        }
        private bool CanMerge()
        {
            return GameManager.Instance.totalMoney <= UIManager.Instance.MergeMoneyCost ? true : false;
        }


        public void SetMerge()
        {
            if (CanMerge()) return;

            int _maxLevel = 3;
            isCanMerge = false;
            int level = 1;



            for (int i = 1; i < _maxLevel; i++)
            {
                List<BaseCharacter> _sameLevelCharacters = new List<BaseCharacter>();

                for (int k = 0; k < onSceneCharacter.Count; k++)
                {
                    var character = onSceneCharacter[k];

                    if (character.CharacterLevel == i)
                    {
                        _sameLevelCharacters.Add(character);
                    }
                    if (_sameLevelCharacters.Count == 3)
                    {
                        isCanMerge = true;
                        level = i + 1;
                        break;

                    }

                }

                if (isCanMerge)
                {
                    ConvertCharacters(_sameLevelCharacters);
                    CreateParticle(sameLevelPositions[1]);
                    CreateCharacter(level, sameLevelPositions[1]);
                    sameLevelPositions.Clear();

                    break;

                }
                else
                {
                    _sameLevelCharacters.Clear();
                }

            }

        }

    }

}

#endregion

[Serializable]
public class CharacterPrefabState
{
    public BaseCharacter characterPrefab;
    public int characterLevel;
}

