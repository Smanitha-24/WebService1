using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;

public class InputFormatter : TextInputFormatter
{
    public InputFormatter()
    {
        SupportedMediaTypes.Add("application/json");
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        using var reader = new StreamReader(context.HttpContext.Request.Body, encoding);
        var rawJson = await reader.ReadToEndAsync();

        // Remove newline characters to sanitize the JSON
        var sanitizedJson = rawJson.Replace("\n", " ").Replace("\r", " ");

        try
        {
            // Deserialize the sanitized JSON into the expected model type
            var deserializedObject = JsonConvert.DeserializeObject(sanitizedJson, context.ModelType);

            return await InputFormatterResult.SuccessAsync(deserializedObject);
        }
        catch (Newtonsoft.Json.JsonException) // This will now refer to Newtonsoft.Json.JsonException
        {
            // Return an error if JSON deserialization fails
            context.ModelState.AddModelError(context.ModelName, "Invalid JSON format.");
            return await InputFormatterResult.FailureAsync();
        }
    }
}
