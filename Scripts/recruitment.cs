using System.Linq;
using UnityEngine;

public class recruitment : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "add")
        {

            Debug.Log("Player Collided");
            PlayerManager.PlayerManagerCls.Rblst.Add(other.collider.GetComponent<Rigidbody>());

            other.transform.parent = null;

            other.transform.parent = PlayerManager.PlayerManagerCls.transform;

            other.gameObject.GetComponent<memberManager>().Ismember = true;
            if (!other.collider.gameObject.GetComponent<recruitment>())
            {              
                other.collider.gameObject.AddComponent<recruitment>();
            }

            other.collider.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material =
                PlayerManager.PlayerManagerCls.Rblst.ElementAt(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        }
    }
}
