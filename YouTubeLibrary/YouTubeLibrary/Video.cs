using System;
using System.Collections.Generic;
using System.Text;

namespace YouTubeLibrary
{
    public class Video
    {
        public string Title { get; set; }
        public long ViewCount { get; set; }
        public long LikeCount { get; set; }
        public long DislikeCount { get; set; }
        public long CommentCount { get; set; }

        public DateTime UploadDate { get; set; }
        public string VideoDescription { get; set; }

        // video length?

        public void PrintVideoDetails()
        {
            Console.WriteLine(string.Format($"\n" +
                            $"\tTitle:    {Title}\n" +
                            $"\tViews:    {ViewCount}\n" +
                            $"\tLikes:    {LikeCount}\n" +
                            $"\tDislikes: {DislikeCount}\n" +
                            $"\tComments: {CommentCount}\n"));
        }
    }
}
