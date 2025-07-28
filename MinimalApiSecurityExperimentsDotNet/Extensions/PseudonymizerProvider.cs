namespace MinimalApiSecurityExperimentsDotNet.Extensions
{

    public class PseudonymizerProvider
    {

        private Pseudonymizer? _pseudonymizer;

        public void Set(Pseudonymizer pseudonymizer)
        {
            _pseudonymizer = pseudonymizer;
        }

        public Pseudonymizer Get()
        {
            return _pseudonymizer ?? throw new InvalidOperationException("Pseudonymizer not initalized");
        }

    }

}
