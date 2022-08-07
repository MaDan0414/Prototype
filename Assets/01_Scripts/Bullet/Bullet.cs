using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _damage;
    public float moveSpeed;
    public Vector3 shootDir;
    public AbstractSkill skill;

    private void Start()
    {
        skill.Attack();
    }
    private void Update()
    {
        skill.Shoot(transform);
    }
}
