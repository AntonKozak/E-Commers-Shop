using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate;

public enum OrderStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "PaymentRecevied")]
    PaymentReceived,
    [EnumMember(Value = "PaymentFailed")]
    PaymentFailed,
    [EnumMember(Value = "StockConfirmed")]
    Shipped,
    [EnumMember(Value = "StockOnRoute")]
    OnRoute,
    [EnumMember(Value = "StockDelivered")]
    Delivered
}
