using Helpers;

namespace Models
{
    public class Die
    {
        public int[] Values { get; private set; }
        public Die(int[] values)
        {
            Values = values;
        }
        public override string ToString()
        {
            return string.Join(",", Values);
        }
    }
}