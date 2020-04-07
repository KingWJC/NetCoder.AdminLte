namespace WebLiquid.Filters
{
    public class DotliquidJsonFilter
    {
        public static string ToJson(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
    }
}
