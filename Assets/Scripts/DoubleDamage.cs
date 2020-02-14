using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamage : MonoBehaviour
{
    #region variables
    [SerializeField]
    [Tooltip("how much it heals")]
    private int healAmount;
    #endregion

    #region functions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerController>().DoubleDamage();
            Destroy(this.gameObject);
        }

    }
    #endregion
}
