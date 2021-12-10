using System.Text.Json.Serialization;

namespace orderprocessor.svc.Dapr;

public class CloudEvent<TCloudEventPayload>
{
    // {"datacontenttype":"application/json","type":"com.dapr.event.sent","topic":"neworder","data":{"basketId":"2","line

    [JsonPropertyName("datacontenttype")]
    public string DataContentType { get; set; } = "";

    [JsonPropertyName("eventtype")]
    public string EventType { get; set; } = "";
    
    [JsonPropertyName("topic")]
    public string Topic { get; set; }="";


    [JsonPropertyName("data")]
    public TCloudEventPayload Data { get; set; }

    public override string ToString()
    {
        return $"Topic={Topic} EventType={EventType} DataContentType={DataContentType} DataType={typeof(TCloudEventPayload)}";
    }
}