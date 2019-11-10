using System.Collections.Generic;

namespace PlumPack.Infrastructure
{
    public class SeekedList<T> : List<T>
    {
        public SeekedList(IEnumerable<T> source, int? skipped, int? taken, long totalCount)
        {
            Skipped = skipped;
            Taken = taken;
            TotalCount = totalCount;
            AddRange(source);
        }
        public int? Skipped { get; }

        public int? Taken { get; }

        public long TotalCount { get; }
    }
}