using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using ZombieAttack.Exceptions;
using Random = UnityEngine.Random;


namespace ZombieAttack.Managers
{
    public class ZombieManager : MonoSingleton<ZombieManager>
    {
        #region Variables

        [Header("Prefab Variables")]
        [SerializeField] private List<ZombiePrefabState> zombiePrefabStates = new List<ZombiePrefabState>();
        [SerializeField] private List<BaseZombie> sceneBaseZombie = new List<BaseZombie>();

        [SerializeField] private bool _areAllZombiesDead = true;
        public int _numEnemies; 
        [SerializeField] private int _multiplier;
       
        private int _zombieLevel = 1;
        public int zombieWawe; 
        private int totalZombieWawe = int.MaxValue;
        public int _zombiesKilled;
  

        #region Private Variables



        private Vector3 _pos;
        
        #endregion
        #endregion

        #region Methods
        private void Start()
        {
            SpawnZombie();
        }

        private void Update()
        {
           
            CheckAllZombie();
        }
        private ZombiePrefabState GetPrefabZombie(int level)
        {
            ZombiePrefabState prefabZombie = default;


            for (int i = 0; i < zombiePrefabStates.Count; i++)
            {
                if (zombiePrefabStates[i].levelZombie == level)
                {
                    prefabZombie = zombiePrefabStates[i];
                }
            }

            return prefabZombie;
        }
        private void AddZombie(BaseZombie baseZombie)
        {
            if (!sceneBaseZombie.Contains(baseZombie))
            {
                sceneBaseZombie.Add(baseZombie);
            }
        }

        public void RemoveZombie(BaseZombie baseZombie)
        {
            if (sceneBaseZombie.Contains(baseZombie))
            {
                sceneBaseZombie.Remove(baseZombie);
            }

        }

        private void CreateZombie(int level)
        {
          
            var prefabZom = GetPrefabZombie(level).basePrefabZombie.gameObject;
            var sceneZombie = Instantiate(prefabZom, _pos, Quaternion.identity);  
            var bZombie = sceneZombie.GetComponent<BaseZombie>();
            AddZombie(bZombie);

            
        }

        public void IncreaseKilledZombie()
        {
            _zombiesKilled++;
        }
        private void CheckAllZombie()
        {
            if (sceneBaseZombie.Count == 0 && _areAllZombiesDead == false)
            {
                _areAllZombiesDead = true;
                SpawnZombie();
            }


        }
        private void SpawnZombie()
        {


         float _circleRadius;
         float _angleStep;
         float _angle;

        StartCoroutine(ZombieSpawner());
            IEnumerator ZombieSpawner()
            {
                for (int i = zombieWawe; i < totalZombieWawe; i++)
                {
                    if (!_areAllZombiesDead) break;

                    zombieWawe++;
                    _numEnemies++;

                    for (int j = 0; j < _numEnemies; j++)
                    {
                        _angleStep = 360 / _numEnemies;
                        _angle = j * _angleStep;

                        _circleRadius = Random.Range(12, 20);

                        float x = _circleRadius * Mathf.Cos(_angle * Mathf.Deg2Rad);
                        float z = _circleRadius * Mathf.Sin(_angle * Mathf.Deg2Rad);

                        _pos = new Vector3(x, 0, z);


                        yield return new WaitForSeconds(Random.Range(0, 4));

                        //int zombieLevel2 = 0;

                        //if (zombieWawe > 3)
                        //{
                        //    if (zombieWawe % 3 == 0)
                        //    {
                        //        zombieLevel2++;
                        //    }
                        //}
                        if (zombieWawe == 1)
                        {
                            CreateZombie(_zombieLevel);
                        }

                        else if (zombieWawe > 1 && zombieWawe <= 5)
                        {

                            CreateZombie(Random.Range(1, 3));

                        }
                        if (zombieWawe > 5)
                        {
                            CreateZombie(Random.Range(1, 3));


                        }
                        _areAllZombiesDead = false;
                    }
                }
            }

            { }


        }

        public BaseZombie GetZombie(Transform target)
        {
            float nearestZombieDist = float.MaxValue;
            BaseZombie selectedZombie = null;
            for (int i = 0; i < sceneBaseZombie.Count; i++)
            {
                var dist = Vector3.Distance(target.transform.position, sceneBaseZombie[i].transform.position);
                if (dist < nearestZombieDist && !sceneBaseZombie[i].IsGameOver)
                {
                    selectedZombie = sceneBaseZombie[i];
                    nearestZombieDist = dist;
                }

            }
            return selectedZombie;
        }

        public bool IsCalculateZombie()
        {
            return sceneBaseZombie.Count > 0 ? true : false;
        }
        
    }
    #endregion



    [Serializable]
    public class ZombiePrefabState
    {
        public BaseZombie basePrefabZombie;
        public int levelZombie;
    }
}