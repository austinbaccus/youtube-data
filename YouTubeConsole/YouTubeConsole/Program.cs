using System;

namespace YouTubeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                // Prompt user to ask for a YouTube channel
                Console.Write("\n\n\n\n\tEnter channel name: ");
                string channelSearchName = Console.ReadLine();

                // Find the channel and create a Channel object with the data found with the API
                YouTubeLibrary.Channel channel = YouTubeLibrary.Api.GetChannel(YouTubeLibrary.Api.SearchForChannelId(channelSearchName), 5);

                // Create a variable to store the channel's rating
                double rating = channel.RateChannel();

                // For each video in the channel, print a snippet of data to the console
                channel.PrintChannelUploadsDetails();

                // Print the channel's rating to the console
                channel.PrintChannelRating(rating);
                Console.WriteLine("\n");
            }
        }
    }
}
