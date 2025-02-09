using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : MonoBehaviour
{
    [SerializeField] private GameObject clonePrefab;

    public void CreateClone(Vector2 clonePos)
    {
        GameObject newClone = Instantiate(clonePrefab);
        CloneSkillController controller = newClone.GetComponent<CloneSkillController>();
        controller.SetupClone(clonePos);
    }
}
