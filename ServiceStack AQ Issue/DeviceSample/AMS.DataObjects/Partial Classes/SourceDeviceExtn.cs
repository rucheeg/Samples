using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCSSample;
using ServiceStack.DataAnnotations;

namespace DCSSample
{
    public partial class SourceDevice
    {
        [Reference]
        public DCSSample.OperatingSystem OperatingSystem { get; set; }
        [Reference]
        public SourceDeviceType SourceDeviceType { get; set; }
    }
}
