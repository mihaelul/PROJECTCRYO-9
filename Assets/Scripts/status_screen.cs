using UnityEngine;

public class status_screen : MonoBehaviour
{
    public Texture[] taskTextures;
    private Renderer screen;
    private int currTask = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screen = GetComponent<Renderer>();
        screen.material.mainTexture = taskTextures[0];
    }

    public void OnTaskCompleted()
    {
        currTask++;
        if (currTask < taskTextures.Length)
            screen.material.mainTexture = taskTextures[currTask];
    }

    public void ResetTasks()
    {
        currTask = 0;
        screen.material.mainTexture = taskTextures[0];
    }
}
