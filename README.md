# PDF Generator
Help to merge and set up PDF documents.

### Introduction
Help to merge and populate PDF documents.  
  
**POST api/document/merge:**  Provide the documents to be merged.  Returns the Base64Encoded merged document.  
**POST api/document/create:**  Provide the unfilled document with form fields and the values to be used.  Returns the Base64Encoded filled document.  
**POST api/document/parsefields:**  Provide a document with form fields.  Returns the form fields as a list.  
**GET api/test:**  Returns a text saying Test called, to help you check if the service is alive.  

### Dependencies
This software uses <a href="https://itextpdf.com/en/products/itext-7/itext-7-core">iText7</a>, which uses an <a href="https://itextpdf.com/en/how-buy/legal/agpl-gnu-affero-general-public-license">AGPL license</a>.

### Deployment
Manual setup:  Download and build it, then you can use IIS to host it.

### Configuration
The configuration is set up to use <a href="https://serilog.net/">Serilog</a> for logging.  

### License
This uses an AGPL license.  
