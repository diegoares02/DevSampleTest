using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSample
{
    class SampleGenerator
    {


        private readonly DateTime _sampleStartDate;
        private readonly TimeSpan _sampleIncrement;

        private readonly List<Sample> _sampleList;


        public SampleGenerator(DateTime sampleStartDate, TimeSpan sampleIncrement)
        {
            _sampleList = new List<Sample>();
            _sampleStartDate = sampleStartDate;
            _sampleIncrement = sampleIncrement;
        }


        /// <summary>
        /// Samples should be a time-descending ordered list
        /// </summary>
        public List<Sample> Samples { get { return _sampleList; } }


        public int SamplesValidated { get; private set; }


        public void LoadSamples(int samplesToGenerate)
        {

            //TODO: can we load samples faster?

            _sampleList.Clear();
            Queue<Sample> samples = new Queue<Sample>();

            DateTime date = _sampleStartDate;

            for (int i = 0; i < samplesToGenerate; i++)
            {
                Sample s = new Sample(i == 0);
                s.LoadSampleAtTime(date);

                samples.Enqueue(s);

                date += _sampleIncrement;
            }

            _sampleList.AddRange(samples.ToList());
        }


        public void ValidateSamples()
        {

            //TODO: can we validate samples faster?

            int samplesValidated = 0;

            Queue<Sample> samples = new Queue<Sample>(_sampleList);
            var sample = samples.Dequeue();
            Sample previous = null;
            while (samples.Count>0)
            {
                if (sample.ValidateSample(previous, _sampleIncrement)) //in this sample the ValidateSample is always true but assume that's not always the case
                    samplesValidated++;
                previous = sample;
                sample = samples.Dequeue();
            }

            SamplesValidated = samplesValidated;

        }



    }
}
