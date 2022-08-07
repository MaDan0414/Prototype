using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill : MonoBehaviour
{
    public AbstractSkill skill;
    [HideInInspector]
    public Image img_Bar;
    public float maxSec, nowSec;

    private void Start()
    {
        maxSec = skill.coolDown;
        img_Bar = transform.Find("img_skillTime").GetComponent<Image>();
    }

    private void Update()
    {
        if(img_Bar != null)
        {
            nowSec = skill.SkillTime < 0 ? 0 : skill.SkillTime;
            img_Bar.fillAmount = nowSec / maxSec;
        }
    }
}
