using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day05;

internal class Map
{
    internal class MapRange
    {
        public MapRange(long sourceStart, long destinationStart, long range)
        {
            this.SourceStart = sourceStart;
            this.DestinationStart = destinationStart;
            this.Range = range;
        }
        internal long SourceStart { get; }
        internal long DestinationStart { get; }
        internal long Range { get; }
    }
    public Map(int order, string name)
    {
        this.Order = order;
        this.Name = name;
        this.Ranges = new List<MapRange>();
    }
    internal int Order { get; } = 0;
    internal string Name { get; } = string.Empty;
    internal IList<MapRange> Ranges { get; set; }

    internal void FillSourceToDesination(long source, long destination, long range) => this.Ranges.Add(new MapRange(source, destination, range));
    internal long GetDestination(long source)
    {
        long toReturn = source;
        MapRange? range = this.Ranges.FirstOrDefault(r => source >= r.SourceStart && source < r.SourceStart + r.Range);
        if(range != null)
        {
            long diff = source - range.SourceStart;
            toReturn = range.DestinationStart + diff;
        }
        return toReturn;
    }
}

/*
    internal IDictionary<long, long> SourceToDestination { get; } = new Dictionary<long, long>();
    internal void FillSourceToDesination(long source, long destination, long length)
    {
        for(int idx = 0; idx < length; idx++)
        {
            this.SourceToDestination[source + idx] = destination + idx;
        }
    }

    internal long GetDestination(long source) => this.SourceToDestination.ContainsKey(source)
        ? this.SourceToDestination[source]
        : source;
 */
