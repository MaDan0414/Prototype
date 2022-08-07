using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : AbstractSkill
{
    public float _coolDown;
    public float _damage;
    public bool _canShoot = true;
    public float moveSpeed;
    [HideInInspector]
    public float _skillTime;
    public float destroyTime;
    [HideInInspector]
    public Vector3 shootDir;
    public Bullet bullet;
    [HideInInspector]
    public Player player;
    private GameObject ui_icon;
    public override float coolDown { get => _coolDown; set => _coolDown = value; }
    public override float damage { get => _damage; set => _damage = value; }
    public override float SkillTime { get => _skillTime; set => _skillTime = value; }
    public override bool canShoot { get => _canShoot; set => _canShoot = value; }

    private void Start()
    {
        player = transform.parent.parent.GetComponent<Player>();
    }

    public override void Attack()
    {
        Debug.Log("Å]¼uA_¶Ë®`:" + damage + "§N«o:" + coolDown);
    }
    public void SetUp(Bullet bullet)
    {
        bullet._damage = damage;
        bullet.moveSpeed = moveSpeed;
        bullet.skill = this;

        float angle = Mathf.Atan2(shootDir.z, shootDir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, -angle + 90f, 0));
        Destroy(bullet.gameObject, destroyTime);
    }
    public override void Shoot(Transform bullet)
    {
        bullet.GetComponent<Rigidbody>().velocity = bullet.forward * moveSpeed;
    }
    public override void SpawnBullet(Vector3 spawnPos)
    {
        if (!canShoot) return;
        if (Input.GetMouseButtonDown(0))
        {
            player.isAttack = true;
            player.FaceAttackDirection();
            StartCoroutine(AttackCD(coolDown, spawnPos));
        }        
    }
    public IEnumerator AttackCD(float cooldDown, Vector3 spawnPos)
    {
        canShoot = false;
        shootDir = GetMousePosition(player.groundMask) - transform.position;
        GameObject _bullet = Instantiate(bullet.gameObject, spawnPos, Quaternion.identity);
        SetUp(_bullet.GetComponent<Bullet>());
        player.isAttack = false;

        for (SkillTime = cooldDown; SkillTime > 0; SkillTime -= Time.deltaTime)
            yield return null;

        canShoot = true;
    }
}
