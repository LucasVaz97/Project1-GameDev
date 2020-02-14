using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickArrow : MonoBehaviour
{


    #region functions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerController>().pickArrow();

            Destroy(this.gameObject);
        }

    }
    #endregion
}
