using System;
using System.IO;
using System.Xml;

namespace Epicast.PodcastSyndication.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please specify a file to process");
                Environment.Exit(-1);
            }

            var path = args[0];
            var feed = ReadFeed(path);
        }

        public static PodcastSyndicationFeed ReadFeed(string path)
        {
            PodcastSyndicationFeed feed;
            using (var stream = File.OpenRead(path))
            using (var xmlReader = new XmlTextReader(stream))
            {
                feed = PodcastSyndicationFeed.Load<PodcastSyndicationFeed>(xmlReader);
            }

            return feed;
        }
    }
}
