using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Transform model;
    public Transform bulletPos;
    public LayerMask groundMask;
    private CharacterController controller;

    public bool isAttack,isDash;

    public SkillController skills;
    #region Movement;
    public float moveSpeed;
    public float rotaeSpeed;
    public float dashRadius;
    public float gravity;
    private float angle;
    private Vector3 moveDirection = Vector3.zero;
    private Quaternion targetRotation = Quaternion.Euler(Vector3.zero);
    #endregion

    public GameObject ball;

    private void Awake()
    {
        isAttack = false;
        model = transform.Find("PlayerModel");
        controller = GetComponent<CharacterController>();
        skills = GetComponent<SkillController>();
        skills.skillsObj = transform.Find("Skills").gameObject;
        bulletPos = transform.Find("PlayerModel").Find("BulletPos");
    }
    private void Update()
    {
        ball.transform.position = GetMousePosition(groundMask);

        Moving();
        Attack();
    }
    private void Moving()
    {
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && !isDash)
        {
            StartCoroutine(Dash(0.05f));
        }

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(H, 0, V);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;            
        }
        if ((H != 0 || V != 0) && !isAttack)
        {
            angle = Mathf.Atan2(H, V) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(new Vector3(0, angle, 0));
            model.rotation = Quaternion.Lerp(model.rotation, targetRotation, rotaeSpeed * Time.deltaTime);

        }
        else if (isAttack)
        {
            FaceAttackDirection();
        }
        moveDirection.y -= gravity * Time.deltaTime;
        if(!isDash)
            controller.Move(moveDirection * Time.deltaTime);
    }
    private void Attack()
    {
        AttackSet(0);
        AttackSet(1);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackSet(2);
        }
    }
    private void AttackSet(int skillNumber)
    {
        if (skills._skillList.Count > skillNumber && skills._skillList[skillNumber].canShoot)
        {
            skills._skillList[skillNumber].SpawnBullet(bulletPos.position);
            return;
        }
        else
        {
            isAttack = false;
            return;
        }
    }
    public void FaceAttackDirection()
    {
        Vector3 shootDir = GetMousePosition(groundMask) - transform.position;
        angle = Mathf.Atan2(shootDir.z, shootDir.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(new Vector3(0, -angle + 90f, 0));
        model.rotation = targetRotation;
    }
    private Vector3 GetMousePosition(LayerMask layerMask)
    {
        Vector3 hitPoint = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,  out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            hitPoint = raycastHit.point;
        }
        else
        {
            hitPoint = Vector3.zero;
        }
        return hitPoint;
    }

    public IEnumerator Dash(float _dashTime)
    {
        isDash = true;
        float startTime = Time.time;
        model.gameObject.SetActive(false);
        Vector3 mousePos = GetMousePosition(groundMask);
        float distance = Vector3.Distance(mousePos, transform.position);
        Vector3 targetPos = distance > dashRadius ? 
            GetVector3_Radius_Angle(dashRadius, GetAngle(mousePos)) : mousePos;
        while (Time.time < startTime + _dashTime)
        {

            transform.position = new Vector3(targetPos.x, transform.localPosition.y, targetPos.z);
            isDash = false;
            yield return null;
        }
        model.gameObject.SetActive(true);
        Debug.Log(distance);
    }
    private Vector3 GetVector3_Radius_Angle(float r, float angle)
    {
        float x = transform.localPosition.x + r * Mathf.Cos(angle * (Mathf.PI / 180f));
        float z = transform.localPosition.z + r * Mathf.Sin(angle * (Mathf.PI / 180f));

        Vector3 targetPos = new Vector3(x, transform.localPosition.y, z);
        return targetPos;
    }
    private float GetAngle(Vector3 pos)
    {
        Vector3 Dir = pos - transform.position;
        float angle = Mathf.Atan2(Dir.z, Dir.x) * Mathf.Rad2Deg;
        return angle;
    }
}
