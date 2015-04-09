
namespace statsd.net.core.Structures
{
    public class MetricInfo
    {
        public string Name { get; private set; }
        public MetricTags Tags { get; private set; }
        public MetricInfo(string name) : this(name, new MetricTags(null)) { }

        public MetricInfo(string name, MetricTags tags)
        {
            Name = name;
            Tags = tags;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + 17 * Tags.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MetricInfo);
        }

        public bool Equals(MetricInfo obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.Name == null)
            {
                return obj.Name == null;
            }
            else
            {
                if (this.Name != obj.Name)
                {
                    return false;
                }
            }

            if (this.Tags == null)
            {
                return obj.Tags == null;
            }
            else
            {
                if (!this.Tags.Equals(obj.Tags))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
