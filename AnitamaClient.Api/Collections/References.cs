using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api.Collections
{
    public static class References
    {
        public static ReferenceDictionary<int, Channel> Channels { get; } = new ReferenceDictionary<int, Channel>();
        public static ReferenceDictionary<int, Tag> Tags { get; } = new ReferenceDictionary<int, Tag>();
        public static ReferenceDictionary<string, Article> Articles { get; } = new ReferenceDictionary<string, Article>();
        public static ReferenceDictionary<int, Bangumi> Bangumis { get; } = new ReferenceDictionary<int, Bangumi>();
        public static ReferenceDictionary<EpisodeKey, Episode> Episodes { get; } = new ReferenceDictionary<EpisodeKey, Episode>();
        public static ReferenceDictionary<int, Topic> Topics { get; } = new ReferenceDictionary<int, Topic>();

        private static Dictionary<(Type key, Type value), object> referencesDic = new Dictionary<(Type, Type), object>
        {
            [(typeof(int), typeof(Channel))] = Channels,
            [(typeof(int), typeof(Tag))] = Tags,
            [(typeof(string), typeof(Article))] = Articles,
            [(typeof(int), typeof(Bangumi))] = Bangumis,
            [(typeof(EpisodeKey), typeof(Episode))] = Episodes,
            [(typeof(int), typeof(Topic))] = Topics,
        };

        internal static ReferenceDictionary<TKey, TValue> Get<TKey, TValue>()
            where TValue : class, IPrimeryKey<TKey>
        {
            return (ReferenceDictionary<TKey, TValue>)referencesDic[(typeof(TKey), typeof(TValue))];
        }
    }
}
