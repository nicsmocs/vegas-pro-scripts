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

public class EntryPoint
{
    public void FromVegas(Vegas vegas)
    {
		//#################################################################################################
		//# YOU CAN DO SOMETHING INTERESTING HERE                                                         #
		//#################################################################################################
		//# this local variable holds the number of miliseconds which should be in between each cut       #
		//# You can change this e.g. to 1000. then you will get a cut each 1 second                       #
		long ms = 500; //                                                                                 #
		//#################################################################################################

		TrackEvent[] selectedEvents = GetSelectedEvents(vegas.Project);

		for(int i = 0; i < selectedEvents.Length; ++i)
		{
			TrackEvent trackEvent = selectedEvents[i];
			int loops = (int)(trackEvent.Length.ToMilliseconds() / ms);
			
			for(int l = 0; l < loops; ++l)
			{
				trackEvent = trackEvent.Split(Timecode.FromMilliseconds(ms));
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