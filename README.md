## Prerequisites ##

The following Azure Logic App demo solution uses connectors for the following services: 

* Google Account (Gmail, Drive, SpreadSheets) 
* Outlook.com email (not Office 365)
* DropBox

After the deployment you will need these accounts credentials to connect the logic app to the services. 

## Deployment ##


1. From the Demo folder upload the Expense_Report.xlsx file to your Google Drive Account
2. In Google Drive right click on the file and click on Open With -> Google SpreadSheets (this will convert the Excel file to a Google SpreadSheet) 

3. Click this button (hold CTRL while clicking to open in a new tab):

    <a target="_blank" id="deploy-to-azure"  href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Folandese%2FTechDays2017%2Fmaster%2FAzureResources%2FParkingTicketScanner.json"><img src="http://azuredeploy.net/deploybutton.png"/></a>

4. Fill the required parameters and click on Purchase
5. After the deployment go to the created Resource Group and click on the Logic App resource
6. Click on Edit 
7. Click on "Invalid Connection" for every connector with a question mark and login to allow the Logic App to talk with the service
8. Configure the DropBox Trigger Connector to watch the folder where the Parking tickets pictures will be saved
9. Configure the Google SpreadSheet Connector to select the worksheet uploaded in step 1 
10. Click on Save
11. Now copy the parking ticket image from the Demo folder to the DropBox folder you configured in step 8 and you will get a confirmation email (the address you filled in step 4), after approving it a new row will appear in the Google SpreadSheet! 
