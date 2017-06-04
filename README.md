# PodcastSyndication
Extends [.NET's Syndication library](https://msdn.microsoft.com/en-us/library/system.servicemodel.syndication(v=vs.110).aspx) 
to allow for [iTunes Podcast RSS extensions](https://help.apple.com/itc/podcasts_connect/?lang=en#/itcb54353390). 
Inherits from classes like `SyndicationFeed` and `SyndicationItem`.

## How to use
```
PodcastSyndicationFeed feed;
using (var stream = File.OpenRead(path))
using (var xmlReader = new XmlTextReader(stream))
{
    feed = PodcastSyndicationFeed.Load<PodcastSyndicationFeed>(xmlReader);
}

Collection<SyndicationCategory> subcagtegories = feed.Categories.First().Subcategories;
Uri imageUrl = feed.ImageUrl;
```
