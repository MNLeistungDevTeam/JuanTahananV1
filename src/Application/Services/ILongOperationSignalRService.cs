namespace DMS.Application.Services;

public interface ILongOperationSignalRService
{
    Task RunLongOperation();
}
