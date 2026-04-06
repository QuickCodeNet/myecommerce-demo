namespace QuickCode.MyecommerceDemo.Gateway.KafkaProducer;

public interface IKafkaProducerWrapper
{
    Task ProduceAsync(string topic, string key, string message);
}