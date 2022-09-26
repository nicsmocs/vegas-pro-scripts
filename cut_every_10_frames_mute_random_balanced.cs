//############################################
//#  _   _ _          __  __                 #
//# | \ | (_)        |  \/  |                #
//# |  \| |_  ___ ___| \  / | ___   ___ ___  #
//# | . ` | |/ __/ __| |\/| |/ _ \ / __/ __| #
//# | |\  | | (__\__ | |  | | (_) | (__\__ \ #
//# |_| \_|_|\___|___|_|  |_|\___/ \___|___/ #
//############################################
//visit www.nicsmocs.com for more free content
//############################################

using System.Collections.Generic;
using ScriptPortal.Vegas;
using System;

public class EntryPoint
{
    public void FromVegas(Vegas vegas)
    {
		//#################################################################################################
		//# YOU CAN DO SOMETHING INTERESTING HERE                                                         #
		//#################################################################################################
		//# this local variable holds the number of frames which should be in between each cut            #
		//# You can change this e.g. to 10. then you will get a cut each 10 frames                        #
		long numFrames = 10; //                                                                           #
		//#################################################################################################
		
		
		Random random = new Random();
		TrackEvent[] selectedEvents = GetSelectedEvents(vegas.Project);
		
		if(selectedEvents.Length < 2) 
			return;
		
		List<int> randomMutedTracks = new List<int>();
		var fps = vegas.Project.Video.FrameRate;
		var allFrames = (int)(fps * (selectedEvents[0].Length.ToMilliseconds() / 1000));
		int loops = (int)(allFrames / numFrames);		
		randomMutedTracks.Add(random.Next(selectedEvents.Length-1));
		List<int> balance = new List<int>();
		balance.Add(randomMutedTracks[0]);
		
		for(int i = 1; i <= loops; ++i)
		{
			var track = random.Next(0, selectedEvents.Length);
			while(balance.Contains(track) || randomMutedTracks[i-1] == track)
			{
				track = random.Next(selectedEvents.Length);
			}
			
			balance.Add(track);
			
			if(balance.Count == selectedEvents.Length)
			{
				balance.Clear();
			}
			
			randomMutedTracks.Add(track);
		}
		
		for(int i = 0; i < selectedEvents.Length; ++i)
		{
			TrackEvent trackEvent = selectedEvents[i];
				
			for(var l = 0; l <= loops; ++l)
			{
				TrackEvent trackEventNew = trackEvent.Split(Timecode.FromFrames(numFrames));
				trackEvent.Mute = (randomMutedTracks[l] != i);
				trackEvent = trackEventNew;
			}
		}
	}
	
	TrackEvent[] GetSelectedEvents(Project project)
	{
		List<TrackEvent> selList = new List<TrackEvent>();
		foreach(Track track in project.Tracks)
		{
			foreach(TrackEvent trackEvent in track.Events)
			{
				if(trackEvent.Selected)
				{
					selList.Add(trackEvent);
				}
			}
		}
		return selList.ToArray();
	}
}