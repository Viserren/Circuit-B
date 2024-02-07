using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BatteryHealth : MonoBehaviour, IDataPersistance
{
    int _health;
    int _maxHealth = 10;

    UnityEvent<float> HealthEvent = new UnityEvent<float>();
    UnityEvent<float> MaxHealthEvent = new UnityEvent<float>();

    public UnityEvent Dead = new UnityEvent();
    public UnityEvent Alive = new UnityEvent();
    bool _isDead = false;

    List<BatteryUI> _batteryUIs;

    private void Start()
    {
        _batteryUIs = FindObjectsOfType<BatteryUI>(true).ToList();

        foreach (var batteryUI in _batteryUIs)
        {
            HealthEvent.AddListener(batteryUI.UpdateValue);
            MaxHealthEvent.AddListener(batteryUI.UpdateMaxValue);
        }
        Dead.AddListener(MenuManager.Instance.DeadScreen);
    }

    public void UpdateUI()
    {
        MaxHealthEvent.Invoke(_maxHealth);
        HealthEvent.Invoke(_health);
    }

    public void IncreaseHealth(int amount)
    {
        if (_health + amount > _maxHealth)
        {
            _health = _maxHealth;
        }
        else
        {
            _health += amount;
        }
        Debug.Log("Increase Health");
        HealthEvent.Invoke(_health);
    }

    public void DecreaseHealth(int amount)
    {
        if (_health - amount < 0)
        {
            _health = 0;
            _isDead = true;
            Dead.Invoke();
        }
        else
        {
            _health -= amount;
        }
        //Debug.Log("Decrease Health");
        HealthEvent.Invoke(_health);
    }

    public void IncreaseMaxHealth(int amount)
    {
        _maxHealth += amount;
        MaxHealthEvent.Invoke(_maxHealth);
    }

    public void DecreaseMaxHealth(int amount)
    {
        _maxHealth -= amount;
        MaxHealthEvent.Invoke(_maxHealth);
    }

    public void LoadData(GameData gameData)
    {
        _maxHealth = gameData.maxHealth;
        _health = gameData.health;

        if (gameData.hasBattery)
        {
            gameObject.SetActive(true);
            foreach (BatteryUI batteryUI in _batteryUIs)
            {
                batteryUI.gameObject.SetActive(true);
            }
        }
        else
        {
            gameObject.SetActive(false);
            foreach (BatteryUI batteryUI in _batteryUIs)
            {
                batteryUI.gameObject.SetActive(false);
            }
        }

        if (_isDead == false && gameData.isDead == true)
        {
            _isDead = gameData.isDead;
            Dead.Invoke();
        }
        else if(_isDead == true && gameData.isDead == false)
        {
            Alive.Invoke();
        }
        UpdateUI();
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.health = _health;
        gameData.maxHealth = _maxHealth;
        gameData.isDead = _isDead;
        gameData.hasBattery = gameObject.activeSelf;
    }
}
