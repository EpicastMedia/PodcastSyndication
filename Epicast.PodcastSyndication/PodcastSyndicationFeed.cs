using System;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Epicast.PodcastSyndication
{
    public class PodcastSyndicationFeed : SyndicationFeed
    {
        private Collection<SyndicationCategory> categories;

        public PodcastSyndicationFeed()
        {
            AttributeExtensions.Add(
                new XmlQualifiedName("itunes", "http://www.w3.org/2000/xmlns/"),
                ITunesConstants.ITunesNamespace);
        }

        public TextSyndicationContent Author { get; set; }
        public TextSyndicationContent Block { get; set; }
        public new Collection<SyndicationCategory> Categories
        {
            get
            {
                if (categories == null)
                {
                    categories = new Collection<SyndicationCategory>();
                }
                return categories;
            }
        }
        public TextSyndicationContent Complete { get; set; }
        public TextSyndicationContent Explicit { get; set; }
        public new Uri ImageUrl { get; set; }
        public Uri NewFeedUrl { get; set; }
        public SyndicationPerson Owner { get; set; }
        public TextSyndicationContent Subtitle { get; set; }
        public TextSyndicationContent Summary { get; set; }

        public static new TSyndicationFeed Load<TSyndicationFeed>(XmlReader reader)
            where TSyndicationFeed : PodcastSyndicationFeed, new()
        {
            var result = SyndicationFeed.Load<TSyndicationFeed>(reader);
            reader = result.ElementExtensions.GetReaderAtElementExtensions();

            while (reader.IsStartElement())
            {
                if (reader.IsStartElement(ITunesConstants.AuthorTag, ITunesConstants.ITunesNamespace))
                {
                    result.Author = new TextSyndicationContent(reader.ReadElementContentAsString());
                }
                else if (reader.IsStartElement(ITunesConstants.BlockTag, ITunesConstants.ITunesNamespace))
                {
                    result.Block = new TextSyndicationContent(reader.ReadElementContentAsString());
                }
                else if (reader.IsStartElement(ITunesConstants.CategoryTag, ITunesConstants.ITunesNamespace))
                {
                    result.Categories.Add(ReadCategory(reader.ReadSubtree()));
                    reader.Skip();
                }
                else if (reader.IsStartElement(ITunesConstants.CompleteTag, ITunesConstants.ITunesNamespace))
                {
                    result.Complete = new TextSyndicationContent(reader.ReadElementContentAsString());
                }
                else if (reader.IsStartElement(ITunesConstants.ExplicitTag, ITunesConstants.ITunesNamespace))
                {
                    result.Explicit = new TextSyndicationContent(reader.ReadElementContentAsString());
                }
                else if (reader.IsStartElement(ITunesConstants.ImageTag, ITunesConstants.ITunesNamespace))
                {
                    if (reader.HasAttributes)
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.LocalName == ITunesConstants.HrefAttribute)
                            {
                                result.ImageUrl = new Uri(reader.Value, UriKind.RelativeOrAbsolute);
                            }
                        }
                        reader.MoveToElement();
                        reader.Skip();
                    }
                }
                else if (reader.IsStartElement(ITunesConstants.NewFeedUrlTag, ITunesConstants.ITunesNamespace))
                {
                    result.NewFeedUrl = new Uri(reader.ReadElementContentAsString(), UriKind.RelativeOrAbsolute);
                }
                else if (reader.IsStartElement(ITunesConstants.OwnerTag, ITunesConstants.ITunesNamespace))
                {
                    result.Owner = ReadOwner(reader);
                }
                else if (reader.IsStartElement(ITunesConstants.SubtitleTag, ITunesConstants.ITunesNamespace))
                {
                    result.Subtitle = new TextSyndicationContent(reader.ReadElementContentAsString());
                }
                else if (reader.IsStartElement(ITunesConstants.SummaryTag, ITunesConstants.ITunesNamespace))
                {
                    result.Summary = new TextSyndicationContent(reader.ReadElementContentAsString());
                }
                else
                {
                    reader.Skip();
                }
            }
            return result;
        }

        public static SyndicationCategory ReadCategory(XmlReader reader)
        {
            reader.Read();

            var category = new SyndicationCategory();
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    if (reader.LocalName == ITunesConstants.TextAttribute)
                    {
                        category.Name = reader.Value;
                    }
                }
                reader.MoveToElement();
            }

            while (reader.Read())
            {
                if (reader.IsStartElement(ITunesConstants.CategoryTag, ITunesConstants.ITunesNamespace))
                {
                    var subcategory = ReadCategory(reader.ReadSubtree());
                    if (subcategory != null)
                    {
                        category.Subcategories.Add(subcategory);
                    }
                    reader.Skip();
                }
            }

            return category;
        }

        private static SyndicationPerson ReadOwner(XmlReader reader)
        {
            var owner = new SyndicationPerson();

            reader.ReadStartElement();
            while (reader.IsStartElement())
            {
                if (reader.IsStartElement(ITunesConstants.EmailTag, ITunesConstants.ITunesNamespace))
                {
                    owner.Email = reader.ReadElementContentAsString();
                }
                else if (reader.IsStartElement(ITunesConstants.NameTag, ITunesConstants.ITunesNamespace))
                {
                    owner.Name = reader.ReadElementContentAsString();
                }
            }
            reader.ReadEndElement();

            return owner;
        }
    }
}
