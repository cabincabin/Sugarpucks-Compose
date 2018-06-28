using System.Collections.Generic;
using UnityEngine;

public class RhythmPlay1 : MonoBehaviour
{
    public TimelinePositioning timeline;
    public Stepp CreateAndPlaySequence;
    private bool isPlay;
    public Sprite play;
    public Sprite stop;
    public AudioClip playOnFirstPlayPress;
    private bool firstPlay = true;
    public GameObject FirstPuck = null;
    public GameObject SecondPuck = null;
    public int StartPuckPlacement=0;

    private void Start()
    {

        if (FirstPuck != null)
        {
            GameObject Puck;
            if (StartPuckPlacement == 0){
                
                Puck = Instantiate(FirstPuck, new Vector3(-4.91f, 0, 0), Quaternion.identity);
                Puck.name = "PlayPuck";
                Puck = Instantiate(FirstPuck, new Vector3(-3.56f, 0, 0), Quaternion.identity);
                Puck.name = "PlayPuck";
                Puck = Instantiate(FirstPuck, new Vector3(-2.08f, 0, 0), Quaternion.identity);
                Puck.name = "PlayPuck";
                Puck = Instantiate(FirstPuck, new Vector3(-0.69f, 0, 0), Quaternion.identity);
                Puck.name = "PlayPuck";
            }
            if (StartPuckPlacement == 1){
                for (int i = 0; i < 8; i++)
                {
                    Puck = Instantiate(FirstPuck, new Vector3(-4.91f + i*1.4f, 0, 0), Quaternion.identity);
                    Puck.name = "PlayPuck";  
                }
 
            }
            if (StartPuckPlacement == 2){
                for (int i = 0; i < 16; i++)
                {
                    Puck = Instantiate(FirstPuck, new Vector3(-4.91f + i*1.4f, 0, 0), Quaternion.identity);
                    Puck.name = "PlayPuck";  
                }
            }
            if (StartPuckPlacement == 3){
                for (int i = 0; i < 16; i++)
                {
                    if (i % 4 == 0 || i % 4 == 3)
                    {
                        Puck = Instantiate(FirstPuck, new Vector3(-4.91f + i * 1.4f, 0, 0), Quaternion.identity);
                        Puck.name = "PlayPuck";
                    }
                }
            }

            if (SecondPuck != null)
            {
                if (StartPuckPlacement == 4)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        if (i % 4 == 0 || i % 4 == 3)
                        {
                            Puck = Instantiate(FirstPuck, new Vector3(-4.91f + i * 1.4f, 0, 0), Quaternion.identity);
                            Puck.name = "PlayPuck";
                        }
                    }
                }
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        if(firstPlay){
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = playOnFirstPlayPress;
            audio.Play();
            firstPlay = false;
        }
        else
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Stop();
            //invert button every click, starting or stoping the subroutine
            isPlay = !isPlay;
            SpriteRenderer spriteRenderer = (SpriteRenderer) GetComponent<Renderer>();
            if (isPlay)
            {
                //wipe the current puck list
                CreateAndPlaySequence.ClearAllPitches();
                //Get all of the grids
                List<GameObject> GridsAsGameObj = timeline.TimingGrids;
                //for each grid
                CreateAndPlaySequence.stepCount = GridsAsGameObj.Count;
                CreateAndPlaySequence.Awake();
                for (int beat = 0; beat < GridsAsGameObj.Count; beat++)
                {
                    //find if there are any pucks added to the beat
                    TimingGrid Grid = GridsAsGameObj[beat].GetComponent<TimingGrid>();

                    for (int PuckIndex = Grid.Sprites.Count - 1; PuckIndex >= 0; PuckIndex--)
                    {
                        //get the puck's note and add the note to the corrisponding note in the sequence.
                        if (Grid.Sprites[PuckIndex].GetComponents<PlayableSprite>().Length != 0)
                        {
                            PlayableSprite SugarPuck = Grid.Sprites[PuckIndex].GetComponent<PlayableSprite>();
                            CreateAndPlaySequence.AddPitchAtStep(SugarPuck.PitchNumber + Grid.BeatNum, beat);
                        }
                        else
                        {
                            Grid.Sprites[PuckIndex].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                            Grid.Sprites[PuckIndex].GetComponent<Collider2D>().enabled = false;
                            Destroy(Grid.Sprites[PuckIndex]);
                            Grid.Sprites.RemoveAt(PuckIndex);
                        }


                    }
                }

                CreateAndPlaySequence.StartSequencer();
                spriteRenderer.sprite = stop;
            }

            //Stop Stepp
            if (!isPlay)
            {
                CreateAndPlaySequence.StopSequencer();
                spriteRenderer.sprite = play;
            }
        }

    }
}