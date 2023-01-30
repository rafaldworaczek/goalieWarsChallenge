using UnityEngine;
[ExecuteInEditMode]
public class ChangeSortingOrder : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GetComponent<MeshRenderer>().sortingOrder = 8;
    }
}
