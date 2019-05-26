using System;
using System.Collections.Generic;
using System.Text;

namespace YouTubeLibrary
{
    public class Channel
    {
        public string ChannelTitle { get; set; }
        public string ChannelId { get; set; }
        public string ChannelPictureUrl { get; set; }

        public List<Video> Videos { get; set; }
        public string ChannelDescription { get; set; }

        public double Rating { get; set; }
        public string RatingString { get; set; }
        public long Likes { get; set; }
        public long Dislikes { get; set; }
        public long Views { get; set; }
        public long RecentViews { get; set; } // 

        public long SubscriberCount { get; set; }
        public long CommentCount { get; set; }
        public long VideoCount { get; set; }


        public double RateChannel()
        {
            long sum_likes = 0;
            long sum_dislikes = 0;
            long sum_views = 0;

            foreach (Video v in Videos)
            {
                // videos with -1 likes, dislikes, or comments are videos that have had those three 
                // fields disabled, and will not be counted.
                if (v.LikeCount != -1)
                {
                    sum_likes += v.LikeCount;
                    sum_dislikes += v.DislikeCount;
                    sum_views += v.ViewCount;
                }
            }

            Likes = sum_likes;
            Dislikes = sum_dislikes;
            RecentViews = sum_views;

            Rating = CalculateRatingParts(sum_views, sum_likes, sum_dislikes);
            RatingString = string.Format("{0:0.##}", Rating) + "%";

            // This is the score that will be diplayed on the screen to everyone.
            return Rating;
        }
        public double CalculateRatingParts(long sum_views, long sum_likes, long sum_dislikes)
        {
            // the rule of thumb for average video data:
            // likes = 10% views
            // dislikes = 10% of likes

            // 90% of rating is likes-to-dislikes ratio
            // 10% of rating is likes-to-views ratio

            double likes_to_dislikes = 93.0 * ((sum_likes * 1.0) / (sum_likes + sum_dislikes));
            double likes_to_views = 7.0 * Math.Min(1, ((sum_likes * 12.5) / (sum_views)));

            double rating = likes_to_dislikes + likes_to_views;

            double skew = 50;
            rating = 100 * ((rating - skew) / (100 - skew));

            return Math.Max(0, Math.Min(100, rating));
        }

        public void PrintChannelRating(double rating)
        {
            string s = string.Format("{0:0.##}", rating);

            Console.WriteLine("\tChannel:  " + ChannelTitle);
            Console.WriteLine(string.Format($"\tRating:   " + s + "\n"));
        }
        public void PrintChannelUploadsDetails()
        {
            foreach (Video v in Videos)
            {
                v.PrintVideoDetails();
            }
        }
    }
}
