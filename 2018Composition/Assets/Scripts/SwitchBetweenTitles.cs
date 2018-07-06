using UnityEngine;

public class SwitchBetweenTitles : MonoBehaviour
{
    public ChangeSkin Skin;
    private bool swap;
    private int c100;
    
    private void Update()
    {
        if (c100 >= 50)
        {
            if (swap)
            {
                Skin.changeSkin(0);
                swap = false;

            }
            else
            {
                Skin.changeSkin(1);
                swap = true;
            }

            c100 = 0;
        }
        else
        {
            c100++;
        }
    }
}