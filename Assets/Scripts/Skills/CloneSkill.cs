using UnityEngine;

/*
 *  -- Clone Skill --
 * 
 * When triggered, this skill will create a clone of the player at the position where the skill is triggered.
 * This clone can attack any enemy inside the attack range. The clone can also switch sides if it detects an enemy
 * on the other side.
 * 
 */

public class CloneSkill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    public void CreateClone(Vector2 clonePos)
    {
        GameObject newClone = Instantiate(clonePrefab);
        CloneSkillController controller = newClone.GetComponent<CloneSkillController>();
        controller.SetupClone(clonePos, cloneDuration, canAttack);
    public IEnumerator CreateCloneInCounter(Vector2 clonePos)
    {
        if (createCloneInCounter)
        {
            yield return new WaitForSeconds(.1f);
            CreateClone(clonePos);
        }
    }
}
