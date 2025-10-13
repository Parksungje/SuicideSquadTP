using UnityEngine;
using Tild.Chest;
using Code.Player;
using System;

public class SampleGun : Weapon
{
    [SerializeField] private PlayerInputSO playerInputSO;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePosition;

    private void Awake()
    {
        playerInputSO.OnAttackKeyPressed += OnAttack;
    }

    private void OnAttack(bool obj)
    {
        if (!obj) return;
        Debug.Log(playerInputSO.GetWorldMousePosition());
        Vector3 targetPos = playerInputSO.GetWorldMousePosition();
        Vector3 dir = (targetPos - firePosition.position).normalized;

        GameObject b = Instantiate(bullet, firePosition.position, Quaternion.LookRotation(dir));
        Rigidbody rb = b.GetComponent<Rigidbody>();
        if (rb) rb.linearVelocity = dir * 30f;
    }
}