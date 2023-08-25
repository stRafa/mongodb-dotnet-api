using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using NAudio.Lame;
using NAudio.Wave;
using System.Net.Http;
using System.Text.RegularExpressions;
using YoutubeExplode;

namespace MongoDBAPI.Services
{
    public class YtConvertService
    {
        private readonly IMongoCollection<YtAudio> _audioFilesCollection;
        public YtConvertService()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("youtubedb");
            _audioFilesCollection = database.GetCollection<YtAudio>("audioFiles");
        }

        public async Task<bool> PostAudio(string id)
        {
            try
            {
                await ConvertAndSaveToMongoDBAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<List<YtAudio>> GetYtAudios()
        {
            var audio = await _audioFilesCollection.Find(_ => true).ToListAsync();

            return audio;
        }

        private async Task ConvertAndSaveToMongoDBAsync(string videoId)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(videoId);




            var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(videoId);
            var audioStreamInfo = streamInfoSet.GetAudioOnlyStreams().FirstOrDefault();
            Convert
            var baseAddress = new Uri("https://rr8---sn-jucj-4gjs.googlevideo.com");
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                using (var responseStream = await httpClient.GetStreamAsync(audioStreamInfo.Url))
                {
                    var respons = await httpClient.GetStreamAsync(audioStreamInfo.Url);
                    var mp3Stream = ConvertToMp3(respons);

                    byte[] mp3Bytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        mp3Stream.CopyTo(memoryStream);
                        mp3Bytes = memoryStream.ToArray();
                    }

                    // Now 'mp3Bytes' contains the content of the downloaded audio in MP3 format
                    // You can further process these bytes as needed
                }
                var audioFile = new YtAudio
                {
                    Name = video.Title,
                    AudioLink = audioStreamInfo.Url
                };
                await _audioFilesCollection.InsertOneAsync(audioFile);

            }

            static Stream ConvertToMp3(Stream audioStream)
            {
                var mp3Stream = new MemoryStream();
                using (var reader = new RawSourceWaveStream(audioStream, new WaveFormat(44100, 2)))
                {
                    using (var writer = new LameMP3FileWriter(mp3Stream, reader.WaveFormat, LAMEPreset.STANDARD))
                    {
                        reader.CopyTo(writer);
                    }
                }
                mp3Stream.Position = 0;
                return mp3Stream;
            }
        }

        private static string GetVideoIdFromUrl(string url)
        {
            // Regular expression pattern to match video ID
            string pattern = @"(?:\?v=|/embed/|/v/|/youtu.be/)([a-zA-Z0-9_-]{11})";

            Match match = Regex.Match(url, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }
    }
}

public class YtAudio
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string AudioLink { get; set; }
}

