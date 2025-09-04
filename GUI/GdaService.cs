using FTN.Common;
using FTN.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public sealed class GdaService : IDisposable
    {
        private NetworkModelGDAProxy proxy;

        public GdaService()
        {
            proxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
            proxy.Open();
        }

        public void Dispose()
        {
            try
            {
                if (proxy != null)
                {
                    if (proxy.State == CommunicationState.Faulted) proxy.Abort();
                    else proxy.Close();
                }
            }
            catch { proxy?.Abort(); }
        }

        public List<long> GetExtentIds(ModelCode mc)
        {
            var ids = new List<long>();
            var mrd = new ModelResourcesDesc();
            var props = new List<ModelCode> { ModelCode.IDOBJ_GID }; // min

            int it = proxy.GetExtentValues(mc, props);
            int left = proxy.IteratorResourcesLeft(it);

            while (left > 0)
            {
                var rds = proxy.IteratorNext(Math.Min(200, left), it);
                ids.AddRange(rds.Select(r => r.Id));
                left = proxy.IteratorResourcesLeft(it);
            }

            proxy.IteratorClose(it);
            return ids;
        }

        public List<ResourceDescription> GetExtentValues(ModelCode mc, List<ModelCode> properties)
        {
            var results = new List<ResourceDescription>();
            int it = proxy.GetExtentValues(mc, properties);
            int left = proxy.IteratorResourcesLeft(it);

            while (left > 0)
            {
                var rds = proxy.IteratorNext(Math.Min(200, left), it);
                results.AddRange(rds);
                left = proxy.IteratorResourcesLeft(it);
            }

            proxy.IteratorClose(it);
            return results;
        }

        public ResourceDescription GetValues(long gid, List<ModelCode> properties)
        {
            return proxy.GetValues(gid, properties);
        }

        public List<ResourceDescription> GetRelatedValues(long sourceGid, ModelCode associationProperty, ModelCode targetTypeOrZero, List<ModelCode> properties)
        {
            var assoc = new Association(associationProperty, targetTypeOrZero, false);
            var results = new List<ResourceDescription>();

            int it = proxy.GetRelatedValues(sourceGid, properties, assoc);
            int left = proxy.IteratorResourcesLeft(it);

            while (left > 0)
            {
                var rds = proxy.IteratorNext(Math.Min(200, left), it);
                results.AddRange(rds);
                left = proxy.IteratorResourcesLeft(it);
            }

            proxy.IteratorClose(it);
            return results;
        }
    }
}
