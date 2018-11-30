using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Service.Serialization
{
    public static class DateTimeFormatExtensions
    {
        public static string ToIso8601(this DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm");
        }
    }

    public struct Iso8601Timestamp : IComparable<Iso8601Timestamp>
    {
        private readonly DateTime _innerDate;

        public DateTime InnerDate
        {
            get
            {
                return _innerDate;
            }
        }

        public Iso8601Timestamp(DateTime date) : this()
        {

            _innerDate = date;
        }

        public Iso8601Timestamp(String dateStr) : this()
        {

            DateTime date;

            if (DateTime.TryParse(dateStr, out date))
            {
                _innerDate = date;
                return;
            }

            _innerDate = DateTime.ParseExact(dateStr, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

        }

        public static bool operator ==(Iso8601Timestamp c1, Iso8601Timestamp c2)
        {
            return c1._innerDate == c2._innerDate;
        }

        public static bool operator !=(Iso8601Timestamp c1, Iso8601Timestamp c2)
        {
            return c1._innerDate != c2._innerDate;
        }

        public static bool operator >(Iso8601Timestamp c1, Iso8601Timestamp c2)
        {
            return c1._innerDate > c2._innerDate;
        }

        public static bool operator <(Iso8601Timestamp c1, Iso8601Timestamp c2)
        {
            return c1._innerDate < c2._innerDate;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(DateTime)) return this._innerDate == (DateTime)obj;
            if (obj.GetType() == typeof(Iso8601Timestamp)) return this == ((Iso8601Timestamp)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return _innerDate.GetHashCode();
        }

        public static implicit operator DateTime(Iso8601Timestamp date)
        {
            return date._innerDate;
        }

        public static implicit operator Iso8601Timestamp(DateTime date)
        {
            return new Iso8601Timestamp(date);
        }

        public override string ToString()
        {
            return _innerDate.ToIso8601();
        }

        public int CompareTo(Iso8601Timestamp obj)
        {
            if (this.InnerDate > obj.InnerDate) return 1;
            if (this.InnerDate == obj.InnerDate) return 0;
            return -1;
        }
    }
}

