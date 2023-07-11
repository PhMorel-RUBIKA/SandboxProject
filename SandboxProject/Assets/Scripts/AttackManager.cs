using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
   [SerializeField] private CameraShake.Properties enemyKillShake;
   [SerializeField] private GameObject enemyDeathVFX;
   [SerializeField] private float knockbackStrength;

   public static AttackManager instance;
   private SatchelManager _satchelManagerInstance;
   [HideInInspector] public int chainKill;

   private void Awake()
   {
      if (instance == null) instance = this;
   }
   
   private void Start()
   {
      _satchelManagerInstance = SatchelManager.instance;
   }

   public void KillEnemy(GameObject other)
   {
      if (other.GetComponent<EnemyManager>().life != 1)
      {
         other.GetComponent<EnemyManager>().life--;
         other.GetComponent<SpriteRenderer>().color = other.GetComponent<EnemyManager>().basicColor;
         
         Vector3 dir = (other.transform.position - transform.position).normalized;
         Vector3 extendedDir = dir * knockbackStrength;
         other.GetComponent<Rigidbody>().AddForce(extendedDir * Time.deltaTime, ForceMode.Impulse);
      }
      else
      {
         Destroy(other);

         CameraShake.instance.StartShake(enemyKillShake);
         Instantiate(enemyDeathVFX, other.transform.position, Quaternion.identity);

         chainKill++;
         ScoreManager.instance.AddCurrentScore(ScoreManager.instance.scoreToAddPerEnemy * chainKill, chainKill);

         for (int i = 0; i < _satchelManagerInstance._satchelAmmos.Length; i++)
         {
            if (_satchelManagerInstance._satchelAmmos[i].isAvailable) continue;

            _satchelManagerInstance.stopTimer[i] = true;
            _satchelManagerInstance.timer[i] = _satchelManagerInstance.cooldownSatchel;
            _satchelManagerInstance._satchelAmmos[i].isAvailable = true;
            _satchelManagerInstance._satchelAmmos[i].uiImage.fillAmount = 1;
         }
      }
   }
}
