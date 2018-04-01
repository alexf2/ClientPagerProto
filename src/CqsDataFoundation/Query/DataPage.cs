using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CqsDataFoundation.Query
{
    [Serializable]
    public class DataPage<T>
    {
        /// <summary>
        /// Page data items
        /// </summary>
        [DataMember(Order = 1)]
        public IList<T> Data;

        /// <summary>
        /// Overall records count through all the pages
        /// This property is filled when IQueryPagged.ReturnTotalRecods is true.
        /// </summary>
        [DataMember(Order = 2)]
        public int TotalRecordsCount;

        /// <summary>
        /// Overall pages count
        /// </summary>
        [DataMember(Order = 3)]
        public int TotalPages;

        /// <summary>
        /// This page number. It may be less than requested one
        /// </summary>
        [DataMember(Order = 4)]
        public int PageNumber;

        /// <summary>
        /// An arbitrary custom data. May be used to tide your extra data to a page
        /// </summary>
        [DataMember(Order = 5)]
        public string Tag;

        public DataPage<T2> Convert<T2>(Func<T, T2> conversion)
        {
            var res = new DataPage<T2>()
            {
                TotalRecordsCount = this.TotalRecordsCount,
                TotalPages = this.TotalPages,
                PageNumber = this.PageNumber,
                Tag = this.Tag
            };

            if (Data == null)
                return res;

            var lst = res.Data = new List<T2>(Data.Count);
            for (int i = 0; i < Data.Count; ++i)
                lst[i] = conversion(Data[i]);

            return res;
        }
    }
}