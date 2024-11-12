# WebService1

## Overview

WebService1 is an ASP.NET Core Web API project that provides an endpoint to calculate sales tax based on input data. The service sanitizes input text, checks for unclosed tags, extracts relevant data, and calculates the sales tax and total excluding tax.

## Features

- Sanitize input text by removing newline characters.
- Validate input text for unclosed tags.
- Extract total and cost center from input text.
- Calculate sales tax and total excluding tax.
- Return a detailed response with the calculated values.

## Requirements

- .NET 8
- ASP.NET Core

## Getting Started

### Prerequisites

- Install .NET 8 SDK from [here](https://dotnet.microsoft.com/download/dotnet/8.0).

### Running the Project

1. Clone the repository:git clone https://github.com/Smanitha-24/WebService1.git
cd WebService1

2. Build the project:dotnet build

3. Run the project:dotnet run
      
### Testing the API

You can test the API using tools like Postman or cURL. The endpoint to calculate sales tax is:/api/SalesTax/calculate

#### Sample Request
{ "Text": "Hi Patricia,
Please create an expense claim for the below. Relevant details are marked up as requested…
<expense><cost_centre>CC123</cost_centre><total>100</total><payment_method>personal
card</payment_method></expense>
From: William Steele
Sent: Friday, 16 June 2022 10:32 AM
To: Maria Washington
Subject: test
Hi Maria,
Please create a reservation for 10 at the <vendor>Seaside Steakhouse</vendor> for our
<description>development team’s project end celebration</description> on <date>27 April
2022</date> at 7.30pm.
Regards,
William", "TaxRate": 5}

#### Sample Response
{
  "totalIncludingTax": 100,
  "salesTax": 4.76,
  "totalExcludingTax": 95.24,
  "costCentre": "CC123",
  "taxRate": 0.05,
  "message": "Sales tax has been calculated at 5.00%. Total includes tax and has been split accordingly."
}

## Authentication

This project uses Basic Authentication for securing the API endpoints. Make sure to include the necessary authentication headers when making requests to the API.
Username : testuser Password : password

## Formatters

The project uses JSON formatters for serializing and deserializing the request and response bodies. Ensure that your requests are formatted as JSON.

## Project Structure

- `Controllers/SalesTaxController.cs`: Contains the main logic for handling the sales tax calculation.
- `Models/InputModel.cs`: Defines the input model for the API.
- `Models/ResponseModel.cs`: Defines the response model for the API.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any changes.


## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.


    