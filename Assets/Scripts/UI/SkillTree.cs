using UnityEngine;
using System.Collections.Generic;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.Systems
{
    [System.Serializable]
    public class SkillNode
    {
        public string SkillName;
        public int Cost;
        public bool IsUnlocked;
        public SkillNode[] Prerequisites;
    }

    public class SkillTree : MonoBehaviour
    {
        public List<SkillNode> Skills;

        public bool TryUnlockSkill(string skillName)
        {
            var skill = Skills.Find(s => s.SkillName == skillName);
            if (skill == null) return false;
            if (skill.IsUnlocked) return true;

            // Check prerequisites
            foreach (var req in skill.Prerequisites)
            {
                if (!req.IsUnlocked)
                {
                    Debug.Log($"Cannot unlock {skillName}, missing {req.SkillName}");
                    return false;
                }
            }

            // Check cost
            if (ResourceManager.Instance.SpendThread(skill.Cost))
            {
                skill.IsUnlocked = true;
                Debug.Log($"Unlocked {skillName}!");
                ApplySkillEffect(skillName);
                return true;
            }

            return false;
        }

        private void ApplySkillEffect(string skillName)
        {
            // Logic to enable abilities on Player
            // e.g. Player.EnableDoubleJump();
        }
    }
}
