using DCSSample;
using ServiceStack;

namespace DeviceSample.API.AutoQuery.Model
{
    [Api("Get Device(s) using Servicestack AutoQuery")]
    [Route("/Device", "GET", Summary="Get Device")]
    public class GetAllDeviceSampleDto : QueryDb<DCSSample.SourceDevice>
                                        , ILeftJoin<DCSSample.SourceDevice, OperatingSystem>
                                        , ILeftJoin<DCSSample.SourceDevice, SourceDeviceType>
    {
        public string DeviceSampleStatusName { get; set; }
    }
}
