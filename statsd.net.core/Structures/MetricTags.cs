using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace statsd.net.core.Structures
{

    public class MetricTags : IEnumerable<string>
    {
        private readonly string[] tags;
        private readonly Int32 hashCode;
        public MetricTags(string[] tags)
        {
            if (tags == null)
            {
                this.tags = null;
                hashCode = 0;
            }
            else
            {
                this.tags = tags;
                Array.Sort(this.tags);
                int hash = 17;

                // get hash code for all items in array
                foreach (var tag in tags)
                {
                    hash = hash * 23 + ((tag != null) ? tag.GetHashCode() : 0);
                }
                this.hashCode = hash;
            }
        }

        public override int GetHashCode()
        {
            return hashCode;
        }


        public override bool Equals(object obj)
        {
            MetricTags other = obj as MetricTags;
            return Equals(other);
        }

        protected bool Equals(MetricTags other)
        {
            if (other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            string[] firstArray = this.tags;
            string[] secondArray = other.tags;

            if (firstArray == null && secondArray == null)
            {
                return true;
            }

            if (firstArray != null && secondArray != null &&
                (firstArray.Length == secondArray.Length))
            {
                for (int i = 0; i < firstArray.Length; i++)
                {
                    // if any mismatch, not equal
                    if (!object.Equals(firstArray[i], secondArray[i]))
                    {
                        return false;
                    }
                }

                // if no mismatches, equal
                return true;
            }

            // if we get here, they are not equal
            return false;
        }

        public IEnumerator<string> GetEnumerator()
        {
            if (tags == null)
            {
                yield break;
            }

            foreach (string o in tags)
            {
                yield return o;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
