using System.Collections.Generic;
using System.Linq;

namespace DemoCluster.DAL
{
    public class RepositoryResult
    {
        private readonly List<RepositoryError> _errors = new List<RepositoryError>();
        public bool Succeeded { get; protected set; }
        public IEnumerable<RepositoryError> Errors => _errors;
        public static RepositoryResult Success { get; } = new RepositoryResult { Succeeded = true };
        public static RepositoryResult Failed(params RepositoryError[] errors)
        {
            var result = new RepositoryResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }

            return result;
        }
        public override string ToString() =>
            Succeeded
                ? "Succeeded"
                : $"Failed : {string.Join(",", Errors.Select(x => x.Code).ToList())}";
    }
}