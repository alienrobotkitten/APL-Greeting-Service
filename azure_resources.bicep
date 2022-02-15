/* 
az deployment group create --resource-group helena-rg-dev --template-file azure_resources.bicep
*/
param appName string = 'pmq8aiwea'
param location string = resourceGroup().location

// storage accounts must be between 3 and 24 characters in length and use numbers and lower-case letters only
var storageAccountNameblob = '${appName}bicep945jfblob' 
var storageAccountNamelog = '${appName}bicep945jflog' 
var hostingPlanName = '${appName}bicep945jfhost'
var appInsightsName = '${appName}bicep945jfins'
var functionAppName = '${appName}app'

resource storageAccount_blob 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: storageAccountNameblob
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

resource storageAccount_log 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: storageAccountNamelog
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02-preview' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: { 
    Application_Type: 'web'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
  tags: {
    // circular dependency means we can't reference functionApp directly  /subscriptions/<subscriptionId>/resourceGroups/<rg-name>/providers/Microsoft.Web/sites/<appName>"
     'hidden-link:/subscriptions/${subscription().id}/resourceGroups/${resourceGroup().name}/providers/Microsoft.Web/sites/${functionAppName}': 'Resource'
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2020-10-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1' 
    tier: 'Dynamic'
  }
}

resource functionApp 'Microsoft.Web/sites@2020-06-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    httpsOnly: true
    serverFarmId: hostingPlan.id
    clientAffinityEnabled: true
    siteConfig: {
      appSettings: [
        {
          'name': 'APPINSIGHTS_INSTRUMENTATIONKEY'
          'value': appInsights.properties.InstrumentationKey
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount_blob.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount_blob.id, storageAccount_blob.apiVersion).keys[0].value}'
        }
        {
          'name': 'FUNCTIONS_EXTENSION_VERSION'
          'value': '~3'
        }
        {
          'name': 'FUNCTIONS_WORKER_RUNTIME'
          'value': 'dotnet'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount_blob.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount_blob.id, storageAccount_blob.apiVersion).keys[0].value}'
        }
        {
          name: 'LoggingStorageAccount'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount_log.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount_log.id, storageAccount_log.apiVersion).keys[0].value}'
        }
        // WEBSITE_CONTENTSHARE will also be auto-generated - https://docs.microsoft.com/en-us/azure/azure-functions/functions-app-settings#website_contentshare
        // WEBSITE_RUN_FROM_PACKAGE will be set to 1 by func azure functionapp publish
      ]
    }
  }
}
