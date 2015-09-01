using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PhoneData
{
    public class DataRetriever
    {
        private IList<PIStream> _streams;

        public DataRetriever()
        {
            _streams = new List<PIStream>();
        }

        public void Add(PIStream str)
        {
            _streams.Add(str);
        }

        public void GetData()
        {
            foreach (PIStream str in _streams)
            {
                str.GetData();
            }

        }

    }
}
