namespace Backgammon_v2
{
    public class Spike
    {
        public int SpikeId { get; set; }
        public int SoldiersCount { get; set; }
        public bool Black { get; set; }
        public bool Marked { get; set; }
        public bool OutMode { get; set; }
        public bool PreviewMode { get; set; }


        public Spike()
        {
            PreviewMode = false;
            SoldiersCount = 0;
            Black = true;
            Marked = false;
            OutMode = false;
        }

        public Spike(int soldiersCount, bool black)
        {
            PreviewMode = false;
            SoldiersCount = soldiersCount;
            Black = black;
            Marked = false;
        }

        public bool IsEmpty()
        {
            return SoldiersCount == 0;
        }

        public override string ToString()
        {
            return $"SoldiersCount = {SoldiersCount}, {(Black ? "Black" : "White")} Player";
        }
    }
}