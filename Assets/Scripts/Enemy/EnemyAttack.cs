using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public List<GameObject> skillPrefabs = new List<GameObject>();

    private float waitTime = 0.15f;

    public float AddAllSkills()
    {
        StartCoroutine(CreateSkills());

        return skillPrefabs.Count * (waitTime + 0.5f);
    }

    private IEnumerator CreateSkills()
    {
        for (int i = 0; i < skillPrefabs.Count; i++)
        {
            GameManager.Instance.inventoryHandler.CreateSkill(skillPrefabs[i], transform.position);
            yield return new WaitForSeconds(waitTime + 0.5f);
        }
    }
}
