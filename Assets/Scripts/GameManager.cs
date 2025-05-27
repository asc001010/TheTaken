using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioListener.pause = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  //ESC∏¶ ¥≠∑∂¿ª∂ß
        {
            Application.Quit(); //∞‘¿”/æ€ ¡æ∑·.
        }
    }
}
