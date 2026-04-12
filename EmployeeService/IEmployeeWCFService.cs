using System.ServiceModel;
using System.Threading.Tasks;
using System.ServiceModel.Web;


namespace EmployeeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IEmployeeWCFService
    {

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetEmployeeById?id={id}",
            ResponseFormat = WebMessageFormat.Json,  BodyStyle = WebMessageBodyStyle.Bare)]
        Task<EmployeeDto> GetEmployeeById(int id);

        // NOTE:
        // The sample data contains a potential self-reference (ManagerID = Id),
        // which may cause infinite recursion.
        //
        // The CTE-based implementation is efficient but may fail on cyclic data.
        // The in-memory implementation safely handles such cases.

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetEmployeeCTEById?id={id}",
        ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<EmployeeDto> GetEmployeeCTEById(int id);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "EnableEmployee?id={id}&enable={enable}", 
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Task EnableEmployee(int id, bool enable);
    }

	
}
