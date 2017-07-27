using AMS.Service;
using DCSSample;
using DeviceSample.API.AutoQuery.Model;
using ServiceStack;

namespace DeviceSample.API.AutoQuery
{
    public class DeviceSampleQueryService : AMSQueryServiceBase
    {
       public GetDeviceSampleResponse Get(GetAllDeviceSampleDto req)
        {
            var queryResult = ExecuteSimpleQuery<SourceDevice>(req);
            var resp = new GetDeviceSampleResponse { DeviceSamplesQueryResult = queryResult };
            return resp;
        }
    }
}
