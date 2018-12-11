namespace SharedLibrary
{
    using System;

    public class OrderChecker
    {
        private readonly string[] status =
            {
                "Grinding beans",
                "Steaming milk",
                "Taking a sip (quality control)",
                "On transit to counter",
                "Picked up"
            };

        private readonly Random random;
        private int index;

        public OrderChecker(Random random)
        {
            this.random = random;
        }

        public CheckResult GetUpdate(int orderNo)
        {
            if (this.random.Next(1, 5) == 4)
            {
                if (this.status.Length - 1 > this.index)
                {
                    this.index++;
                    var result = new CheckResult
                    {
                        New = true,
                        Update = this.status[this.index],
                        Finished = this.status.Length - 1 == this.index
                    };
                    return result;
                }
            }

            return new CheckResult { New = false };
        }
    }
}
