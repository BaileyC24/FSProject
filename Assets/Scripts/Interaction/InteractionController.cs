using UnityEngine;

public class InteractionController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Hey, y'all, InteractionController is running. Woohoo!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("InteractionController detected a P key. Woohoo!");
        }
    }
}
