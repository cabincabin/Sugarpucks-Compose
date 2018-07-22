using System.Collections.Generic;
using UnityEngine;

public class RhythmPlayPoly : MonoBehaviour
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
    public int UpperPoly;
    public int LowerPoly;

    public GameObject PlayPos;
   
    private void Update()
    {
        if (isPlay)
        {
            PlayPos.GetComponent<SpriteRenderer>().enabled = true;
            if (CreateAndPlaySequence.currentStep<11)
            {
                timeline.TimelinePosTo(0);
                PlayPos.transform.position = new Vector3((CreateAndPlaySequence.currentStep)*timeline.lenOfTimelineSeg,PlayPos.transform.position.y,PlayPos.transform.position.z);
            }
            else
            {
                timeline.TimelinePosTo(CreateAndPlaySequence.currentStep+1);
            }
        }
    }

   
    private void Start()
    {
        PlayPos.GetComponent<SpriteRenderer>().enabled = false;
        FirstPuck.GetComponent<PlayableSprite>().PitchNumber = UpperPoly;
        SecondPuck.GetComponent<PlayableSprite>().PitchNumber = LowerPoly;

        GameObject Puck;
            
        for (int i = 0; i < UpperPoly*LowerPoly*3; i++)
        {
            if (i <  UpperPoly*LowerPoly)
            {
                    Puck = Instantiate(FirstPuck, new Vector3(-4.91f + i * 1.4f, FirstPuck.transform.position.y, 0), Quaternion.identity);
                    Puck.name = "PlayPuck";
            
                    Puck = Instantiate(SecondPuck, new Vector3(-4.91f + i * 1.4f, SecondPuck.transform.position.y, 0), Quaternion.identity);
                    Puck.name = "PlayPuck";

            }
            if (i >=  UpperPoly*LowerPoly)
            {
                if (i % UpperPoly == 0)
                {
                    Puck = Instantiate(FirstPuck, new Vector3(-4.91f + i * 1.4f, FirstPuck.transform.position.y, 0), Quaternion.identity);
                    Puck.name = "PlayPuck";
                }

                if (i % LowerPoly == 0)
                {
                    Puck = Instantiate(SecondPuck, new Vector3(-4.91f + i * 1.4f, SecondPuck.transform.position.y, 0), Quaternion.identity);
                    Puck.name = "PlayPuck";
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
                            if(SugarPuck.PitchNumber==UpperPoly)
                                CreateAndPlaySequence.AddPitchAtStep(Grid.BeatNum%SugarPuck.PitchNumber, beat);
                            else
                                CreateAndPlaySequence.AddPitchAtStep(Grid.BeatNum%SugarPuck.PitchNumber+UpperPoly, beat);
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