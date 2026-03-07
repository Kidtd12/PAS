using Domain.ValueObjects;


namespace Domain.ValueObjects
{
    public class Quantity
    {
        public int Value { get; }

        public Quantity(int value)
        {
            if (value < 0)
                throw new System.ArgumentException("Quantity cannot be negative");

            Value = value;
        }
    }
}
