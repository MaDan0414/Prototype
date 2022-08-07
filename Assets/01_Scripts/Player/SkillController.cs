using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public List<AbstractSkill> _skillList = new List<AbstractSkill>();
    public GameObject skillsObj;

    public void AddSkill(AbstractSkill skill)
    {
        _skillList.Add(skill);
    }
    public void RemoveSkill(AbstractSkill skill)
    {
        _skillList.Remove(skill);
    }
    public void ChangeSkill(AbstractSkill skill, int index)
    {
        _skillList.RemoveAt(index);
        _skillList.Insert(index, skill);
    }
}
