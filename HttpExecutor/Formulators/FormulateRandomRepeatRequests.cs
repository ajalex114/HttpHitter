
namespace HttpExecutor.Formulators
{
    using HttpExecutor.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    sealed class FormulateRandomRepeatRequests : AbstractFormulateRequestor
    {
        public FormulateRandomRepeatRequests() { }

        public override IEnumerable<ExecutorRequest> FormulateRequests()
        {
            var randomRequests = new List<ExecutorRequest>();
            Random random = new Random();

            // Get sequential request formulator algorithm
            var requestAlgorithm = Provider.GetRequestAlgorithm(Order.SequentialRepeat);

            // Get requests formulated by SequentialRequest
            var requests = requestAlgorithm.FormulateRequests().ToList();

            var randomLimit = requests.Count;

            for (int i = 0; i < requests.Count; i++)
            {
                var index = random.Next(0, randomLimit--);

                var item = requests[index];

                // Add the random item to result list
                randomRequests.Add(item);

                // Remove the random item from the original list so that it will not be considered again
                requests.Remove(item);
            }

            return randomRequests;
        }
    }
}
