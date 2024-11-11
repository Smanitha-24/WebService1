using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace SalesTaxService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class SalesTaxController : ControllerBase
    {
        [HttpPost("calculate")]
        public IActionResult Calculate([FromBody] InputModel input)
        {
            // Sanitize the text by removing newline characters
            input.Text = input.Text.Replace("\n", " ").Replace("\r", " ");

            // Check for unclosed tags
            if (!Regex.IsMatch(input.Text, @"<total>.*?<\/total>") ||
                (input.Text.Contains("<cost_centre>") && !Regex.IsMatch(input.Text, @"<cost_centre>.*?<\/cost_centre>")))
            {
                return BadRequest("Input text contains unclosed tags.");
            }

            // Extract the total from the input text and remove commas
            var totalMatch = Regex.Match(input.Text, @"<total>([\d,]+(\.\d{1,2})?)<\/total>", RegexOptions.Singleline);
            if (!totalMatch.Success)
            {
                return BadRequest("Total not found in the input text.");
            }

            var totalString = totalMatch.Groups[1].Value.Replace(",", "");
            if (!decimal.TryParse(totalString, out var total))
            {
                return BadRequest("Invalid total value.");
            }

            // Extract the cost centre from the input text
            var costCentreMatch = Regex.Match(input.Text, @"<cost_centre>(.*?)<\/cost_centre>", RegexOptions.Singleline);
            var costCentre = costCentreMatch.Success ? costCentreMatch.Groups[1].Value : "UNKNOWN";

            // Extract the tax rate from the input json
            // Default tax rate of 10% (0.1 as decimal)
             decimal taxRate = 0.1m;  // Default set directly to 0.1 (10%)
            // Check if input.TaxRate is provided and valid (i.e., not null and a valid decimal)
            if (input.TaxRate != 0 && decimal.TryParse(input.TaxRate.ToString(), out decimal parsedTaxRate))
            {
                // Convert percentage to decimal form (e.g., 10 -> 0.1)
                taxRate = parsedTaxRate / 100;
            }
            else
            {
                // Handle the case where TaxRate is not a valid decimal value
                return BadRequest("Invalid tax rate. Please enter a valid decimal or percentage value.");
            }
            // Calculate sales tax and total excluding tax
            //  const decimal taxRate = 0.1m; // Example tax rate of 10%
            var salesTax = Math.Round(total * taxRate / (1 + taxRate), 2);
            var totalExcludingTax = total - salesTax;

            // Create the response model
            var response = new ResponseModel
            {
                TotalIncludingTax = total,
                SalesTax = salesTax,
                TotalExcludingTax = totalExcludingTax,
                CostCentre = costCentre,
                TaxRate = taxRate,
                message = $"Sales tax has been calculated at {taxRate * 100}%. Total includes tax and has been split accordingly."

               // message = "Sales tax has been calculated at "+taxRate+"%. Total includes tax and has been split accordingly."
            };

            return Ok(response);
        }
    }

    public class InputModel
    {
        public string Text { get; set; } = string.Empty;
        public decimal TaxRate { get; set; } = 10;
    }

    public class ResponseModel
    {
        public decimal TotalIncludingTax { get; set; }
        public decimal SalesTax { get; set; }
        public decimal TotalExcludingTax { get; set; }
        public string CostCentre { get; set; } = string.Empty;
        public decimal TaxRate { get; set; } = 0.1m;
        public string message { get; set; } = string.Empty;
    }
}
