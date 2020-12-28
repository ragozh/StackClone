using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterDetection : MonoBehaviour
{
    public CharacterController GetClosestEnemy(List<CharacterController> listEnemies)
    {
        var enemiesOrdered = listEnemies
                        .OrderBy(x => Mathf.Abs((x.transform.position - transform.position).magnitude))
                        .ToList();
        if (enemiesOrdered.Count > 0 && enemiesOrdered.First())
            return enemiesOrdered.First();
        return null;
    }
}
