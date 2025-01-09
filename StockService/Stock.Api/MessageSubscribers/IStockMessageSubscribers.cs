using SharedUtils.Messages;

namespace Stock.Api.MessageSubscribers
{
    public interface IStockMessageSubscribers
    {
        Task HandleStockRollback(StockRollBackMessage stockRollbackMessage);
    }
}
