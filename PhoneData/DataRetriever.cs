using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PhoneData
{
    public class DataRetriever
    {
        private IList<Stream> _streams;

        public DataRetriever()
        {
            _streams = new List<Stream>();
        }

        public void Add(Stream str)
        {
            _streams.Add(str);
        }

        public void GetData()
        {
            foreach (Stream str in _streams)
            {
                str.GetData();
            }

        }

    }
}
