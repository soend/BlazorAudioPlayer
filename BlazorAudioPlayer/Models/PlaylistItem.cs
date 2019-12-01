using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorAudioPlayer.Models
{
    public class PlaylistItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string File { get; set; }
        public TimeSpan Duration { get; set; }
        
        public PlaylistItem(int id, string title, string file)
        {
            Id = id;
            Title = title;
            File = file;
        }
    }
}
