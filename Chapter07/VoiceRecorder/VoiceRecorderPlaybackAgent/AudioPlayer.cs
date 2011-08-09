using System;
using Microsoft.Phone.BackgroundAudio;

namespace VoiceRecorderPlaybackAgent
{
    public class AudioPlayer : AudioPlayerAgent
    {
        /// <summary>
        /// Called when the playstate changes, except for the Error state (see OnError)
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track playing at the time the playstate changed</param>
        /// <param name="playState">The new playstate of the player</param>
        /// <remarks>
        /// Play State changes cannot be cancelled. They are raised even if the application
        /// caused the state change itself, assuming the application has opted-in to the callback
        /// </remarks>
        protected override void OnPlayStateChanged(BackgroundAudioPlayer player, AudioTrack track, PlayState playState)
        {
            //TODO: Add code to handle play state changes
            System.Diagnostics.Debug.WriteLine("OnPlayStateChagned(title={0}, source={1}, playState={2})", track.Title, track.Source, playState);
            NotifyComplete();
        }


        /// <summary>
        /// Called when the user requests an action using system-provided UI and the application has requesed
        /// notifications of the action
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track playing at the time of the user action</param>
        /// <param name="action">The action the user has requested</param>
        /// <param name="param">The data associated with the requested action.
        /// In the current version this parameter is only for use with the Seek action,
        /// to indicate the requested position of an audio track</param>
        /// <remarks>
        /// User actions do not automatically make any changes in system state; the agent is responsible
        /// for carrying out the user actions if they are supported
        /// </remarks>
        protected override void OnUserAction(BackgroundAudioPlayer player, AudioTrack track, UserAction action, object param)
        {
            //TODO: Add code to handle user actions through the application and system-provided UI
            System.Diagnostics.Debug.WriteLine("OnUserAction(title={0}, source={1}, actcion={2})", track.Title, track.Source, action);
            if(action == UserAction.Play)
                player.Play();
            NotifyComplete();
        }

        /// <summary>
        /// Called whenever there is an error with playback, such as an AudioTrack not downloading correctly
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track that had the error</param>
        /// <param name="error">The error that occured</param>
        /// <param name="isFatal">If true, playback cannot continue and playback of the track will stop</param>
        /// <remarks>
        /// This method is not guaranteed to be called in all cases. For example, if the background agent 
        /// itself has an unhandled exception, it won't get called back to handle its own errors.
        /// </remarks>
        protected override void OnError(BackgroundAudioPlayer player, AudioTrack track, Exception error, bool isFatal)
        {
            //TODO: Add code to handle error conditions
            System.Diagnostics.Debug.WriteLine("OnError(title={0}, source={1})", track.Title, track.Source);

            NotifyComplete();
        }

        /// <summary>
        /// Called when the agent request is getting cancelled
        /// </summary>
        protected override void OnCancel()
        {
            System.Diagnostics.Debug.WriteLine("OnCancel");
        }
    }
}
