using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Splines;
using UnityEngine.Timeline;
using static UnityEngine.GraphicsBuffer;



public class TimelineDebugTools : MonoBehaviour
{
    [SerializeField] private GameObject animalParent;

    [Button]
    private void DebugSetParentsAndResizeClips()
    {
        DebugSetAllClipParents();
        DebugSetAllClipLengths();
    }

    private void hello()
    {

    }

    private void DebugSetAllClipLengths()
    {
        PlayableDirector playableDirector = GetComponent<PlayableDirector>();

        foreach (TimelineClip clip in GetAllTimelineClips(playableDirector))
        {
            ControlPlayableAsset controlAsset = clip.asset as ControlPlayableAsset;
            clip.duration = controlAsset.prefabGameObject.GetComponentInChildren<SplineAnimate>().Duration;

            clip.displayName = controlAsset.prefabGameObject.name;
        }
    }

    
    private void DebugSetAllClipParents()
    {
        PlayableDirector playableDirector = GetComponent<PlayableDirector>();

        foreach (TimelineClip clip in GetAllTimelineClips(playableDirector))
        {
            ControlPlayableAsset controlAsset = clip.asset as ControlPlayableAsset;

            //Set parent gameObject reference
            playableDirector.SetReferenceValue(controlAsset.sourceGameObject.exposedName, animalParent);
        }
    }

    private List<TimelineClip> GetAllTimelineClips(PlayableDirector director)
    {
        List<TimelineClip> clips = new List<TimelineClip>();
        TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;

        if (timelineAsset == null)
        {
            Debug.LogError("The assigned PlayableAsset is not a TimelineAsset.");
            return clips;
        }

        // Iterate through all root tracks in the Timeline Asset
        foreach (TrackAsset track in timelineAsset.GetRootTracks())
        {
            // Use the GetClips() method for the track
            IEnumerable<TimelineClip> trackClips = track.GetClips();
            clips.AddRange(trackClips);

            // If you have nested timelines (Control Tracks), you might need
            // additional logic to access their clips recursively.
        }

        return clips;
    }
}
