namespace WindowOffset.Models
{
    internal class Dimension
    {
        internal Dimension(float value)
        {
            this.Value = value;
        }

        internal float Value { get; private set; }

        internal float From { get; set; }
    }
}
