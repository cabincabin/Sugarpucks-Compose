/*!	Stepp
 *	Pelican 7 LTD
 *
 *	Stepp is licensed under the terms of the MIT license.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Stepp : MonoBehaviour
{
	#region - Sequencer Properties
	/** Step Count
	 *	The number of steps/columns.
	 *	
	 *	@note Changing the step count whilst the sequencer is running is not supported.
	 */
	public int stepCount = kSteppDefaults_StepCount;

	/** Pitch Count
	 *	The number of pitches/rows.
	 *
	 *	@note Changing the pitch count whilst the sequencer is running is not supported.
	 */
	public int pitchCount = kSteppDefaults_PitchCount;
	
	/** Tempo
	 *	The number of steps per minute.
	 */
	public int tempo = kSteppDefaults_Tempo;

	/** Sequence
	 *	A two-dimensional array of booleans containing the current sequence.
	 *
	 *	@see AddPitchAtStep()
	 *	@see RemovePitchAtStep()
	 */
	private bool[,] sequence;

	/** Running
	 *	A boolean flag indicating whether the sequencer is currently running/sequencing.
	 */
	private bool running;

	/** Pitches
	 * 	An array of AudioClips corresponding to the sequencer's pitches. There must be
	 * 	an equal number of clips in the array as defined in the pitchCount. The order
	 * 	of clips in this array maps directly to the sequencer's pitches.
	 * 
	 * 	@see pitchCount
	 */
	public AudioClip[] pitches;
	#endregion
	
	#region - Voice Configuration
	private class Voice
	{
		public readonly AudioSource audioSource;
		public int pitch;
		public int step;

		public Voice(AudioSource inAudioSource, int inPitch, int inStep)
		{
			audioSource = inAudioSource;
			pitch = inPitch;
			step = inStep;
		}
	}

	/** Voice Count
	 *	The number of 'voices' or AudioSources available to the step sequencer. You
	 *	can increase the voiceCount if you require a particularly large number of
	 *	simultaneous sounds. For example this could occur when the step sequencer is
	 *	running particularly quickly or the audioclips are long in duration. You can
	 *	see the number of voices currently in use by using the
	 *	PrintNumberOfVoicesInUse() function to tune this to your needs.
	 *
	 *	@see PrintNumberOfVoicesInUse()
	 */
	public int voiceCount = kSteppDefaults_VoiceCount;

	/** Voices
	 *	The step sequencer's voices.
	 */
	private Voice[] voices;

	/** Least Recent Voice Index
	 *	The index of the least recently used voice.
	 */
	private int leastRecentVoiceIndex = 0;
	#endregion
	
	#region - Timing State
	/** DSP Start Time
	 *	The DSP time at which the sequencer was started.
	 */
	private double dspStartTime;

	/** Step Duration
	 *	The duration of a single step at the current tempo.
	 */
	private double stepDuration;

	/** Last Step
	 *	The last step that the sequencer was on. Used to determine when step
	 *	boundaries are crossed.
	 */
	private int lastStep;

	/** Current Step
	 *	The current step that the sequencer is on.
	 */
	private int currentStep;

	/** Scheduler Steps Ahead
	 *	The number of steps ahead that the sequencer schedules pitches to be
	 *	played. To ensure sequencing remains tight, sounds are scheduled this
	 *	many steps ahead of time.
	 *
	 *	@note Pitches will still be scheduled when added after the scheduler
	 *	has passed the step. Likewise they will still be cancelled if removed
	 *	after being scheduled, maintaining responsiveness.
	 */
	private int schedulerStepsAhead = kSteppDefaults_SchedulerStepsAhead;

	/** Has Scheduled First Step
	 *	Used to schedule the first step after calling StartSequencer().
	 */
	private bool hasScheduledFirstStep;
	#endregion
	
	#region - Callbacks
	/** Advanced Step Delegate
	 *	You may use this delegate callback to be notified when the sequencer
	 *	advances a step. The intended use is to allow you to update UI or other
	 *	state as the sequencer advances through steps.
	 *
	 *	@note This is called on the main thread and is not suitable for audio
	 *	synchronising.
	 */
	public delegate void SteppCallback(int step);
	public SteppCallback advancedStepDelegate;
	#endregion

	#region - Defaults
	private const int kSteppDefaults_StepCount = 8;
	private const int kSteppDefaults_PitchCount = 8;
	private const int kSteppDefaults_Tempo = 220;
	private const int kSteppDefaults_VoiceCount = 8;
	private const int kSteppDefaults_SchedulerStepsAhead = 1;
	#endregion

	#region - Initialization
	private void Awake()
	{
		sequence = new bool[pitchCount, stepCount];
		InitializeVoices();
	}
	#endregion
	
	#region - Sequencer Control
	/** Start Sequencer
	 *	Configures and starts the step sequencer.
	 */
	public void StartSequencer()
	{
		if (pitches.Length != pitchCount) {
			if (Debug.isDebugBuild) {
				Debug.Log("Warning - The number of assigned pitches does not match the pitch count.");
			}
		}

		stepDuration = 60.0 / tempo;
		
		lastStep = -1;
		currentStep = -1;

		double dspStartDelay = 0.2;
		dspStartTime = AudioSettings.dspTime + dspStartDelay;
		
		hasScheduledFirstStep = false;
		running = true;
	}
	
	/** Stop Sequencer
	 *	Stops the step sequencer. Cancels any scheduled pitches.
	 */
	public void StopSequencer()
	{
		running = false;
		CancelAllScheduledPitches();
	}

	/** Add Pitch At Step.
	 *	Adds a pitch to the step sequencer.
	 *	Attempts to add pitches/steps outside of the pitchCount/stepCount will
	 *	fail (gracefully).
	 *
	 *	@param pitch The pitch to add.
	 *	@param step The step at which to add.
	 *
	 *	@note You may add pitches when the sequencer is both running and not running.
	 */
	public void AddPitchAtStep(int pitch, int step)
	{
		SetPitchAtStep(pitch, step, true);
	}

	/** Remove Pitch At Step.
	 *	Removes a pitch from the step sequencer.
	 *	Attempts to remove pitches/steps outside of the pitchCount/stepCount will
	 *fail (gracefully).
	 *
	 *	@param pitch The pitch to remove.
	 *	@param step The step at which to remove.
	 *
	 *	@note You may remove pitches when the sequencer is both running and not running.
	 */
	public void RemovePitchAtStep(int pitch, int step)
	{
		SetPitchAtStep(pitch, step, false);
	}

	/** Set Pitch At Step
	 *	Set the state of a pitch step pair. Used internally by Add/RemovePitchAtStep.
	 *
	 *	@param pitch The pitch to set.
	 *	@param step The step at which to set.
	 *	@param active The state to set.
	 *
	 *	@note If a pitch is added at a step that the scheduler has passed but the
	 *	current step has not, it will be scheduled by this method. Likewise if a
	 *	pitch is removed during this zone between current step and schedule step, it
	 *	will be cancelled. This ensures the sequencer responds immediately to sequence
	 *	alterations.
	 *
	 *	@see AddPitchAtStep()
	 *	@see RemovePitchAtStep()
	 */
	private void SetPitchAtStep(int pitch, int step, bool active)
	{
		if (PitchInBounds(pitch) && StepInBounds(step)) {
			sequence[pitch, step] = active;
			if (running) {
				if (StepIsBetweenCurrentStepAndScheduleStep(step)) {
					if (active) {
						SchedulePitchAtTimeStep(pitch, step);
					} else {
						CancelPitchAtTimeStep(pitch, step);
					}
				}
			}
		} else {
			if (Debug.isDebugBuild) {
				Debug.Log("Cannot add pitch (" + pitch + ") at step (" + step + "). Out of bounds.");
			}
		}
	}

	public void ClearAllPitches()
	{
		for (int step = 0; step < stepCount; step++) {
			for (int pitch = 0; pitch < pitchCount; pitch++) {
				SetPitchAtStep(pitch, step, false);
			}
		}
	}
	#endregion

	#region - Sequencer Scheduling
	private void Update()
	{
		if (running) {
			
			stepDuration = 60.0 / tempo;

			double dspRunningTime = AudioSettings.dspTime - dspStartTime;
			float step = (float)(dspRunningTime / stepDuration);
			currentStep = Mathf.FloorToInt(step) % stepCount;
			currentStep = Mathf.Max(currentStep, -1);

			if (currentStep < 0) {
				if (hasScheduledFirstStep == false) {
					SchedulePitches();
					hasScheduledFirstStep = true;
				}
			}

			if (lastStep != currentStep) {
				SchedulePitches();
				lastStep = currentStep;

				if ((currentStep >= 0) && (advancedStepDelegate != null)) {
					advancedStepDelegate(currentStep);
				}
			}
		}
	}

	/**	Schedule Pitches
	 *	Schedule all pitches for the current schedule step.
	 */
	private void SchedulePitches()
	{
		int scheduleStep = (currentStep + schedulerStepsAhead) % stepCount;

		List<int> pitches = PitchesAtStep(scheduleStep);
		if (pitches != null) {

			foreach (int pitch in pitches) {
				SchedulePitchAtTimeStep(pitch, scheduleStep);
			}
		}
	}

	/**	Schedule Pitch At Time Step
	 *	Schedule a single pitch at a given step.
	 *
	 *	@param pitch The pitch to schedule.
	 *	@param step The step at which to schedule.
	 */
	private void SchedulePitchAtTimeStep(int pitch, int step)
	{
		Voice voice = LeastRecentlyUsedVoice();
		voice.audioSource.clip = AudioClipForPitch(pitch);
		voice.pitch = pitch;
		voice.step = step;
		
		double scheduleTime = ScheduleTimeForStep(step);
		voice.audioSource.PlayScheduled(scheduleTime);
	}

	/**	Cancel Pitch At Time Step
	 *	Cancel a single pitch at a given step.
	 *
	 *	@param pitch The pitch to cancel.
	 *	@param step The step at which to cancel.
	 */
	private void CancelPitchAtTimeStep(int pitch, int step)
	{
		Voice voiceToCancel = ScheduledVoiceAtPitchStep(pitch, step);
		if (voiceToCancel != null) {
			voiceToCancel.audioSource.Stop();
		}
	}

	/**	Cancel All Scheduled Pitches
	 *	Cancel all scheduled pitches at the current schedule step.
	 */
	private void CancelAllScheduledPitches()
	{
		int scheduleStep = (currentStep + schedulerStepsAhead) % stepCount;
		List<Voice> scheduledVoices = ScheduledVoicesAtStep(scheduleStep);
		foreach (Voice voiceToCancel in scheduledVoices) {
			voiceToCancel.audioSource.Stop();
		}
	}

	/**	Pitches At Step
	 *	Retrieve all pitches at a given step.
	 *
	 *	@param step The step at which to retrieve.
	 */
	private List<int> PitchesAtStep(int step)
	{
		if (StepInBounds(step)) {	 

			List<int> pitches = new List<int>();
			for (int pitch = 0; pitch < pitchCount; pitch++) {
				bool containsPitch = sequence[pitch, step];
				if (containsPitch) {
					pitches.Add(pitch);
				}
			}
			return pitches;

		} else {
			if (Debug.isDebugBuild) {
				Debug.Log("Cannot get pitches at step " + step + ". Out of bounds.");
			}
		}

		return null;
	}
	#endregion
	
	#region - Sequencer Timing
	/**	Current Sequence DSP Start Time
	 *	The DSP start time of the current sequence.
	 */
	private double CurrentSequenceDSPStartTime()
	{
		return dspStartTime + (SequencesElapsedCount() * SequenceDuration());
	}

	/**	Sequences Elapsed Count
	 *	The number of sequences that have elapsed since StartSequencer() was called.
	 *
	 *	@see StartSequencer()
	 */
	private int SequencesElapsedCount()
	{
		double dspRunningTime = AudioSettings.dspTime - dspStartTime;
		float currentSequence = (float)(dspRunningTime / SequenceDuration());
		return Mathf.Max(Mathf.FloorToInt(currentSequence), 0);
	}

	/**	Sequence Duration
	 *	The duration of a single sequence.
	 */
	private double SequenceDuration()
	{
		return stepDuration * stepCount;
	}

	/**	Schedule Time For Step
	 *	The schedule time for a given step taking into account the current sequence.
	 */
	private double ScheduleTimeForStep(int step)
	{
		double scheduleTime = CurrentSequenceDSPStartTime() + (step * stepDuration);
		if (step < currentStep) {
			scheduleTime += SequenceDuration();
		}
		return scheduleTime;
	}
	#endregion
	
	#region - Voice Management
	/**	Initialize Voices
	 *	Initialize all voices. Called internally in Awake().
	 *
	 *	@see voiceCount
	 */
	private void InitializeVoices()
	{
		voices = new Voice[voiceCount];
		for (int i = 0; i < voiceCount; i++) {
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			Voice voice = new Voice(audioSource, -1, -1);
			voices[i] = voice;
		}
	}

	/**	Least Recently Used Voice
	 *	Retrieve the least recently used voice.
	 *
	 *	@see leastRecentVoiceIndex
	 */
	private Voice LeastRecentlyUsedVoice()
	{
		Voice voice = voices[leastRecentVoiceIndex];
		leastRecentVoiceIndex = (leastRecentVoiceIndex + 1) % voices.Length;
		return voice;
	}

	/**	Audio Clip For Pitch
	 *	The AudioClip associated with a given pitch.
	 *
	 *	@param pitch The pitch with which the clip is associated.
	 *
	 *	@see pitches
	 */
	private AudioClip AudioClipForPitch(int pitch)
	{
		if (PitchInBounds(pitch)) {
			if (pitch < pitches.Length) {
				return pitches[pitch];
			} else {
				if (Debug.isDebugBuild) {
					Debug.Log("No pitch audio clip is assigned for pitch " + pitch + ".");
				}
			}
		} else {
			if (Debug.isDebugBuild) {
				Debug.Log("Cannot get audio clip for pitch " + pitch + ". Out of bounds.");
			}
		}
		return null;
	}

	/**	Scheduled Voice At Pitch Step
	 *	Retrieve the scheduled voice for a given pitch step pair. NULL if one has
	 *	not been scheduled.
	 *
	 *	@param pitch The pitch of the voice.
	 *	@param pitch The step of the voice.
	 */
	private Voice ScheduledVoiceAtPitchStep(int pitch, int step)
	{
		Voice scheduledVoice = null;
		foreach (Voice voice in voices) {
			if ((voice.pitch == pitch) && (voice.step == step)) {
				scheduledVoice = voice;
				break;
			}
		}
		return scheduledVoice;
	}

	/**	Scheduled Voices At Step
	 *	Retrieve all scheduled voices for a given step.
	 *
	 *	@param pitch The step at which to retrieve.
	 */
	private List<Voice> ScheduledVoicesAtStep(int step)
	{
		List<Voice> scheduledVoices = new List<Voice>();
		List<int> scheduledPitches = new List<int>();
		foreach (Voice voice in voices) {
			if (voice.step == step) {
				if (scheduledPitches.Contains(voice.pitch) == false) {
					scheduledVoices.Add(voice);
					scheduledPitches.Add(voice.pitch);
				}
			}
		}
		return scheduledVoices;
	}
	#endregion

	#region - Checks
	/** Pitch In Bounds
	 *	Tests that a given pitch is within the pitchCount.
	 *
	 *	@param pitch The pitch to test.
	 *
	 *	@see pitchCount
	 */
	private bool PitchInBounds(int pitch)
	{
		return (pitch >= 0 && pitch < pitchCount);
	}
	
	/** Step In Bounds
	 *	Tests that a given step is within the stepCount.
	 *
	 *	@param step The step to test.
	 *
	 *	@see stepCount
	 */
	private bool StepInBounds(int step)
	{
		return (step >= 0 && step < stepCount);
	}
	
	/**	Step Is Between Current Step And Schedule Step
	 * 	Tests if a given step is between the current step and the schedule step. This
	 * 	is used to determine if sequence alterations need to be updated immediately
	 * 	in the audio scheduling.
	 * 
	 *	@param step The step to test.
	 *
	 * 	@see SetPitchAtStep()
	 */
	private bool StepIsBetweenCurrentStepAndScheduleStep(int step)
	{
		int scheduleStep = (currentStep + schedulerStepsAhead) % stepCount;
		return ((step <= scheduleStep) && (step > currentStep));
	}
	#endregion
	
	#region - Tuning
	/** Print Number Of Voices In Use
	 * 	Prints the number of voices currently playing to the console. Useful for tuning
	 * 	the sequencer to your needs. More voices can be defined in the voiceCount if
	 * 	you wish to have more simultaneous sounds.
	 * 
	 * 	@note You can call this function from an update loop for a continuous report.
	 * 
	 *	@see voiceCount
	 *	@see Update()
	 */
	public void PrintNumberOfVoicesInUse()
	{
		int voicesInUseCount = 0;
		foreach (Voice voice in voices) {
			if (voice.audioSource.isPlaying) {
				voicesInUseCount += 1;
			}
		}
		Debug.Log(voicesInUseCount + " voices in use.");
	}
	#endregion
}