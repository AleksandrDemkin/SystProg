using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int _health = 60;
    [SerializeField] private bool _coroutineIsOn;
    private int _maxHealth = 100;
    private int _addHealth = 5;
    private int _time = 3;

    private Coroutine _coroutine;


    private void Start()
    {
        NotActiveCoroutine();
        if (!_coroutineIsOn)
        {
            MyCoroutine();
        }
    }

    private void MyCoroutine()
    {
        _coroutine = StartCoroutine(ReceiveHealing(_health, _maxHealth, _addHealth, _time));
    }

    private IEnumerator ReceiveHealing(int health, int maxHealth, int addHealth, int time)
    {
        ActiveCoroutine();
        while (health < maxHealth)
        {
            health += addHealth;
            Debug.Log($"Current health: {health}");
            yield return new WaitForSeconds(time);
        }
        NotActiveCoroutine();
        Debug.Log($"Health is restored to {health}");
    }

    private void ActiveCoroutine()
    {
        _coroutineIsOn = true;
    }
    
    private void NotActiveCoroutine()
    {
        _coroutineIsOn = false;
    }
}
