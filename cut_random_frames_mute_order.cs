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
		//# this local variable holds the minimum and maximum values of the random number of frames which #
		//# should be in between each cut                                                                 #
		//# You can change this e.g. to {30,50}. then you will get cuts which are random generated between#
		//# 30 and 50 frames                                                                              #
		int[] valueRange = {20,60}; //                                                                    #
		//#################################################################################################
		Random random = new Random();
		TrackEvent[] selectedEvents = GetSelectedEvents(vegas.Project);
		
		if(selectedEvents.Length < 2) 
			return;
		
		List<int> randomNumbers = new List<int>();
		var fps = vegas.Project.Video.FrameRate;
		var allFrames = (int)(fps * (selectedEvents[0].Length.ToMilliseconds() / 1000));
		
		while(allFrames > 0)
		{
			var numFrames = random.Next(valueRange[0],valueRange[1]);
			if(allFrames + numFrames < valueRange[1])
			{
				numFrames = allFrames + numFrames;
			}
			randomNumbers.Add(numFrames);
			allFrames -= numFrames;
		}
		
		int[] arrRandomNums = randomNumbers.ToArray();

		for(int i = 0; i < selectedEvents.Length; ++i)
		{
			TrackEvent trackEvent = selectedEvents[i];
			
			for(int l = 0; l < arrRandomNums.Length; ++l)
			{
				TrackEvent trackEventNew = trackEvent.Split(Timecode.FromFrames(arrRandomNums[l]));
				trackEvent.Mute = (l % selectedEvents.Length != i);
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