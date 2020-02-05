using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    #region gameobject_variables
    [SerializeField]
    [Tooltip("healthpack")]
    private GameObject helthPack;
    #endregion

    #region helper_function
    IEnumerator DeleteChest()
    {
        yield return new WaitForSeconds(0.3f);
        Instantiate(helthPack, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void Interact()
    {
        StartCoroutine(DeleteChest());
    }
    #endregion
}
