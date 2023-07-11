using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SatchelManager : MonoBehaviour
{
    [Header("Satchel Parameters")]
    [SerializeField] private GameObject satchelPrefab;
    [SerializeField] private float satchelExplosionRange, satchelExplosionStrength, enemySatchelExplosionStrength;
    [SerializeField] public float cooldownSatchel;
    [SerializeField] private AnimationCurve velocityCurve;
    [SerializeField] private GameObject explosionVFX;
    
    [Header("Core Parameters")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject attackColliderGameObject;
    [SerializeField] private string layerMaskName;
    [SerializeField] private CameraShake.Properties explosionCameraShake;
    
    public SatchelAmmo[] _satchelAmmos;
    public static SatchelManager instance;
    private NewControls _input = null;
    private bool _satchelExploded;
    private Collider[] _explosionCollider;
    
    private Vector3[] dir;
    private float time;
    
    [HideInInspector] public float[] timer;
    [HideInInspector] public bool[] stopTimer;

    private void Awake()
    {
        if (instance == null) instance = this;
        
        _input = new NewControls();
        stopTimer = new bool[_satchelAmmos.Length];
        timer = new float[_satchelAmmos.Length];
    }

    private void Start()
    {
        for (int i = 0; i < _satchelAmmos.Length; i++)
        {
            stopTimer[i] = true;
            timer[i] = cooldownSatchel;

            _satchelAmmos[i].uiImage.fillAmount = 1;
            _satchelAmmos[i].isAvailable = true;
        }
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.DropSatchel.performed += OnSatchelDropPerformed;
        _input.Player.ExplodeFirstSatchel.performed += OnSatchelFirstExplodePerformed;
        _input.Player.ExplodeSecondSatchel.performed += OnSatchelSecondExplodePerformed;
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        for (int i = 0; i < _satchelAmmos.Length; i++)
        {
            TimerManagerFunction(i);
            if (!stopTimer[i]) timer[i] += Time.deltaTime;
            UIFunctionUpdate(i);
        }

        if (!_satchelExploded) return;
        time += Time.deltaTime;

        if (time <= velocityCurve.keys[velocityCurve.length - 1].time)
        {
            float velocityFactor = velocityCurve.Evaluate(time / velocityCurve.keys[velocityCurve.length - 1].time);

            for (int i = 0; i < _explosionCollider.Length; i++)
            {
                if (_explosionCollider[i] == null) continue;
                
                Vector3 explosionForce = new Vector3();

                if (_explosionCollider[i].CompareTag("Enemy")) explosionForce = dir[i] * velocityFactor * enemySatchelExplosionStrength;
                else explosionForce = dir[i] * velocityFactor * satchelExplosionStrength;
                
                _explosionCollider[i].GetComponent<Rigidbody>().AddForce(explosionForce * Time.deltaTime, ForceMode.Impulse);
            }
        }
        else
        {
            _satchelExploded = false;
        }
    }

    private void OnSatchelDropPerformed(InputAction.CallbackContext value)
    {
        for (int i = 0; i < _satchelAmmos.Length; i++)
        {
            if (!_satchelAmmos[i].isAvailable) continue;
            DropSatchelAmmo(i);
            return;
        }
    }

    private void OnSatchelFirstExplodePerformed(InputAction.CallbackContext value)
    {
        if (_satchelAmmos[0].gameObject != null) ExplodeSatchel(_satchelAmmos[0].gameObject);
        _satchelAmmos[0].gameObject = null;
        _satchelAmmos[0].isAvailable = true;
    }
    
    private void OnSatchelSecondExplodePerformed(InputAction.CallbackContext value)
    {
        if (_satchelAmmos[1].gameObject != null) ExplodeSatchel(_satchelAmmos[1].gameObject);
        _satchelAmmos[1].gameObject = null;
        _satchelAmmos[1].isAvailable = true;
    }

    public void ExplodeSatchel(GameObject activeSatchel)
    {
        _explosionCollider = Physics.OverlapSphere(activeSatchel.transform.position, satchelExplosionRange, LayerMask.GetMask(layerMaskName));
        foreach (Collider col in _explosionCollider)
        {
            if (!col.gameObject.CompareTag("Enemy"))  continue;
            AttackManager.instance.KillEnemy(col.gameObject);
        }

        dir = new Vector3[_explosionCollider.Length];
        
        for (int i = 0; i < _explosionCollider.Length; i++)
            dir[i] = _explosionCollider[i].transform.position - activeSatchel.transform.position;

        _satchelExploded = true;
        time = 0f;

        Instantiate(explosionVFX, activeSatchel.transform.position, Quaternion.identity);
        CameraShake.instance.StartShake(explosionCameraShake);
        
        Destroy(activeSatchel);
    }

    private void DropSatchelAmmo(int id)
    {
        _satchelAmmos[id].isAvailable = false;
        _satchelAmmos[id].gameObject = Instantiate(satchelPrefab, player.position, Quaternion.identity);
        _satchelAmmos[id].uiImage.fillAmount = 0;
        timer[id] = 0;
        stopTimer[id] = false;
    }

    private void TimerManagerFunction(int id)
    {
        if (!(timer[id] >= cooldownSatchel)) return;
        stopTimer[id] = true;
        _satchelAmmos[id].isAvailable = true;
        _satchelAmmos[id].uiImage.fillAmount = 1;
    }

    private void UIFunctionUpdate(int id)
    {
        float currentFillAmount1 = Mathf.Lerp(0, 1, timer[id] / cooldownSatchel);
        _satchelAmmos[id].uiImage.fillAmount = currentFillAmount1;
    }
    

    [Serializable]
    public class SatchelAmmo
    {
        public Image uiImage;
        public bool isAvailable;
        public GameObject gameObject;
    }

}
