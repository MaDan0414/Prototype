using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSkill : MonoBehaviour
{
    public Player player;
    public AbstractSkill magicBullet, MagicV2;
    public UI_Skill ui_MagicBullet, ui_MagicV2;

    public void Btn_GetMagicBullet()
    {
        AbstractSkill skill = Instantiate(magicBullet, player.skills.skillsObj.transform);
        player.skills.AddSkill(skill); 
        
        UI_Skill uI_Skill = Instantiate(ui_MagicBullet, transform.Find("My Skills").GetChild(player.skills._skillList.IndexOf(skill)));
        uI_Skill.GetComponent<RectTransform>().localPosition = Vector3.zero;
        uI_Skill.skill = skill;
    }
    public void Btn_GetMagicV2()
    {
        AbstractSkill skill = Instantiate(MagicV2, player.skills.skillsObj.transform);
        player.skills.AddSkill(skill);

        UI_Skill uI_Skill = Instantiate(ui_MagicV2, transform.Find("My Skills").GetChild(player.skills._skillList.IndexOf(skill)));
        uI_Skill.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        uI_Skill.skill = skill;
    }
}
