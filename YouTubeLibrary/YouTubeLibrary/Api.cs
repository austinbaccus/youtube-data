using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace YouTubeLibrary
{
    public static class Api
    {
        public static Task<string> GetChannelId(string channelName)
        {
            #region API information
            // https://www.googleapis.com/youtube/v3/search?part=id%2Csnippet&maxResults=1&q={WHATEVER THE USER TYPED}&type=channel&key={YOUR_API_KEY}

            /* Sample API call result
            {
             "kind": "youtube#searchListResponse",
             "etag": "\"XI7nbFXulYBIpL0ayR_gDh3eu1k/Wj92Y8UjkebcICR2N71aaqAha-g\"",
             "nextPageToken": "CAEQAA",
             "regionCode": "US",
             "pageInfo": {
              "totalResults": 3636,
              "resultsPerPage": 1
             },
             "items": [
              {
               "kind": "youtube#searchResult",
               "etag": "\"XI7nbFXulYBIpL0ayR_gDh3eu1k/9tzJfpig6VOMJNEn6Xab8_LaKJo\"",
               "id": {
                "kind": "youtube#channel",
                "channelId": "UCLx053rWZxCiYWsBETgdKrQ"
               },
               "snippet": {
                "publishedAt": "2006-03-09T16:31:14.000Z",
                "channelId": "UCLx053rWZxCiYWsBETgdKrQ",
                "title": "LGR",
                "description": "Weekly coverage of retro tech, PC games, and more! Classic computer gaming, Oddware, thrifting, Tech Tales, Sims, etc If you'd like to ask a question or just say ...",
                "thumbnails": {
                 "default": {
                  "url": "https://yt3.ggpht.com/-CsHahRaj2wE/AAAAAAAAAAI/AAAAAAAAAAA/3PP6XFMR-wk/s88-c-k-no-mo-rj-c0xffffff/photo.jpg"
                 },
                 "medium": {
                  "url": "https://yt3.ggpht.com/-CsHahRaj2wE/AAAAAAAAAAI/AAAAAAAAAAA/3PP6XFMR-wk/s240-c-k-no-mo-rj-c0xffffff/photo.jpg"
                 },
                 "high": {
                  "url": "https://yt3.ggpht.com/-CsHahRaj2wE/AAAAAAAAAAI/AAAAAAAAAAA/3PP6XFMR-wk/s800-c-k-no-mo-rj-c0xffffff/photo.jpg"
                 }
                },
                "channelTitle": "LGR",
                "liveBroadcastContent": "none"
               }
              }
             ]
            } 
            */
            #endregion

            Console.WriteLine("1: ");

            var parameters = new Dictionary<string, string>
            {
                ["part"] = "id%2Csnippet",
                ["maxResults"] = "1",
                ["q"] = channelName,
                ["type"] = "channel",
                ["key"] = System.Environment.GetEnvironmentVariable("APIKey", EnvironmentVariableTarget.Process)
            };
            string baseUrl = "https://www.googleapis.com/youtube/v3/search?";
            var fullUrl = MakeUrlWithQuery(baseUrl, parameters);

            var result = new HttpClient().GetStringAsync(fullUrl).Result;

            if (result != null)
            {
                dynamic dsObject = JsonConvert.DeserializeObject(result);
                return dsObject.items[0].snippet.channelId;
            }

            return default(dynamic);
        }
        public static string GetUploadPlaylistId(string channelId)
        {
            #region API information
            // https://www.googleapis.com/youtube/v3/channels?part=contentDetails&id={CHANNEL_ID}&maxResults={MAX_RESULTS}&fields=items%2FcontentDetails%2FrelatedPlaylists%2Fuploads&key={YOUR_API_KEY}

            /* Sample API call result
            {
             "items": [
              {
               "contentDetails": {
                "relatedPlaylists": {
                 "uploads": "UULx053rWZxCiYWsBETgdKrQ"
                }
               }
              }
             ]
            }
            */
            #endregion

            Console.WriteLine("2: ");

            var parameters = new Dictionary<string, string>
            {
                ["part"] = "contentDetails",
                ["id"] = channelId,
                ["maxResults"] = "50",
                ["fields"] = "items%2FcontentDetails%2FrelatedPlaylists%2Fuploads",
                ["key"] = System.Environment.GetEnvironmentVariable("APIKey", EnvironmentVariableTarget.Process)
            };
            string baseUrl = "https://www.googleapis.com/youtube/v3/channels?";
            var fullUrl = MakeUrlWithQuery(baseUrl, parameters);

            var result = new HttpClient().GetStringAsync(fullUrl).Result;

            if (result != null)
            {
                dynamic dsObject = JsonConvert.DeserializeObject(result);
                return dsObject.items[0].contentDetails.relatedPlaylists.uploads;
            }

            return default(dynamic);
        }
        public static List<string> GetVideoIdsInPlaylist(string playlistId, int maxResults)
        {
            #region API information
            // https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=50&playlistId={PLAYLIST_ID}&key={YOUR_API_KEY}

            /* Sample API call result
            {
             "kind": "youtube#playlistItemListResponse",
             "etag": "\"XI7nbFXulYBIpL0ayR_gDh3eu1k/bWO32_vm7nt9yV02UQBH7IRQbkc\"",
             "nextPageToken": "CDIQAA",
             "pageInfo": {
              "totalResults": 980,
              "resultsPerPage": 50
             },
             "items": [
              {
               "kind": "youtube#playlistItem",
               "etag": "\"XI7nbFXulYBIpL0ayR_gDh3eu1k/vwtyVa1XJqE9b-jnnwILHffYpAg\"",
               "id": "VVVMeDA1M3JXWnhDaVlXc0JFVGdkS3JRLlJySU8tdGFBVUg0",
               "snippet": {
                "publishedAt": "2018-12-14T19:28:40.000Z",
                "channelId": "UCLx053rWZxCiYWsBETgdKrQ",
                "title": "Holiday Lemmings: DOS PC Gaming Happiness",
                "description": "Lemmings by DMA Design was a computer gaming institution in the first half of the 1990s. And of course there were inevitable Christmas spin-offs of the original game. Four of them for DOS, to be exact!\n\n● LGR links:\nhttps://twitter.com/lazygamereviews\nhttps://www.facebook.com/LazyGameReviews\nhttps://www.patreon.com/LazyGameReviews\n\n● Music credits go to:\nhttp://www.epidemicsound.com\n\n#LGR #Christmas #Review",
                "thumbnails": {
                 "default": {
                  "url": "https://i.ytimg.com/vi/RrIO-taAUH4/default.jpg",
                  "width": 120,
                  "height": 90
                 },
                 "medium": {
                  "url": "https://i.ytimg.com/vi/RrIO-taAUH4/mqdefault.jpg",
                  "width": 320,
                  "height": 180
                 },
                 "high": {
                  "url": "https://i.ytimg.com/vi/RrIO-taAUH4/hqdefault.jpg",
                  "width": 480,
                  "height": 360
                 },
                 "standard": {
                  "url": "https://i.ytimg.com/vi/RrIO-taAUH4/sddefault.jpg",
                  "width": 640,
                  "height": 480
                 },
                 "maxres": {
                  "url": "https://i.ytimg.com/vi/RrIO-taAUH4/maxresdefault.jpg",
                  "width": 1280,
                  "height": 720
                 }
                },
                "channelTitle": "LGR",
                "playlistId": "UULx053rWZxCiYWsBETgdKrQ",
                "position": 0,
                "resourceId": {
                 "kind": "youtube#video",
                 "videoId": "RrIO-taAUH4"
                }
               }
              },
              
              .
              .
              .

             ]
            }
            */
            #endregion

            Console.WriteLine("3: ");

            var parameters = new Dictionary<string, string>
            {
                ["part"] = "snippet",
                ["maxResults"] = maxResults + "",
                ["playlistId"] = playlistId,
                ["key"] = System.Environment.GetEnvironmentVariable("APIKey", EnvironmentVariableTarget.Process)
            };
            string baseUrl = "https://www.googleapis.com/youtube/v3/playlistItems?";
            var fullUrl = MakeUrlWithQuery(baseUrl, parameters);

            var result = new HttpClient().GetStringAsync(fullUrl).Result;

            List<string> videoIds = new List<string>();

            if (result != null)
            {
                dynamic dsObject = JsonConvert.DeserializeObject(result);

                var count = dsObject.items.Count;
                if (count > 0)
                {
                    foreach (var item in dsObject.items)
                    {
                        string s = item.snippet.resourceId.videoId;
                        videoIds.Add(s);
                    }
                }
            }

            return videoIds;
        }
        public static Video GetVideoData(string videoId)
        {
            #region API information
            // https://www.googleapis.com/youtube/v3/videos?part=snippet%2Cstatistics&id={VIDEO_ID}&fields=items(snippet%2Ftitle%2Cstatistics)&key={YOUR_API_KEY}

            /* Sample API call result
            {
             "items": 
             [
              {
               "snippet": 
               {
                "title": "Holiday Lemmings: DOS PC Gaming Happiness"
               },
               "statistics": 
               {
                "viewCount": "145886",
                "likeCount": "8377",
                "dislikeCount": "85",
                "favoriteCount": "0",
                "commentCount": "718"
               }
              }
             ]
            }
            */
            #endregion

            Console.WriteLine("4: ");

            var parameters = new Dictionary<string, string>
            {
                ["part"] = "snippet%2Cstatistics",
                ["id"] = videoId,
                ["fields"] = "items(snippet%2Ftitle%2Cstatistics)",
                ["key"] = System.Environment.GetEnvironmentVariable("APIKey", EnvironmentVariableTarget.Process)
            };
            string baseUrl = "https://www.googleapis.com/youtube/v3/videos?";
            var fullUrl = MakeUrlWithQuery(baseUrl, parameters);

            var result = new HttpClient().GetStringAsync(fullUrl).Result;

            if (result != null)
            {
                dynamic dsObject = JsonConvert.DeserializeObject(result);

                Video v = new Video
                {
                    Title = dsObject.items[0].snippet.title
                };

                string viewCount = dsObject.items[0].statistics.viewCount;
                string likeCount = dsObject.items[0].statistics.likeCount;
                string dislikeCount = dsObject.items[0].statistics.dislikeCount;
                string commentCount = dsObject.items[0].statistics.commentCount;

                long views = -1;
                if (!Int64.TryParse(viewCount, out views))
                {
                    v.ViewCount = -1;
                }
                else v.ViewCount = views;

                long likes = -1;
                if (!Int64.TryParse(likeCount, out likes))
                {
                    v.LikeCount = -1;
                }
                else v.LikeCount = likes;

                long dislikes = -1;
                if (!Int64.TryParse(dislikeCount, out dislikes))
                {
                    v.DislikeCount = -1;
                }
                else v.DislikeCount = dislikes;

                long comments = -1;
                if (!Int64.TryParse(commentCount, out comments))
                {
                    v.CommentCount = -1;
                }
                else v.CommentCount = comments;

                return v;
            }

            return default(Video);
        }

        public static string SearchForChannelId(string channelSearchName)
        {
            #region API information
            // https://www.googleapis.com/youtube/v3/search?part=id%2Csnippet&maxResults=1&q={WHATEVER THE USER TYPED}&type=channel&key={YOUR_API_KEY}

            /* Sample API call result
            {
             "kind": "youtube#searchListResponse",
             "etag": "\"XI7nbFXulYBIpL0ayR_gDh3eu1k/Wj92Y8UjkebcICR2N71aaqAha-g\"",
             "nextPageToken": "CAEQAA",
             "regionCode": "US",
             "pageInfo": {
              "totalResults": 3636,
              "resultsPerPage": 1
             },
             "items": [
              {
               "kind": "youtube#searchResult",
               "etag": "\"XI7nbFXulYBIpL0ayR_gDh3eu1k/9tzJfpig6VOMJNEn6Xab8_LaKJo\"",
               "id": {
                "kind": "youtube#channel",
                "channelId": "UCLx053rWZxCiYWsBETgdKrQ"
               },
               "snippet": {
                "publishedAt": "2006-03-09T16:31:14.000Z",
                "channelId": "UCLx053rWZxCiYWsBETgdKrQ",
                "title": "LGR",
                "description": "Weekly coverage of retro tech, PC games, and more! Classic computer gaming, Oddware, thrifting, Tech Tales, Sims, etc If you'd like to ask a question or just say ...",
                "thumbnails": {
                 "default": {
                  "url": "https://yt3.ggpht.com/-CsHahRaj2wE/AAAAAAAAAAI/AAAAAAAAAAA/3PP6XFMR-wk/s88-c-k-no-mo-rj-c0xffffff/photo.jpg"
                 },
                 "medium": {
                  "url": "https://yt3.ggpht.com/-CsHahRaj2wE/AAAAAAAAAAI/AAAAAAAAAAA/3PP6XFMR-wk/s240-c-k-no-mo-rj-c0xffffff/photo.jpg"
                 },
                 "high": {
                  "url": "https://yt3.ggpht.com/-CsHahRaj2wE/AAAAAAAAAAI/AAAAAAAAAAA/3PP6XFMR-wk/s800-c-k-no-mo-rj-c0xffffff/photo.jpg"
                 }
                },
                "channelTitle": "LGR",
                "liveBroadcastContent": "none"
               }
              }
             ]
            } 
            */
            #endregion

            Console.WriteLine("5: ");

            var parameters = new Dictionary<string, string>
            {
                ["part"] = "id%2Csnippet",
                ["maxResults"] = "1",
                ["q"] = channelSearchName,
                ["type"] = "channel",
                ["key"] = System.Environment.GetEnvironmentVariable("APIKey", EnvironmentVariableTarget.Process)
            };
            string baseUrl = "https://www.googleapis.com/youtube/v3/search?";
            var fullUrl = MakeUrlWithQuery(baseUrl, parameters);

            var result = new HttpClient().GetStringAsync(fullUrl).Result;

            if (result != null)
            {
                dynamic dsObject = JsonConvert.DeserializeObject(result);

                if (dsObject == null)
                    return "";

                string resultKind = "";
                try
                {
                    resultKind = dsObject.items[0].id.kind;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return null;
                }

                if (resultKind.Contains("channel") || resultKind.Contains("user"))
                {
                    return dsObject.items[0].snippet.channelId; ;
                }
            }

            return null;
        }
        public static Channel GetChannel(string channelId, int maxResults)
        {
            Console.WriteLine("hello: ");

            // if the user's search yielded no results for any channels
            if (channelId == null)
            {
                return null;
            }

            #region API information
            // https://www.googleapis.com/youtube/v3/search?part=id%2Csnippet&maxResults=1&q={WHATEVER THE USER TYPED}&type=channel&key={YOUR_API_KEY}

            /* Sample API call result
            {
             "kind": "youtube#searchListResponse",
             "etag": "\"XI7nbFXulYBIpL0ayR_gDh3eu1k/Wj92Y8UjkebcICR2N71aaqAha-g\"",
             "nextPageToken": "CAEQAA",
             "regionCode": "US",
             "pageInfo": {
              "totalResults": 3636,
              "resultsPerPage": 1
             },
             "items": [
              {
               "kind": "youtube#searchResult",
               "etag": "\"XI7nbFXulYBIpL0ayR_gDh3eu1k/9tzJfpig6VOMJNEn6Xab8_LaKJo\"",
               "id": {
                "kind": "youtube#channel",
                "channelId": "UCLx053rWZxCiYWsBETgdKrQ"
               },
               "snippet": {
                "publishedAt": "2006-03-09T16:31:14.000Z",
                "channelId": "UCLx053rWZxCiYWsBETgdKrQ",
                "title": "LGR",
                "description": "Weekly coverage of retro tech, PC games, and more! Classic computer gaming, Oddware, thrifting, Tech Tales, Sims, etc If you'd like to ask a question or just say ...",
                "thumbnails": {
                 "default": {
                  "url": "https://yt3.ggpht.com/-CsHahRaj2wE/AAAAAAAAAAI/AAAAAAAAAAA/3PP6XFMR-wk/s88-c-k-no-mo-rj-c0xffffff/photo.jpg"
                 },
                 "medium": {
                  "url": "https://yt3.ggpht.com/-CsHahRaj2wE/AAAAAAAAAAI/AAAAAAAAAAA/3PP6XFMR-wk/s240-c-k-no-mo-rj-c0xffffff/photo.jpg"
                 },
                 "high": {
                  "url": "https://yt3.ggpht.com/-CsHahRaj2wE/AAAAAAAAAAI/AAAAAAAAAAA/3PP6XFMR-wk/s800-c-k-no-mo-rj-c0xffffff/photo.jpg"
                 }
                },
                "channelTitle": "LGR",
                "liveBroadcastContent": "none"
               }
              }
             ]
            } 
            */
            #endregion

            Channel channel = new Channel();

            Console.WriteLine("6: ");

            var parameters = new Dictionary<string, string>
            {
                ["id"] = channelId,
                ["part"] = "snippet%2CcontentDetails%2Cstatistics",
                ["key"] = System.Environment.GetEnvironmentVariable("APIKey", EnvironmentVariableTarget.Process)
            };
            string baseUrl = "https://www.googleapis.com/youtube/v3/channels?"; // youtube/v3/youtube.channels.list?
            var fullUrl = MakeUrlWithQuery(baseUrl, parameters);

            var result = new HttpClient().GetStringAsync(fullUrl).Result;

            if (result != null)
            {
                dynamic dsObject = JsonConvert.DeserializeObject(result);

                if (dsObject == null)
                    return default(Channel);

                channel.ChannelTitle = dsObject.items[0].snippet.title;
                channel.ChannelId = channelId;
                channel.ChannelDescription = dsObject.items[0].snippet.description;
                channel.Videos = GetVideosInChannel(channel, maxResults);

                channel.SubscriberCount = dsObject.items[0].statistics.subscriberCount;
                channel.Views = dsObject.items[0].statistics.viewCount;
                channel.CommentCount = dsObject.items[0].statistics.commentCount;
                channel.VideoCount = dsObject.items[0].statistics.videoCount;

                channel.ChannelPictureUrl = dsObject.items[0].snippet.thumbnails.medium.url;
                channel.ChannelPictureUrl = channel.ChannelPictureUrl.Replace("s240", "s88");

                return channel;
            }

            return default(Channel);
        }

        private static List<Video> GetVideosInChannel(Channel channel, int maxResults)
        {
            string uploadPlaylistId = GetUploadPlaylistId(channel.ChannelId);
            List<string> videoIds = GetVideoIdsInPlaylist(uploadPlaylistId, maxResults);

            List<Video> videos = new List<Video>();

            foreach (string id in videoIds)
            {
                videos.Add(Api.GetVideoData(id));
            }

            return videos;
        }

        private static string MakeUrlWithQuery(string baseUrl, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            if (parameters == null || parameters.Count() == 0)
                return baseUrl;

            return parameters.Aggregate(baseUrl, (accumulated, kvp) => string.Format($"{accumulated}{kvp.Key}={kvp.Value}&"));
        }
    }
}
