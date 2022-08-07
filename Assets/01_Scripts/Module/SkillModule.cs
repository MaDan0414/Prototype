using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SkillModule
{
    float coolDown { get; set; }
    float damage { get; set; }
    bool canShoot { get; set; }
}

public abstract class AbstractSkill : MonoBehaviour, SkillModule
{
    public abstract float coolDown { get; set; }
    public abstract float damage { get; set; }
    public abstract float SkillTime { get; set; }
    public abstract bool canShoot { get; set; }
    public abstract void Attack();
    public abstract void Shoot(Transform bullet);
    public abstract void SpawnBullet(Vector3 spawnPos);
    public Vector3 GetMousePosition(LayerMask layerMask)
    {
        Vector3 hitPoint = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            hitPoint = raycastHit.point;
        }
        else
        {
            hitPoint = Vector3.zero;
        }
        return hitPoint;
    }
}
