using System.Collections.ObjectModel;

namespace Epicast.PodcastSyndication
{
    public class SyndicationCategory
    {
        private Collection<SyndicationCategory> subcategories;


        public string Name { get; set; }
        public Collection<SyndicationCategory> Subcategories
        {
            get
            {
                if (subcategories == null)
                {
                    subcategories = new Collection<SyndicationCategory>();
                }
                return subcategories;
            }
        }
    }
}
