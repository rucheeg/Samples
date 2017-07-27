using ServiceStack;

namespace DeviceSample.API.AutoQuery.Model
{
   public class GetDeviceSampleResponse
    {
        public ResponseStatus ResponseStatus { get; set; }
        public QueryResponse<DCSSample.SourceDevice> DeviceSamplesQueryResult { get; set; }
    }
}
