namespace Server.BuildingBlocks
{
    public class SkipCountProvider
    {
        private int _skipCount;
        public int GetSkipCount()
        {
            return _skipCount;
        }

        public void Increment()
        {
            if(_skipCount > 10000 * 60 * 30)
            {
                _skipCount = 0;
            }
            else
            {
                _skipCount += 10000;
            }
        }
    }
}
