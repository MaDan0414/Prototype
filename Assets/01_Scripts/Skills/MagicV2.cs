using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicV2 : AbstractSkill
{
    public float _coolDown;
    public float _damage;
    public bool _canShoot = true;
    public float moveSpeed;
    [HideInInspector]
    public float _skillTime;
    public float destroyTime;
    public float spawnWait;
    public int defultValue;
    public float holdTime;
    private float maxHoldTime;
    private bool isHold;
    //[HideInInspector]
    public int bulletCount;
    [HideInInspector]
    public Vector3 shootDir;
    public Bullet bullet;
    [HideInInspector]
    public Player player;
    private GameObject ui_icon;
    private RectTransform ui_HoldTime, img_bar;
    public override float coolDown { get => _coolDown; set => _coolDown = value; }
    public override float damage { get => _damage; set => _damage = value; }
    public override float SkillTime { get => _skillTime; set => _skillTime = value; }
    public override bool canShoot { get => _canShoot; set => _canShoot = value; }
    private void Start()
    {
        maxHoldTime = 3;
        defultValue = 3;
        holdTime = 0;
        player = transform.parent.parent.GetComponent<Player>();
        ui_HoldTime = GameObject.FindGameObjectWithTag("Skill_Canvas").transform.Find("UI_holdTime").GetComponent<RectTransform>();
        img_bar = ui_HoldTime.transform.Find("img_bg").Find("img_bar").GetComponent<RectTransform>();
    }
    public override void Attack()
    {
        Debug.Log("多連魔彈B_傷害:" + damage + "冷卻:" + coolDown + "飛行速度:" + moveSpeed);
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

        if (Input.GetMouseButtonDown(1))
        {
            isHold = true;
            ui_HoldTime.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(1))
        {
            player.isAttack = true;
            player.FaceAttackDirection();
            holdTime = holdTime + Time.deltaTime > maxHoldTime ? maxHoldTime : holdTime + Time.deltaTime;
            img_bar.localPosition = new Vector3((-img_bar.rect.width + img_bar.rect.width * (holdTime / maxHoldTime)), 0, 0);
            if (holdTime >= maxHoldTime)
                Attack(spawnPos);
        }

        if (isHold && Input.GetMouseButtonUp(1))
        {
            Attack(spawnPos);
        }
    }    
    public void Attack(Vector3 spawnPos)
    {
        isHold = false;
        ui_HoldTime.gameObject.SetActive(false);
        bulletCount = (int)Math.Ceiling(Math.Round(holdTime, 1)) * defultValue;
        shootDir = GetMousePosition(player.groundMask) - transform.position;
        StartCoroutine(AttackCD(coolDown, spawnWait, bulletCount, spawnPos));
    }
    public IEnumerator AttackCD(float cooldDown, float spawnWait, int spawnCount , Vector3 spawnPos)
    {
        holdTime = 0;
        canShoot = false;
        player.isAttack = false;
        while (true)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                GameObject _bullet = Instantiate(bullet.gameObject, spawnPos, Quaternion.identity);
                SetUp(_bullet.GetComponent<Bullet>());
                yield return new WaitForSeconds(spawnWait);
            }
            break;
        }
        for (SkillTime = cooldDown; SkillTime > 0; SkillTime -= Time.deltaTime)
            yield return null;

        canShoot = true;
    }
}
